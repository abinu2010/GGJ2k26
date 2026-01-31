using UnityEngine;

public class KnowledgeManager : MonoBehaviour
{
    public static KnowledgeManager Instance;

    public GameObject knowledgePanel;
    private MaskPieceDrop maskPieceDropper;
    private GameObject enemyToDestroy;

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
        maskPieceDropper = dropper;
        enemyToDestroy = enemy;
        knowledgePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void HideKnowledgeCheck()
    {
        knowledgePanel.SetActive(false);
        Time.timeScale = 1;

        if (maskPieceDropper != null)
        {
            maskPieceDropper.Drop();
            maskPieceDropper = null;
        }

        if (enemyToDestroy != null)
        {
            Destroy(enemyToDestroy);
        }

        GameManager.Instance.PerformKnowledgeCheck();
    }
}
