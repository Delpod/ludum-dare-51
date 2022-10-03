using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour {
    static public InputManager instance { get; private set; }

    [HideInInspector] public PlayerInput playerInput;

    private void Awake() {
        instance = this;
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnNavigate() {
        UIMenu uiMenu = FindObjectOfType<UIMenu>();
        EventSystem es = FindObjectOfType<EventSystem>();

        if (uiMenu && es && es.currentSelectedGameObject == null) {
            es.SetSelectedGameObject(uiMenu.firstToSelect.gameObject);
        }
    }
}
