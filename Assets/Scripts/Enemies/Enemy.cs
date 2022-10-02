using Pathfinding;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] int damage = 10;
    [SerializeField] int health = 100;
    [SerializeField] float stopDistance = 1.1f;
    [SerializeField] float hitDistance = 1.75f;
    [SerializeField] float hitDelay = 0.2f;
    [SerializeField] float attackDelay = 1f;
    [SerializeField] bool destroyOnCollisionWithPlayer = false;
    [SerializeField] AudioClip[] hitClips;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] bool isShooting = false;
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] LayerMask raycastMask;
    [SerializeField] float bulletSpeed = 400f;

    private Player player;
    private float activeDelay = -1f;
    private float activeAttackDelay = -1f;
    private Shader shaderGUIText;
    private Shader URPShader;
    private SpriteRenderer myRenderer;
    private bool rendererChanged = true;
    private Animator animator;
    private bool isAttacking = false;
    private AIPath aiPath;
    private AIDestinationSetter aiDestinationSetter;
    private bool markedForDeath = false;
    private bool canShoot = false;

    private void Start() {
        aiDestinationSetter = GetComponent<AIDestinationSetter>();
        aiPath = GetComponent<AIPath>();
        animator = GetComponent<Animator>();
        myRenderer = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<Player>();
        aiDestinationSetter.target = player.transform;
        shaderGUIText = Shader.Find(Strings.SHADER_GUI_TEXT);
        URPShader = Shader.Find(Strings.SHADER_URP);
    }

    private void Update() {
        if (activeDelay > 0f) {
            activeDelay -= Time.deltaTime;
            return;
        } else if (!rendererChanged) {
            myRenderer.material.shader = URPShader;
            rendererChanged = true;
            animator.enabled = true;
            aiPath.canMove = true;
        }

        if (activeAttackDelay > 0f) {
            activeAttackDelay -= Time.deltaTime;
        }

        if (isAttacking) {
            return;
        }

        if (isShooting) {
            animator.SetBool(Strings.TRIGGER_WALKING, !canShoot);
            if (canShoot && activeAttackDelay <= 0f) {
                StartShooting();
            }
        } else {
            if (Vector3.SqrMagnitude(transform.position - player.transform.position) > (stopDistance * stopDistance)) {
                animator.SetBool(Strings.TRIGGER_WALKING, true);
            } else if (activeAttackDelay <= 0f) {
                StartAttack();
            }
        }
    }

    private void FixedUpdate() {
        myRenderer.flipX = player.transform.position.x > transform.position.x;

        if (isShooting) {
            RaycastHit2D raycastHit = Physics2D.CircleCast(transform.position, 0.2f, player.transform.position - transform.position, 100f, raycastMask.value);
            if (raycastHit.transform && raycastHit.transform.CompareTag(Strings.TAG_PLAYER)) {
                aiPath.canMove = false;
                canShoot = true;
            } else {
                aiPath.canMove = true;
                canShoot = false;
            }
        }
    }

    private void StartAttack() {
        isAttacking = true;
        aiPath.enabled = false;
        animator.SetTrigger(Strings.TRIGGER_ATTACK);
    }

    private void StartShooting() {
        isAttacking = true;
        aiPath.enabled = false;
        animator.SetTrigger(Strings.TRIGGER_ATTACK);
    }

    public void Attack() {
        if (markedForDeath) {
            return;
        }

        if (isShooting) {
            Vector3 dir = player.transform.position - transform.position;
            GameObject go = Instantiate(bulletPrefab.gameObject, transform.position, Quaternion.Euler(0f, 0f, -Mathf.Sign(dir.x) * Vector3.Angle(transform.up, dir)));

            go.GetComponent<Bullet>().damage = damage;
            go.GetComponent<Rigidbody2D>().AddForce(go.transform.up * bulletSpeed);
        } else if (Vector3.SqrMagnitude(transform.position - player.transform.position) <= (hitDistance * hitDistance)) {
            player.GetDamage(damage);
        }
    }

    public void FinishAttack() {
        isAttacking = false;
        aiPath.enabled = true;
        activeAttackDelay = attackDelay;
        animator.SetBool(Strings.TRIGGER_WALKING, false);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (!markedForDeath && destroyOnCollisionWithPlayer && collision.collider.CompareTag(Strings.TAG_PLAYER)) {
            player.GetDamage(damage);
            GetDamage(health);
        }
    }

    private void HandleDeath() {
        if (markedForDeath) {
            return;
        }

        markedForDeath = true;
        GameManager.MonsterKilled();
        GameObject particlesGO = Instantiate(deathParticles.gameObject, transform.position, Quaternion.identity);
        Destroy(gameObject, hitDelay);
        Destroy(particlesGO, 1f);
    }

    public void GetDamage(int damage) {
        if (markedForDeath) {
            return;
        }

        health -= damage;

        AudioSource.PlayClipAtPoint(Helpers.RandomElement(hitClips), transform.position, 1f);
        CameraShake.instance.ShakeCamera(1f, 0.25f);

        if (health <= 0) {
            HandleDeath();
        }

        activeDelay = hitDelay;
        rendererChanged = false;
        myRenderer.material.shader = shaderGUIText;
        animator.enabled = false;
        aiPath.canMove = false;
    }
}
