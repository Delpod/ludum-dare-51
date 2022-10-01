using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] int damage = 10;
    [SerializeField] int health = 100;
    [SerializeField] float stopDistance = 1f;

    Player player;

    private void Start() {
        player = FindObjectOfType<Player>();
    }

    private void Update() {
        if (Vector3.SqrMagnitude(transform.position - player.transform.position) > stopDistance) {
            transform.position = new Vector3(Mathf.MoveTowards(transform.position.x, player.transform.position.x, Time.deltaTime * walkSpeed), Mathf.MoveTowards(transform.position.y, player.transform.position.y, Time.deltaTime * walkSpeed), 0f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.CompareTag(Strings.TAG_PLAYER)) {
            player.GetDamage(damage);
        }
    }

    public void GetDamage(int damage) {
        health -= damage;
        
        if (health <= 0) {
            Destroy(gameObject);
        }
    }
}
