using UnityEngine;
public class Sword : Weapon {

    private Animator animator;
    
    private void Start() {
        animator = GetComponent<Animator>();
    }

    public override void Attack() {
        transform.parent.GetComponent<Character>().StopRotation();
        animator.SetTrigger(Strings.TRIGGER_ATTACK);
    }

    public void AttackFinished() {
        transform.parent.GetComponent<Character>().StartRotation();
    }
}