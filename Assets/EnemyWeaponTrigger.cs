using UnityEngine;

public class EnemyWeaponTrigger : MonoBehaviour {
    private int damage;

    private void Start() {
        damage = GetComponentInParent<Weapon>().damage;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag(Strings.TAG_ENEMY)) {
            other.GetComponent<Enemy>().GetDamage(damage);
        }
    }
}
