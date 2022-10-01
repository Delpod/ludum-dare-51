using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour {
    static public GameManager instance;
    static public GameState state = GameState.StartMenu;
    static public int monsterCount;
    static public int roomCount;
    static public int levelCount;
    static public int itemCount;

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
        UIManager.instance.endMenuCanvas.gameObject.SetActive(false);
        UIManager.instance.ingameUICanvas.gameObject.SetActive(true);
        monsterCount = 0;
        roomCount = 0;
        levelCount = 0;
        itemCount = 0;
        state = GameState.Playing;
    }

    static public void LoseGame() {
        EndGame();
        UIManager.instance.endMenuCanvas.gameObject.SetActive(true);
        state = GameState.Lose;
    }

    static public void WinGame() {
        EndGame();
        state = GameState.Won;
    }

    static private void EndGame() {
        Time.timeScale = 0f;
        InputManager.instance.playerInput.SwitchCurrentActionMap(Strings.ACTION_MAP_UI);
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

    static public void MonsterKilled() {
        ++monsterCount;
    }

    static public void RoomCleared() {
        ++roomCount;
    }

    static public void LevelCleared() {
        ++levelCount;
    }

    static public void ItemPickedUp() {
        ++itemCount;
    }
}
