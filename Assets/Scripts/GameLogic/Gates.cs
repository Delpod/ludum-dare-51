using UnityEngine;

public class Gates : MonoBehaviour {
    [SerializeField] Transform topGate;
    [SerializeField] Transform bottomGate;
    [SerializeField] float openSpeed = 2f;


    public bool openable = true;
    public bool opened = false;

    private float targetY;

    private void Start() {
        if (opened) {
            Open();
        } else {
            Close();
        }

        topGate.localPosition = new Vector3(topGate.localPosition.x, targetY, 0f);
        bottomGate.localPosition = new Vector3(bottomGate.localPosition.x, -targetY, 0f);
    }

    public void Open() {
        if (!openable) {
            return;
        }

        targetY = 1.75f;
    }

    public void Close() {
        targetY = 1f;
    }

    private void Update() {
        if (!Mathf.Approximately(topGate.localPosition.y, targetY)) {
            topGate.localPosition = new Vector3(topGate.localPosition.x, Mathf.MoveTowards(topGate.localPosition.y, targetY, Time.deltaTime * openSpeed), 0f);
            bottomGate.localPosition = new Vector3(bottomGate.localPosition.x, Mathf.MoveTowards(bottomGate.localPosition.y, -targetY, Time.deltaTime * openSpeed), 0f);
        }
    }
}
