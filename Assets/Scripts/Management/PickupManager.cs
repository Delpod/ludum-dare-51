using UnityEditor;
using UnityEngine;
public class PickupManager : MonoBehaviour {
    static public PickupManager instance { get; private set; }
    [HideInInspector] public float armorPercent = 1f;
    [HideInInspector] public float swordDamagePercent = 1f;
    [HideInInspector] public float gunDamagePercent = 1f;

    private float armorTime = 0f;
    private float swordDamageTime = 0f;
    private float gunDamageTime = 0f;

    private void Awake() {
        instance = this;
    }

    private void Update() {
        if (armorTime > 0f) {
            armorTime -= Time.deltaTime;
        }

        if (swordDamageTime > 0f) {
            swordDamageTime -= Time.deltaTime;
        }

        if (gunDamageTime > 0f) {
            gunDamageTime -= Time.deltaTime;
        }

        if (armorTime <= 0f && !Mathf.Approximately(armorPercent, 1f)) {
            armorPercent = 0f;
        }

        if (swordDamageTime <= 0f && !Mathf.Approximately(swordDamagePercent, 1f)) {
            armorPercent = 0f;
        }

        if (gunDamageTime <= 0f && !Mathf.Approximately(gunDamagePercent, 1f)) {
            armorPercent = 0f;
        }
    }

    public void AddHealth(float amount) {
        FindObjectOfType<Player>().GetDamage((int)-amount);
    }

    public void AddArmor(float amount, float time) {
        armorPercent = amount;
        armorTime = time;
    }

    public void AddSwordDamage(float amount, float time) {
        swordDamagePercent = amount;
        swordDamageTime = time;
    }

    public void AddGunDamage(float amount, float time) {
        gunDamagePercent = amount;
        gunDamagePercent = time;
    }
}