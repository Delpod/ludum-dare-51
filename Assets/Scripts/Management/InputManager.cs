using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour {
    static public InputManager instance { get; private set; }

    [HideInInspector] public PlayerInput playerInput;

    private void Awake() {
        instance = this;
        playerInput = GetComponent<PlayerInput>();
    }
}
