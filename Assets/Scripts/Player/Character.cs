using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour {
    [SerializeField] Sprite sprite;
    [SerializeField] Weapon weapon;
    [SerializeField] float movementSpeed = 6f;
    [SerializeField] float movementAttackSpeed = 6f;
    [SerializeField] float changeSpeed = 20f;
    [SerializeField] float attackDelay = 0.5f;

    public AudioClip audioClip;
    public float currentSpeed = 10f;
    public bool isAttacking = false;

    private Animator animator;
    private Vector2 mousePos;
    private float delay = -1f;
    private Camera mainCamera;

    private void Start() {
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
    }

    public void Attack() {
        if (delay <= 0f) {
            weapon.Attack();
            delay = attackDelay;
        }
    }

    public void Update() {
        if (delay > 0f) {
            if (!isAttacking) {
                delay -= Time.deltaTime;
            }
            currentSpeed = Mathf.MoveTowards(currentSpeed, movementAttackSpeed, Time.deltaTime * changeSpeed);
        } else {
            currentSpeed = Mathf.MoveTowards(currentSpeed, movementSpeed, Time.deltaTime * changeSpeed);
        }

        if (isAttacking) {
            return;
        }

        if (InputManager.instance.playerInput.currentControlScheme != Strings.SCHEME_KEYBOARD_MOUSE) {
            Vector2 tmpPos = InputManager.instance.playerInput.actions[Strings.PLAYER_LOOK].ReadValue<Vector2>();
            if (!Mathf.Approximately(tmpPos.x, 0f) || !Mathf.Approximately(tmpPos.y, 0f)) {
                mousePos = tmpPos;
            }

            weapon.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.MoveTowardsAngle(weapon.transform.rotation.eulerAngles.z, -Mathf.Sign(mousePos.x) * Vector3.Angle(transform.up, new Vector3(mousePos.x, mousePos.y)), 1080f * Time.deltaTime));
        } else {
            Vector2 mousePos = mainCamera.ScreenToViewportPoint(Mouse.current.position.ReadValue());
            Vector3 dir = new Vector3(mousePos.x, mousePos.y) - mainCamera.WorldToViewportPoint(transform.position);
            dir.z = 0f;

            weapon.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.MoveTowardsAngle(weapon.transform.rotation.eulerAngles.z, -Mathf.Sign(dir.x) * Vector3.Angle(transform.up, dir), 1080f * Time.deltaTime));
        }
    }

    public void StopRotation() {
        isAttacking = true;
    }

    public void StartRotation() {
        isAttacking = false;
    }

    public void ZeroAnimations() {
        isAttacking = false;
        currentSpeed = movementSpeed;
        weapon.ZeroAnimations();
        SetWalking(false);
    }

    public void SetWalking(bool value) {
        animator.SetBool(Strings.TRIGGER_WALKING, value);
    }
}