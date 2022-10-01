using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour {
    static public GameManager instance;
    static public GameState state = GameState.StartMenu;

    private void Start() {
        if (instance == null || this == instance) {
            DontDestroyOnLoad(gameObject);
            instance = this;
            Time.timeScale = 0f;
            InputManager.instance.playerInput.SwitchCurrentActionMap(Strings.ACTION_MAP_UI);
        } else {
            Destroy(this);
            return;
        }
    }

    static public void StartGame() {
        Time.timeScale = 1f;
        InputManager.instance.playerInput.SwitchCurrentActionMap(Strings.ACTION_MAP_PLAYER);
        UIManager.instance.startMenuCanvas.gameObject.SetActive(false);
        UIManager.instance.ingameUICanvas.gameObject.SetActive(true);
        state = GameState.Playing;
    }

    static public void PauseGame() {
        state = GameState.Paused;
    }

    static public void QuitGame() {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif

        Application.Quit();
    }
}
