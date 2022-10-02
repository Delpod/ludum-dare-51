using UnityEngine;

public class UpgradeMenu : MonoBehaviour {
    [SerializeField] AudioClip upgradeClip;

    private void OnEnable() {
        InputManager.instance.playerInput.SwitchCurrentActionMap(Strings.ACTION_MAP_UI);
    }

    private void OnDisable() {
        InputManager.instance.playerInput.SwitchCurrentActionMap(Strings.ACTION_MAP_PLAYER);

    }

    public void UpgradeSword() {
        foreach (Character c in FindObjectOfType<Player>().swordCharacterList) {
            c.SetWeaponDamage((int)(c.GetWeaponDamage() * 1.25f));
        }

        HideMenu();
    }

    public void UpgradeShotgun() {
        foreach (Character c in FindObjectOfType<Player>().shotgunCharacterList) {
            c.SetWeaponDamage((int)(c.GetWeaponDamage() * 1.25f));
        }

        HideMenu();
    }
    public void UpgradeHealth() {
        FindObjectOfType<Player>().GetDamage(-25);

        HideMenu();
    }

    public void UpgradeArmor() {
        FindObjectOfType<Player>().damageModifier *= 0.75f;

        HideMenu();
    }

    private void HideMenu() {
        AudioSource.PlayClipAtPoint(upgradeClip, Vector3.zero, 0.5f);
        gameObject.SetActive(false);
    }
}
