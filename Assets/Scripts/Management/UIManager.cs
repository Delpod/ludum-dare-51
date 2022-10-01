using UnityEngine;

public class UIManager : MonoBehaviour {
    static public UIManager instance { get; private set; }

    public Canvas startMenuCanvas;
    public Canvas endMenuCanvas;
    public Canvas ingameUICanvas;

    private void Awake() {
        instance = this;
    }
}
