using UnityEngine;

public class Bullet : MonoBehaviour {
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] ParticleSystem deathParticles;

    public int damage = 25;

    private void Update() {
        transform.Rotate(new Vector3(0f, 0f, rotationSpeed * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag(Strings.TAG_ENEMY)) {
            other.GetComponent<Enemy>().GetDamage(damage);
        }

        GameObject deathGO = Instantiate(deathParticles.gameObject, transform.position, Quaternion.identity);

        Destroy(deathGO, 0.5f);
        Destroy(gameObject);
    }
}