using UnityEngine;
public abstract class Weapon : MonoBehaviour {
    [SerializeField] public int damage = 25;
    public abstract void Attack();
    public abstract void ZeroAnimations();
}