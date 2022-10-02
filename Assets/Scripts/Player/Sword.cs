using UnityEngine;
public class Sword : Weapon {
    private Animator animator;
    
    private void Start() {
        animator = GetComponent<Animator>();
    }

    public override void Attack() {
        transform.parent.GetComponent<Character>().StopRotation();
        animator.SetTrigger(Strings.TRIGGER_ATTACK);
        CameraShake.instance.ShakeCamera(0.5f, 0.5f);
    }

    public void StartAttack() {
        AudioSource.PlayClipAtPoint(attackSound, transform.position, 0.75f);
    }

    public void AttackFinished() {
        transform.parent.GetComponent<Character>().StartRotation();
    }
    
    public override void ZeroAnimations() {
        animator.SetTrigger(Strings.TRIGGER_FORCE_IDLE);
    }
}