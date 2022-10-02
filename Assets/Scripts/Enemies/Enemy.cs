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

        if (Vector3.SqrMagnitude(transform.position - player.transform.position) > (stopDistance * stopDistance)) {
            animator.SetBool(Strings.TRIGGER_WALKING, true);
        } else if (activeAttackDelay <= 0f) {
            StartAttack();
        }
    }

    private void FixedUpdate() {
        if (player.transform.position.x > transform.position.x ) {
            myRenderer.flipX = true;
        }
    }

    private void StartAttack() {
        isAttacking = true;
        aiPath.enabled = false;
        animator.SetTrigger(Strings.TRIGGER_ATTACK);
    }

    public void Attack() {
        if (!markedForDeath && Vector3.SqrMagnitude(transform.position - player.transform.position) <= (hitDistance * hitDistance)) {
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
        if (destroyOnCollisionWithPlayer && collision.collider.CompareTag(Strings.TAG_PLAYER)) {
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
        Destroy(gameObject, hitDelay);
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
