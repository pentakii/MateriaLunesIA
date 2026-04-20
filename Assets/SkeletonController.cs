using UnityEngine;

public class SkeletonController : MonoBehaviour
{
    public enum EnemyState { Patrol, Pursuit, Attack, Flee }

    public Transform player;
    public LineOfSight los;
    public Rigidbody rb;



    public EnemyState currentState = EnemyState.Patrol;
    public float fleeThreshold = 25f;
    public float attackRange = 2f;
    public float speed = 7f;

    public float damageAmount = 10f;
    public float attackCooldown = 1.0f;
    private float _nextAttackTime;

    private Renderer _renderer;
    private Health myHealth;
    private Animator animator;


    void Awake()
    {
        los = GetComponent<LineOfSight>();
        rb = GetComponent<Rigidbody>();

        _renderer = GetComponentInChildren<Renderer>();
        animator = GetComponent<Animator>();
        myHealth = GetComponent<Health>();
    }

    void Update()
    {
        bool canSeePlayer = los.isInRange(transform, player) &&
                            los.isInAngle(transform, player) &&
                            los.hasLineOfSight(transform, player);

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (myHealth.currentHealth < fleeThreshold)
            currentState = EnemyState.Flee;
        else if (canSeePlayer && distanceToPlayer <= attackRange)
            currentState = EnemyState.Attack;
        else if (canSeePlayer)
            currentState = EnemyState.Pursuit;
        else
            currentState = EnemyState.Patrol;

        ExecuteState();
    }

    void ExecuteState()
    {
        switch (currentState)
        {
            case EnemyState.Patrol:
                _renderer.material.color = Color.white;
                animator.SetBool("isWalking", false);
                Patrol();
                break;

            case EnemyState.Pursuit:
                _renderer.material.color = Color.yellow;
                animator.SetBool("isWalking", true);
                MoveTowards(player.position);
                break;

            case EnemyState.Attack:
                _renderer.material.color = Color.red;
                animator.SetBool("isWalking", false);
                TryDamagePlayer();
                break;

            case EnemyState.Flee:
                _renderer.material.color = Color.blue;
                animator.SetBool("isWalking", true);
                Escape();
                break;
        }
    }

    void MoveTowards(Vector3 target)
    {
        Vector3 dir = SteeringBehaviours.Seek(transform, target);

        rb.MovePosition(transform.position + dir * speed * Time.deltaTime);

        if (dir != Vector3.zero)
            transform.forward = dir;
    }

    void Escape()
    {
        Vector3 dir = SteeringBehaviours.Flee(transform, player.position);

        rb.MovePosition(transform.position + dir * speed * Time.deltaTime);

        if (dir != Vector3.zero)
            transform.forward = dir;
    }

    void Patrol()
    {
        transform.Rotate(Vector3.up * 30f * Time.deltaTime);
    }
    void TryDamagePlayer()
    {
        if (Time.time >= _nextAttackTime)
        {
            Health playerHealth = player.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
                Debug.Log("¡El Esqueleto te ha golpeado!");
                animator.SetTrigger("attack");
                _nextAttackTime = Time.time + attackCooldown;
            }
        }
    }

}