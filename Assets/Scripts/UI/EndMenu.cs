using UnityEngine;
using TMPro;

public class EndMenu : MonoBehaviour {
    [SerializeField] TMP_Text monstersKilled;
    [SerializeField] TMP_Text roomsCleared;
    [SerializeField] TMP_Text levelsBeaten;
    [SerializeField] TMP_Text itemsPickedUp;
    [SerializeField] TMP_Text timeText;

    private CanvasGroup canvasGroup;
    private float showSpeed = 2f;

    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable() {
        monstersKilled.text = "Monsters killed: " + GameManager.monsterCount;
        roomsCleared.text = "Rooms cleared: " + GameManager.roomCount;
        levelsBeaten.text = "Levels beaten: " + GameManager.levelCount;
        itemsPickedUp.text = "Items picked up: " + GameManager.itemCount;

        if (timeText) {
            float time = Time.time - GameManager.time;
            string text = ((int)(time / 60)).ToString("00") + ':' + (time % 60f).ToString("00.00").Replace(',', '.');
            timeText.text = "Time: " + text;
        }
    }

    private void Update() {
        if (!Mathf.Approximately(canvasGroup.alpha, 1f)) {
            canvasGroup.alpha += Time.unscaledDeltaTime * showSpeed;
        }
    }
}
