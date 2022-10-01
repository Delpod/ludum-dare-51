using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour {
    [SerializeField] Sprite sprite;
    [SerializeField] Weapon weapon;
    [SerializeField] float movementSpeed = 8f;
    [SerializeField] float movementAttackSpeed = 8f;
    [SerializeField] float attackDelay = 0.5f;

    public float currentSpeed = 10f;
    public bool isAttacking = false;

    private Vector2 mousePos;
    private float delay = -1f;
    private Camera mainCamera;

    private void Start() {
        mainCamera = Camera.main;
    }

    public void Attack() {
        if (delay <= 0f) {
            weapon.Attack();
            delay = attackDelay;
        }
    }

    public void Update() {
        if (delay > 0f) {
            delay -= Time.deltaTime;
            currentSpeed = movementAttackSpeed;
        } else {
            currentSpeed = movementSpeed;
        }

        if (InputManager.instance.playerInput.currentControlScheme != Strings.SCHEME_KEYBOARD_MOUSE) {
            Vector2 tmpPos = InputManager.instance.playerInput.actions[Strings.PLAYER_LOOK].ReadValue<Vector2>();
            if (!Mathf.Approximately(tmpPos.x, 0f) || !Mathf.Approximately(tmpPos.y, 0f)) {
                mousePos = tmpPos;
            }

            weapon.transform.rotation = Quaternion.Euler(0f, 0f, -Mathf.Sign(mousePos.x) * Vector3.Angle(transform.up, transform.position + new Vector3(mousePos.x, mousePos.y)));
        } else {
            Vector2 mousePos = mainCamera.ScreenToViewportPoint(Mouse.current.position.ReadValue());
            Vector3 dir = new Vector3(mousePos.x, mousePos.y) - mainCamera.WorldToViewportPoint(transform.position);
            dir.z = 0f;

            weapon.transform.rotation = Quaternion.Euler(0f, 0f, -Mathf.Sign(dir.x) * Vector3.Angle(transform.up, dir));
        }
    }

}