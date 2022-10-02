using UnityEngine;

public class Timer : MonoBehaviour {
    [SerializeField] Transform arrow;

    float time = 0f;

    private float multiplier = 10f;
    private float maxTime;
    private Player player;
    private Animator animator;

    void Start() {
        animator = GetComponent<Animator>();
        player = FindObjectOfType<Player>();
        maxTime = 10f * player.characterList.Length;
    }

    void LateUpdate() {
        int currentId = (int)(time / multiplier);
        time += Time.deltaTime;

        if (time > maxTime) {
            time -= maxTime;
        }

        int newId = (int)(time / multiplier);

        if (currentId != newId) {
            player.NextCharacter();
        }

        arrow.rotation = Quaternion.Euler(0f, 0f, -360f * time / maxTime);
    }

    public static void RestartTimer() {
        Timer timer = FindObjectOfType<Timer>();
        timer.time = 0f;
        if (timer.animator) {
            timer.animator.SetTrigger(Strings.TRIGGER_RESTART_TIMER);
        }
    }
}
