using UnityEngine;

public class Mover : MonoBehaviour {
    [SerializeField] float yPositionChange = 0.05f;
    [SerializeField] float speedChange = 1f;

    private Vector3 startPosition;

    private void Start() {
        startPosition = transform.position;
    }

    void Update() {
        transform.position = new(startPosition.x, startPosition.y + Mathf.Sin(speedChange * Time.unscaledTime) * yPositionChange, startPosition.z);
    }
}
