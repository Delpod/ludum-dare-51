using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] Character[] characterList;
    [SerializeField] int health = 100;

    private Vector2 movingVector;
    private Character currentCharacter;

    private void Start() {
        for (int i = 0; i < characterList.Length; ++i) {
            characterList[i].gameObject.SetActive(i == 0);
        }

        currentCharacter = characterList[0];
    }

    private void OnDestroy() {
    }

    private void Update() {
        if (InputManager.instance.playerInput.actions[Strings.PLAYER_FIRE].triggered) {
            currentCharacter.Attack();
        }
    }

    private void FixedUpdate() {
        movingVector = InputManager.instance.playerInput.actions[Strings.PLAYER_MOVE].ReadValue<Vector2>();
        transform.position += currentCharacter.currentSpeed * Time.fixedDeltaTime * new Vector3(movingVector.x, movingVector.y);
    }

    public void GetDamage(int damage) {
        print("hit");

        health -= damage;

        if (health < 0f) {

        }
    }
}
