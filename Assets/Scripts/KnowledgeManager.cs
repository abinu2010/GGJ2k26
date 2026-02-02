using UnityEngine;

public class KnowledgeManager : MonoBehaviour
{
    public static KnowledgeManager Instance;

    public GameObject knowledgePanel;

    private MaskPieceDrop pendingDropper;
    private GameObject pendingEnemy;

    private CursorLockMode cachedLockMode;
    private bool cachedCursorVisible;

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
    public void HideKnowledgeCheck()
    {
        CancelKnowledgeCheck();
    }


    public void ShowKnowledgeCheck(MaskPieceDrop dropper, GameObject enemy)
    {
        if (knowledgePanel == null)
        {
            Debug.LogError("KnowledgePanel is not assigned in the KnowledgeManager.");
            return;
        }

        pendingDropper = dropper;
        pendingEnemy = enemy;

        cachedLockMode = Cursor.lockState;
        cachedCursorVisible = Cursor.visible;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        knowledgePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    private void CompleteAndDrop()
    {
        if (knowledgePanel != null)
        {
            knowledgePanel.SetActive(false);
        }

        Time.timeScale = 1f;

        Cursor.lockState = cachedLockMode;
        Cursor.visible = cachedCursorVisible;

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySoundEffect(SoundManager.Instance.uiButton);
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.MarkKnowledgeCheckCompleted();
        }

        if (pendingDropper != null)
        {
            pendingDropper.Drop();
        }

        if (pendingEnemy != null)
        {
            Destroy(pendingEnemy);
        }

        pendingDropper = null;
        pendingEnemy = null;
    }

    public void ChooseWandDamage()
    {
        if (GameManager.Instance != null) GameManager.Instance.IncreaseWandDamage();
        CompleteAndDrop();
    }

    public void ChooseSwordDamage()
    {
        if (GameManager.Instance != null) GameManager.Instance.IncreaseSwordDamage();
        CompleteAndDrop();
    }

    public void ChooseBowDamage()
    {
        if (GameManager.Instance != null) GameManager.Instance.IncreaseBowDamage();
        CompleteAndDrop();
    }

    public void CancelKnowledgeCheck()
    {
        CompleteAndDrop();
    }
}
