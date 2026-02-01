using UnityEngine;

public class ManagerInitializer : MonoBehaviour
{
    public GameObject gameManagerPrefab;
    public GameObject knowledgeManagerPrefab;
    public GameObject soundManagerPrefab;

    void Awake()
    {
        if (GameManager.Instance == null)
        {
            if (gameManagerPrefab != null)
            {
                Instantiate(gameManagerPrefab);
            }
            else
            {
                Debug.LogError("GameManager prefab is not assigned in the ManagerInitializer.");
            }
        }

        if (KnowledgeManager.Instance == null)
        {
            if (knowledgeManagerPrefab != null)
            {
                Instantiate(knowledgeManagerPrefab);
            }
            else
            {
                Debug.LogError("KnowledgeManager prefab is not assigned in the ManagerInitializer.");
            }
        }

        if (SoundManager.Instance == null)
        {
            if (soundManagerPrefab != null)
            {
                Instantiate(soundManagerPrefab);
            }
            else
            {
                Debug.LogError("SoundManager prefab is not assigned in the ManagerInitializer.");
            }
        }
    }
}
