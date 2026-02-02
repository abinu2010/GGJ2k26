using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float wandDamageBonus = 0f;
    public float swordDamageBonus = 0f;
    public float bowDamageBonus = 0f;

    public int knowledgeChecksCompleted = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }

        Destroy(gameObject);
    }

    public void IncreaseWandDamage()
    {
        wandDamageBonus += 25f;
    }

    public void IncreaseSwordDamage()
    {
        swordDamageBonus += 25f;
    }

    public void IncreaseBowDamage()
    {
        bowDamageBonus += 25f;
    }

    public void MarkKnowledgeCheckCompleted()
    {
        knowledgeChecksCompleted += 1;
    }
}
