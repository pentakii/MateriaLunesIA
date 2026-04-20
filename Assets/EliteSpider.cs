using UnityEngine;

public class EliteSpider : MonoBehaviour
{
    public Transform player;
    public LineOfSight los;
    public Health health;
    private Animator animator;

    [SerializeField] private float moveSpeed = 5f;

    [SerializeField] private float decisionDelay = 0.3f;

    [SerializeField] private float wanderDirectionChangeTime = 0.5f;
    [SerializeField] private float wanderAngle = 25f;

    [SerializeField] private float attackRange = 1.8f;
    [SerializeField] private float damageAmount = 15f;
    [SerializeField] private float attackCooldown = 1.2f;

    private float _nextAttackTime;
    private bool isAttacking;

    private Node currentAction;
    private Vector3 _wDir;

    private float decisionTimer;
    private float wanderTimer;

    void Awake()
    {
        _wDir = transform.forward;
        animator = GetComponent<Animator>();
        currentAction = new ActionNode(s => s.DoWander());
    }

    void Update()
    {
        if (isAttacking)
        {
            currentAction?.Evaluate(this);
            return;
        }

        decisionTimer += Time.deltaTime;

        if (decisionTimer >= decisionDelay)
        {
            currentAction = Decide();
            decisionTimer = 0f;
        }

        currentAction?.Evaluate(this);
    }

    Node Decide()
    {
        if (los.CanSeeTarget(transform, player))
        {
            float distance = Vector3.Distance(transform.position, player.position);

            if (health.currentHealth < 30)
                return new ActionNode(s => s.DoFlee());

            if (distance <= attackRange)
                return new ActionNode(s => s.DoAttack());

            return new ActionNode(s => s.DoPursue());
        }

        return new ActionNode(s => s.DoWander());
    }

    public void DoWander()
    {
        animator.SetBool("isWalking", true);

        wanderTimer += Time.deltaTime;

        if (wanderTimer >= wanderDirectionChangeTime)
        {
            _wDir = SteeringBehaviours.Wander(_wDir, wanderAngle);
            wanderTimer = 0f;
        }

        Move(_wDir);
    }

    public void DoPursue()
    {
        animator.SetBool("isWalking", true);
        Move(SteeringBehaviours.Seek(transform, player.position));
    }

    public void DoFlee()
    {
        animator.SetBool("isWalking", true);
        Move(SteeringBehaviours.Flee(transform, player.position));
    }

    public void DoAttack()
    {
        Move(Vector3.zero);
        animator.SetBool("isWalking", false);

        if (Time.time >= _nextAttackTime)
        {
            isAttacking = true;

            animator.SetTrigger("attack");

            Health playerHealth = player.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
                Debug.Log("¡La Araña te atacó!");
            }
            _nextAttackTime = Time.time + attackCooldown;

            Invoke(nameof(EndAttack), 0.5f);
        }
    }

    void EndAttack()
    {
        isAttacking = false;
    }

    void Move(Vector3 dir)
    {
        dir.y = 0f;
        dir = dir.normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;
        if (dir != Vector3.zero)
        {
            transform.forward = Vector3.Lerp(
                transform.forward,
                dir,
                Time.deltaTime * 8f
            );
        }
    }
}