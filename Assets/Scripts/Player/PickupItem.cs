using UnityEngine;

public class PickupItem : MonoBehaviour {
    public enum PickupType {
        HEALTH,
        ARMOR,
        SWORD_DAMAGE,
        GUN_DAMAGE
    };

    [SerializeField] float amount = 1.5f;
    [SerializeField] float time = 30f;
    [SerializeField] PickupType type;

    private ParticleSystem particles;

    private void Start() {
        particles = GetComponentInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag(Strings.TAG_PLAYER)) {
            if (type == PickupType.HEALTH) {
                PickupManager.instance.AddHealth(amount);
            } else if (type == PickupType.ARMOR) {
                PickupManager.instance.AddArmor(amount, time);
            } else if (type == PickupType.SWORD_DAMAGE) {
                PickupManager.instance.AddSwordDamage(amount, time);
            } else if (type == PickupType.GUN_DAMAGE) {
                PickupManager.instance.AddGunDamage(amount, time);
            }
            GetComponent<CircleCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            Destroy(gameObject, 1f);
        }
    }
}
