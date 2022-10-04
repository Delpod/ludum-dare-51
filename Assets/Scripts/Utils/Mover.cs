using UnityEngine;

public class Mover : MonoBehaviour {
    [SerializeField] float yPositionChange = 0.05f;
    [SerializeField] float speedChange = 1f;

    private Vector2 startPosition;
    private RectTransform rectTransform;

    private void Start() {
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.anchoredPosition;
    }

    void Update() {
        rectTransform.anchoredPosition = new(startPosition.x, startPosition.y + Mathf.Sin(speedChange * Time.unscaledTime) * yPositionChange);
    }
}
