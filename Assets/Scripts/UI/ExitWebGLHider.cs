using UnityEngine;
using UnityEngine.UI;

public class ExitWebGLHider : MonoBehaviour {

    [SerializeField] Button playButton;

    private void Start() {
        if (Application.platform == RuntimePlatform.WebGLPlayer) {
            RectTransform rect = playButton.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y / 2f);

            gameObject.SetActive(false);
        }
    }
}
