using UnityEngine;

public class Vortex : MonoBehaviour {
    [SerializeField] AudioClip enterClip;

    private void Awake() {
        transform.localScale = new Vector3(0f, 0f, 0f);
    }

    private void Update() {
        if (!Mathf.Approximately(transform.localScale.x, 1f)) {
            float newVal = Mathf.MoveTowards(transform.localScale.x, 1f, Time.deltaTime);
            transform.localScale = new Vector3(newVal, newVal, newVal);
        }

        transform.Rotate(new Vector3(0f, 0f, Time.deltaTime * 45f));
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag(Strings.TAG_PLAYER)) {
            AudioSource.PlayClipAtPoint(enterClip, transform.position, 1f);

            GameManager.LevelCleared();

            if (GameManager.levelCount == GameManager.maxLevel) {
                GameManager.WinGame();
            }

            FindObjectOfType<RoomManager>().CreateRooms(false);
            Destroy(gameObject);
        }
    }
}
