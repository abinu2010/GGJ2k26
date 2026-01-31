using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject sword;
    public GameObject bow;
    public GameObject wand;

    public GameObject currentWeapon;

    public void EquipWeapon(string weaponType)
    {
        if (currentWeapon != null)
        {
            currentWeapon.SetActive(false);
        }

        switch (weaponType)
        {
            case "Sword":
                currentWeapon = sword;
                break;
            case "Bow":
                currentWeapon = bow;
                break;
            case "Wand":
                currentWeapon = wand;
                break;
        }

        if (currentWeapon != null)
        {
            currentWeapon.SetActive(true);
        }
    }
}
