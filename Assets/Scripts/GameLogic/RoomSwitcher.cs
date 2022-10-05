using UnityEngine;

public class RoomSwitcher : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag(Strings.TAG_PLAYER)) {
            if (FindObjectOfType<RoomManager>().SwitchActiveRoom(transform.parent.GetComponent<Room>())) {
                other.transform.position = transform.position;
            }
        }
    }
}
