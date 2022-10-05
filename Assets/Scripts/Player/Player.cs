using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour {
    [SerializeField] public Character[] characterList;
    [SerializeField] public Character[] swordCharacterList;
    [SerializeField] public Character[] shotgunCharacterList;
    [SerializeField] int maxHealth = 100;
    [SerializeField] Slider healthBarSlider;
    [SerializeField] TMP_Text healthBarText;
    [SerializeField] AudioSource audioSourceHeroTheme;
    [SerializeField] AudioClip switchClip;
    [SerializeField] AudioClip[] hurtClips;
    [SerializeField] ParticleSystem switchParticles;

    public AudioSource audioSourceAlive;

    [HideInInspector] public float damageModifier = 1f;
    [HideInInspector] public int currentCharacterId = 0;

    private int health;
    private Vector2 movingVector;
    private Character currentCharacter;

    private void Start() {
        SetCharacterActive();
        health = maxHealth;
        audioSourceHeroTheme.Stop();
        audioSourceAlive.Stop();
    }

    private void Update() {
        if (InputManager.instance.playerInput.actions[Strings.PLAYER_FIRE].triggered) {
            currentCharacter.Attack();
        }
    }

    private void SetCharacterActive() {
        for (int i = 0; i < characterList.Length; ++i) {
            characterList[i].gameObject.SetActive(i == currentCharacterId);
        }

        currentCharacter = characterList[currentCharacterId];
        if (audioSourceHeroTheme.isPlaying) {
            audioSourceHeroTheme.Stop();
        }
        audioSourceHeroTheme.clip = currentCharacter.audioClip;
        audioSourceHeroTheme.Play();
    }

    private void FixedUpdate() {
        movingVector = InputManager.instance.playerInput.actions[Strings.PLAYER_MOVE].ReadValue<Vector2>();
        if (!Mathf.Approximately(movingVector.x, 0f) || !Mathf.Approximately(movingVector.y, 0f)) {
            transform.position += currentCharacter.currentSpeed * Time.fixedDeltaTime * new Vector3(movingVector.x, movingVector.y);
            currentCharacter.SetWalking(true);
        } else {
            currentCharacter.SetWalking(false);
        }
    }

    public void GetDamage(int damage, bool useModifier = true) {
        health = Mathf.Min(health - (useModifier ? (int)(damage * (damageModifier * PickupManager.instance.armorPercent)) : damage), maxHealth);
        UpdateSlider();

        if (damage > 0) {
            AudioSource.PlayClipAtPoint(Helpers.RandomElement(hurtClips), transform.position, 1f);
            CameraShake.instance.ShakeCamera(2f, 0.5f);
        }

        if (health <= 0) {
            audioSourceAlive.Stop();
            GameManager.LoseGame();
        }
    }

    private void UpdateSlider() {
        healthBarText.text = "" + health + " / " + maxHealth;
        healthBarSlider.value = (float)health / maxHealth;
        healthBarSlider.GetComponent<Animator>().SetTrigger(Strings.TRIGGER_SHAKE);
    }

    public void NextCharacter() {
        if (currentCharacterId + 1 == characterList.Length) {
            currentCharacterId = 0;
        } else {
            ++currentCharacterId;
        }

        currentCharacter.ZeroAnimations();

        AudioSource.PlayClipAtPoint(switchClip, transform.position, 0.75f);
        switchParticles.Play();

        SetCharacterActive();
    }

    public static void RestartPlayer(bool startAgain) {
        Player p = FindObjectOfType<Player>();
        p.currentCharacter.SetWalking(false);


        if (startAgain) {
            p.maxHealth = 100;
            p.health = p.maxHealth;
            p.UpdateSlider();
            p.currentCharacterId = 0;
            p.SetCharacterActive();
            p.audioSourceAlive.Play();
            foreach (Character c in p.characterList) {
                c.SetWeaponDamage(c.defaultDamage);
            }
        }

        if (p.switchParticles.isPlaying) {
            p.switchParticles.Stop();
        }
        p.transform.position = Vector3.zero;
        p.healthBarSlider.GetComponent<Animator>().SetTrigger(Strings.TRIGGER_RESTART_ANIMATION);
    }
}
