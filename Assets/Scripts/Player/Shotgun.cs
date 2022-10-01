using UnityEngine;

public class Shotgun : Weapon {
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] float bulletSpeed = 100f;
    private Animator animator;

    private void Start() {
        animator = GetComponent<Animator>();
    }

    public override void Attack() {
        animator.SetTrigger(Strings.TRIGGER_ATTACK);
    }

    public void Shoot() {
        GameObject go = Instantiate(bulletPrefab.gameObject, transform.position, transform.rotation);

        go.GetComponent<Bullet>().damage = damage;
        go.GetComponent<Rigidbody2D>().AddForce(go.transform.up * bulletSpeed)
        ;
    }
}
