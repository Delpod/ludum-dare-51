using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] Character[] characterList;

    private Vector2 movingVector;
    private Vector2 lookingVector;
    private float currentMaxSpeed = 5f;

    private void Start() {
    }

    private void OnDestroy() {
    }

    private void Update() {
        movingVector = InputManager.instance.playerInput.actions[Strings.PLAYER_MOVE].ReadValue<Vector2>();
        lookingVector = InputManager.instance.playerInput.actions[Strings.PLAYER_LOOK].ReadValue<Vector2>();

        transform.position += currentMaxSpeed * Time.deltaTime * new Vector3(movingVector.x, movingVector.y);

        if (InputManager.instance.playerInput.actions[Strings.PLAYER_FIRE].triggered) {
            // Shooting
        }
    }
}
