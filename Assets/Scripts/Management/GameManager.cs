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
        } else {
            Destroy(this);
            return;
        }
    }

    static public void StartGame() {
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
