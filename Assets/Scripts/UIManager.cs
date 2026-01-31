using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerController playerController;

    void Start()
    {
        if (playerController == null)
        {
            playerController = FindObjectOfType<PlayerController>();
        }
    }

    public void OnIntelligenceButtonClicked()
    {
        if (playerController != null && playerController.weaponManager.currentWeapon.GetComponentInChildren<Wand>() != null)
        {
            GameManager.Instance.IncreaseWandDamage();
        }
        KnowledgeManager.Instance.HideKnowledgeCheck();
    }

    public void OnStrengthButtonClicked()
    {
        if (playerController != null && playerController.weaponManager.currentWeapon.GetComponentInChildren<Sword>() != null)
        {
            GameManager.Instance.IncreaseSwordDamage();
        }
        KnowledgeManager.Instance.HideKnowledgeCheck();
    }

    public void OnAgilityButtonClicked()
    {
        if (playerController != null && playerController.weaponManager.currentWeapon.GetComponentInChildren<Bow>() != null)
        {
            GameManager.Instance.IncreaseBowDamage();
        }
        KnowledgeManager.Instance.HideKnowledgeCheck();
    }
}
