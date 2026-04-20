using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerCombat : MonoBehaviour
{
    public Transform swordTransform; 
    public Transform attackPoint;   
    public LayerMask enemyLayers;  

    public float attackDamage = 25f;
    public float attackRange = 2f;  
    public float attackCooldown = 0.6f;
    public Vector3 attackRotation = new Vector3(0, 45, 0); 
    public float swingSpeed = 12f;
    public float returnSpeed = 6f;
    public float lungeForce = 4f;

    private Rigidbody _rb;
    private Quaternion _startRotation;
    private bool _isAttacking = false;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        if (swordTransform != null)
            _startRotation = swordTransform.localRotation;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !_isAttacking)
        {
            StartCoroutine(PerformAttack());
        }
    }

    IEnumerator PerformAttack()
    {
        _isAttacking = true;

        _rb.AddForce(transform.forward * lungeForce, ForceMode.Impulse);

        Quaternion targetRot = _startRotation * Quaternion.Euler(attackRotation);
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * swingSpeed;
            swordTransform.localRotation = Quaternion.Lerp(swordTransform.localRotation, targetRot, t);
            yield return null;
        }

        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider enemy in hitEnemies)
        {
            Health enemyHealth = enemy.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);
            }
        }

        t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * returnSpeed;
            swordTransform.localRotation = Quaternion.Lerp(swordTransform.localRotation, _startRotation, t);
            yield return null;
        }

        yield return new WaitForSeconds(attackCooldown);
        _isAttacking = false;
    }

}