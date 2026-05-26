using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // maquina de estados
    protected FSM enemyFSM;

    // referencias
    protected Rigidbody enemyRigidbody;

    protected Transform player;
    public Transform Player => player;

    protected Animator enemyAnimator;

    protected HealthSystem enemyHealthSystem;

    // IMPORTANTE:
    // ahora se asigna manualmente desde inspector
    [SerializeField] protected Transform enemyVisualTransform;

    protected Collider enemyCollider;

    // variables movimiento
    [SerializeField] private float moveSpeed;

    // combate
    [SerializeField] private float distanceToAttack;
    public float DistanceToAttack => distanceToAttack;

    // hit flags
    public bool wasHitted { get; private set; }

    // muerte
    public bool isDead { get; private set; }

    // vfx
    [SerializeField] private GameObject prefabHitEffect;

    // tiempo destroy enemigos normales
    [SerializeField] private float destroyAfterDeathTime = 3f;

    // evita múltiples hits instantáneos
    private bool canReceiveDamage = true;

    [SerializeField] private float hitCooldown = 0.2f;

    void Awake()
    {
        enemyRigidbody = GetComponent<Rigidbody>();

        enemyAnimator = GetComponentInChildren<Animator>();

        enemyHealthSystem = GetComponent<HealthSystem>();

        enemyCollider = GetComponent<Collider>();

        // seguridad extra
        if (enemyVisualTransform == null)
        {
            Debug.LogError(
                gameObject.name +
                " NO tiene asignado enemyVisualTransform");
        }
    }

    protected virtual void Start()
    {
        // crear fsm
        enemyFSM = new FSM(this);

        // buscar jugador una sola vez
        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    protected virtual void Update()
    {
        // ejecutar fsm
        enemyFSM.Update();
    }

    public void MoveToPlayer()
    {
        if (player == null) return;

        Vector3 direccion = player.position - transform.position;

        // ignorar altura
        direccion.y = 0f;

        direccion.Normalize();

        enemyRigidbody.velocity =
            direccion * moveSpeed;

        RotateVisual(direccion);
    }

    public void MoveTowards(Vector3 target)
    {
        Vector3 direccion = target - transform.position;

        // ignorar altura
        direccion.y = 0f;

        direccion.Normalize();

        enemyRigidbody.velocity = direccion * moveSpeed;

        // rotación correcta
        if (direccion.x > 0.1f)
        {
            enemyVisualTransform.rotation =
                Quaternion.Euler(0f, 0f, 0f);
        }
        else if (direccion.x < -0.1f)
        {
            enemyVisualTransform.rotation =
                Quaternion.Euler(0f, 180f, 0f);
        }
    }

    // reutilizamos rotación
    private void RotateVisual(Vector3 direccion)
    {
        if (enemyVisualTransform == null) return;

        if (direccion.x > 0f)
        {
            enemyVisualTransform.rotation =
                Quaternion.Euler(0f, 0f, 0f);
        }
        else if (direccion.x < 0f)
        {
            enemyVisualTransform.rotation =
                Quaternion.Euler(0f, 180f, 0f);
        }
    }

    private float distanceToPlayer;

    public float DistanceToPlayer()
    {
        if (player == null) return Mathf.Infinity;

        distanceToPlayer =
            Vector3.Distance(
                player.position,
                transform.position);

        return distanceToPlayer;
    }

    public void OnTriggerEnter(Collider col)
    {
        // ya murió
        if (isDead) return;

        // invulnerabilidad temporal
        if (!canReceiveDamage) return;

        // ataque jugador
        if (col.CompareTag("PlayerAttackZone"))
        {
            canReceiveDamage = false;

            StartCoroutine(HitCooldownCoroutine());

            // daño
            enemyHealthSystem.Damage(2);

            // efecto golpe
            if (prefabHitEffect != null)
            {
                Instantiate(
                    prefabHitEffect,
                    transform.position,
                    Quaternion.identity);
            }

            // muerte
            if (enemyHealthSystem.CurrentHealth <= 0)
            {
                isDead = true;

                enemyAnimator.SetTrigger("die");

                enemyRigidbody.velocity =
                    Vector3.zero;

                enemyCollider.enabled = false;

                StartCoroutine(DestroyEnemyAfterTime());
            }
            else
            {
                wasHitted = true;
            }
        }
    }

    private IEnumerator HitCooldownCoroutine()
    {
        yield return new WaitForSeconds(hitCooldown);

        canReceiveDamage = true;
    }

    public void ResetHitted()
    {
        wasHitted = false;
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void DisableCollider()
    {
        enemyCollider.enabled = false;
    }

    public void EnableCollider()
    {
        enemyCollider.enabled = true;
    }

    private IEnumerator DestroyEnemyAfterTime()
    {
        yield return new WaitForSeconds(
            destroyAfterDeathTime);

        Destroy(gameObject);
    }
}