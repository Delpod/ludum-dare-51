using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIMenu : MonoBehaviour {
    public Selectable firstToSelect;

    private void OnDisable() {
        FindObjectOfType<EventSystem>().SetSelectedGameObject(null);
    }
}
