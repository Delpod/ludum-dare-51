using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    [SerializeField] public Character[] characterList;
    [SerializeField] int maxHealth = 100;
    [SerializeField] Slider healthBarSlider;
    [SerializeField] AudioSource audioSourceHeroTheme;
    [SerializeField] AudioClip switchClip;
    [SerializeField] AudioClip[] hurtClips;

    public AudioSource audioSourceAlive;

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
        transform.position += currentCharacter.currentSpeed * Time.fixedDeltaTime * new Vector3(movingVector.x, movingVector.y);
    }

    public void GetDamage(int damage) {
        health -= damage;
        UpdateSlider();

        AudioSource.PlayClipAtPoint(Helpers.RandomElement(hurtClips), transform.position, 1f);

        if (health < 0f) {
            audioSourceAlive.Stop();
            GameManager.LoseGame();
        }
    }

    private void UpdateSlider() {
        healthBarSlider.value = (float)health / maxHealth;
    }

    public void NextCharacter() {
        if (currentCharacterId + 1 == characterList.Length) {
            currentCharacterId = 0;
        } else {
            ++currentCharacterId;
        }

        currentCharacter.ZeroAnimations();

        AudioSource.PlayClipAtPoint(switchClip, transform.position, 0.75f);

        SetCharacterActive();
    }

    public static void RestartPlayer(bool restoreHealth) {
        Player p = FindObjectOfType<Player>();
        p.maxHealth = 100;

        if (restoreHealth) {
            p.health = p.maxHealth;
        }

        p.UpdateSlider();
        p.currentCharacterId = 0;
        p.SetCharacterActive();
        p.transform.position = Vector3.zero;
        p.audioSourceAlive.Play();
    }
}
