using UnityEngine;

public class Bullet : MonoBehaviour {
    [SerializeField] float rotationSpeed = 10f;

    public int damage = 25;

    private void Update() {
        transform.Rotate(new Vector3(0f, 0f, rotationSpeed * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag(Strings.TAG_ENEMY)) {
            other.GetComponent<Enemy>().GetDamage(damage);
        }

        Destroy(gameObject);
    }
}