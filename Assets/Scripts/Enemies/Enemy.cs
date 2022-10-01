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

    private Player player;
    private float activeDelay = -1f;
    private float activeAttackDelay = -1f;
    private Shader shaderGUIText;
    private Shader URPShader;
    private SpriteRenderer myRenderer;
    private bool rendererChanged = true;
    private Animator animator;
    private bool isAttacking = false;

    private void Start() {
        animator = GetComponent<Animator>();
        myRenderer = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<Player>();
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
        }

        if (activeAttackDelay > 0f) {
            activeAttackDelay -= Time.deltaTime;
        }

        if (isAttacking) {
            return;
        }

        if (Vector3.SqrMagnitude(transform.position - player.transform.position) > (stopDistance * stopDistance)) {
            transform.position = new Vector3(Mathf.MoveTowards(transform.position.x, player.transform.position.x, Time.deltaTime * walkSpeed), Mathf.MoveTowards(transform.position.y, player.transform.position.y, Time.deltaTime * walkSpeed), 0f);
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
        animator.SetTrigger(Strings.TRIGGER_ATTACK);
    }

    public void Attack() {
        if (Vector3.SqrMagnitude(transform.position - player.transform.position) <= (hitDistance * hitDistance)) {
            player.GetDamage(damage);
        }
    }

    public void FinishAttack() {
        isAttacking = false;
        activeAttackDelay = attackDelay;
        animator.SetBool(Strings.TRIGGER_WALKING, false);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.CompareTag(Strings.TAG_PLAYER)) {
            player.GetDamage(damage);
            if (destroyOnCollisionWithPlayer) {
                HandleDeath();
            }
        }
    }

    private void HandleDeath() {
        Destroy(gameObject);
    }

    public void GetDamage(int damage) {
        health -= damage;
        
        if (health <= 0) {
            HandleDeath();
            return;
        }

        activeDelay = hitDelay;
        rendererChanged = false;
        myRenderer.material.shader = shaderGUIText;
        animator.enabled = false;
    }
}
