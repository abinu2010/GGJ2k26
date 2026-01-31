using UnityEngine;

public class WeaponSelectionUI : MonoBehaviour
{
    public WeaponManager weaponManager;
    public GameObject weaponSelectionPanel;

    public void SelectSword()
    {
        weaponManager.EquipWeapon("Sword");
        weaponSelectionPanel.SetActive(false);
    }

    public void SelectBow()
    {
        weaponManager.EquipWeapon("Bow");
        weaponSelectionPanel.SetActive(false);
    }

    public void SelectWand()
    {
        weaponManager.EquipWeapon("Wand");
        weaponSelectionPanel.SetActive(false);
    }
}
