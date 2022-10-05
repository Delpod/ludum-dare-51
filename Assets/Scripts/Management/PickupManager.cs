using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PickupManager : MonoBehaviour {
    static public PickupManager instance { get; private set; }

    [SerializeField] Image armorUpgradeImage;
    [SerializeField] Image swordUpgradeImage;
    [SerializeField] Image gunUpgradeImage;
    [SerializeField] TMP_Text messageText;
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
    private WaitForSeconds messageWait = new(5f);

    private readonly string messageHealth = "25 health restored";
    private readonly string messageArmor = "-50% damage taken for next 30s";
    private readonly string messageSword = "+50% sword damage for next 30s";
    private readonly string messageGun = "+50% gun damage for next 30s";

    private void Awake() {
        instance = this;
        armorUpgradeText = armorUpgradeImage.GetComponentInChildren<TMP_Text>();
        swordUpgradeText = swordUpgradeImage.GetComponentInChildren<TMP_Text>();
        gunUpgradeText = gunUpgradeImage.GetComponentInChildren<TMP_Text>();
    }

    public void Reset() {
        messageText.gameObject.SetActive(false);
        armorPercent = 1f;
        swordDamagePercent = 1f;
        gunDamagePercent = 1f;
        armorTime = 0f;
        swordDamageTime = 0f;
        gunDamageTime = 0f;
        StopAllCoroutines();
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
        messageText.text = messageHealth;
        messageText.gameObject.SetActive(true);
        StartCoroutine(HideMessage());
        GameManager.ItemPickedUp();
        GameObject go = Instantiate(healthParticles.gameObject, player.transform);
        Destroy(go, particlesLength);
    }

    public void AddArmor(float amount, float time) {
        armorPercent = amount;
        armorTime = time;
        armorUpgradeImage.gameObject.SetActive(true);
        armorUpgradeText.text = time.ToString("0");
        messageText.text = messageArmor;
        messageText.gameObject.SetActive(true);
        StartCoroutine(HideMessage());
        GameManager.ItemPickedUp();
        GameObject go = Instantiate(armorParticles.gameObject, FindObjectOfType<Player>().transform);
        Destroy(go, particlesLength);
    }

    public void AddSwordDamage(float amount, float time) {
        swordDamagePercent = amount;
        swordDamageTime = time;
        swordUpgradeImage.gameObject.SetActive(true);
        swordUpgradeText.text = time.ToString("0");
        messageText.text = messageSword;
        messageText.gameObject.SetActive(true);
        StartCoroutine(HideMessage());
        GameManager.ItemPickedUp();
        GameObject go = Instantiate(swordParticles.gameObject, FindObjectOfType<Player>().transform);
        Destroy(go, particlesLength);
    }

    public void AddGunDamage(float amount, float time) {
        gunDamagePercent = amount;
        gunDamageTime = time;
        gunUpgradeImage.gameObject.SetActive(true);
        gunUpgradeText.text = time.ToString("0");
        messageText.text = messageGun;
        messageText.gameObject.SetActive(true);
        StartCoroutine(HideMessage());
        GameManager.ItemPickedUp();
        GameObject go = Instantiate(gunParticles.gameObject, FindObjectOfType<Player>().transform);
        Destroy(go, particlesLength);
    }

    private IEnumerator HideMessage() {
        yield return messageWait;
        messageText.gameObject.SetActive(false);
    }
}