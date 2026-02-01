using UnityEngine;

public class KnowledgeManager : MonoBehaviour
{
    public static KnowledgeManager Instance;

    public GameObject knowledgePanel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowKnowledgeCheck(MaskPieceDrop dropper, GameObject enemy)
    {
        if (knowledgePanel == null)
        {
            Debug.LogError("KnowledgePanel is not assigned in the KnowledgeManager.");
            return;
        }

        knowledgePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void HideKnowledgeCheck()
    {
        Debug.Log("HideKnowledgeCheck() called.");
        knowledgePanel.SetActive(false);
        Time.timeScale = 1;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.PerformKnowledgeCheck();
        }
        else
        {
            Debug.LogError("GameManager.Instance is null. Cannot perform knowledge check.");
        }
    }
}
