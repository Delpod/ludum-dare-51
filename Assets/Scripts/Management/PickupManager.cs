using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PickupManager : MonoBehaviour {
    static public PickupManager instance { get; private set; }

    [SerializeField] Image armorUpgradeImage;
    [SerializeField] Image swordUpgradeImage;
    [SerializeField] Image gunUpgradeImage;
    [SerializeField] ParticleSystem healthParticles;
    [SerializeField] ParticleSystem armorParticles;
    [SerializeField] ParticleSystem swordParticles;
    [SerializeField] ParticleSystem gunParticles;
    [SerializeField] float particlesLength = 1.25f;

    [HideInInspector] public float armorPercent = 1f;
    [HideInInspector] public float swordDamagePercent = 1f;
    [HideInInspector] public float gunDamagePercent = 1f;

    private float armorTime = 0f;
    private float swordDamageTime = 0f;
    private float gunDamageTime = 0f;
    private TMP_Text armorUpgradeText;
    private TMP_Text swordUpgradeText;
    private TMP_Text gunUpgradeText;

    private void Awake() {
        instance = this;
        armorUpgradeText = armorUpgradeImage.GetComponentInChildren<TMP_Text>();
        swordUpgradeText = swordUpgradeImage.GetComponentInChildren<TMP_Text>();
        gunUpgradeText = gunUpgradeImage.GetComponentInChildren<TMP_Text>();
    }

    private void Update() {
        if (armorTime > 0f) {
            armorTime -= Time.deltaTime;
            armorUpgradeText.text = armorTime.ToString("0");
        }

        if (swordDamageTime > 0f) {
            swordDamageTime -= Time.deltaTime;
            swordUpgradeText.text = swordDamageTime.ToString("0");
        }

        if (gunDamageTime > 0f) {
            gunDamageTime -= Time.deltaTime;
            gunUpgradeText.text = gunDamageTime.ToString("0");
        }

        if (armorTime <= 0f && !Mathf.Approximately(armorPercent, 1f)) {
            armorPercent = 1f;
            armorUpgradeImage.gameObject.SetActive(false);
        }

        if (swordDamageTime <= 0f && !Mathf.Approximately(swordDamagePercent, 1f)) {
            swordDamagePercent = 1f;
            swordUpgradeImage.gameObject.SetActive(false);
        }

        if (gunDamageTime <= 0f && !Mathf.Approximately(gunDamagePercent, 1f)) {
            gunDamagePercent = 1f;
            gunUpgradeImage.gameObject.SetActive(false);
        }
    }

    public void AddHealth(float amount) {
        Player player = FindObjectOfType<Player>();
        player.GetDamage((int)-amount);
        GameObject go = Instantiate(healthParticles.gameObject, player.transform);
        Destroy(go, particlesLength);
    }

    public void AddArmor(float amount, float time) {
        armorPercent = amount;
        armorTime = time;
        armorUpgradeImage.gameObject.SetActive(true);
        armorUpgradeText.text = time.ToString("0");
        GameObject go = Instantiate(armorParticles.gameObject, FindObjectOfType<Player>().transform);
        Destroy(go, particlesLength);
    }

    public void AddSwordDamage(float amount, float time) {
        swordDamagePercent = amount;
        swordDamageTime = time;
        swordUpgradeImage.gameObject.SetActive(true);
        swordUpgradeText.text = time.ToString("0");
        GameObject go = Instantiate(swordParticles.gameObject, FindObjectOfType<Player>().transform);
        Destroy(go, particlesLength);
    }

    public void AddGunDamage(float amount, float time) {
        gunDamagePercent = amount;
        gunDamageTime = time;
        gunUpgradeImage.gameObject.SetActive(true);
        gunUpgradeText.text = time.ToString("0");
        GameObject go = Instantiate(gunParticles.gameObject, FindObjectOfType<Player>().transform);
        Destroy(go, particlesLength);
    }
}