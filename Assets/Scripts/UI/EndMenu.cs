using UnityEngine;
using TMPro;

public class EndMenu : MonoBehaviour {
    [SerializeField] TMP_Text monstersKilled;
    [SerializeField] TMP_Text roomsCleared;
    [SerializeField] TMP_Text levelsBeaten;
    [SerializeField] TMP_Text itemsPickedUp;

    private CanvasGroup canvasGroup;
    private float showSpeed = 2f;

    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();

        monstersKilled.text = "Monsters killed: " + GameManager.monsterCount;
        roomsCleared.text = "Rooms cleared: " + GameManager.monsterCount;
        levelsBeaten.text = "Levels beaten: " + GameManager.monsterCount;
        itemsPickedUp.text = "Items picked up: " + GameManager.monsterCount;
    }

    private void Update() {
        if (!Mathf.Approximately(canvasGroup.alpha, 1f)) {
            canvasGroup.alpha += Time.unscaledDeltaTime * showSpeed;
        }
    }
}
