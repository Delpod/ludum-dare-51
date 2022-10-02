using UnityEngine;

public class Bullet : MonoBehaviour {
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] bool hitEnemy = true;

    public int damage = 25;

    private void Update() {
        transform.Rotate(new Vector3(0f, 0f, rotationSpeed * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (hitEnemy && other.CompareTag(Strings.TAG_ENEMY)) {
            other.GetComponent<Enemy>().GetDamage(damage);
        } else if (!hitEnemy && other.CompareTag(Strings.TAG_PLAYER)) {
            other.GetComponent<Player>().GetDamage(damage);
        }

        GameObject deathGO = Instantiate(deathParticles.gameObject, transform.position, Quaternion.identity);

        Destroy(deathGO, 0.5f);
        Destroy(gameObject);
    }
}