using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    [SerializeField] public Character[] characterList;
    [SerializeField] int maxHealth = 100;
    [SerializeField] Slider healthBarSlider;

    [HideInInspector] public int currentCharacterId = 0;

    private int health;
    private Vector2 movingVector;
    private Character currentCharacter;

    private void Start() {
        SetCharacterActive();
        health = maxHealth;
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
    }

    private void FixedUpdate() {
        movingVector = InputManager.instance.playerInput.actions[Strings.PLAYER_MOVE].ReadValue<Vector2>();
        transform.position += currentCharacter.currentSpeed * Time.fixedDeltaTime * new Vector3(movingVector.x, movingVector.y);
    }

    public void GetDamage(int damage) {
        health -= damage;
        UpdateSlider();

        if (health < 0f) {
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
    }
}
