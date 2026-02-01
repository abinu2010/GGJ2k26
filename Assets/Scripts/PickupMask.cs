using UnityEngine;

public class PickupMask : MonoBehaviour
{
    public int pieceId = 1;

    void OnTriggerEnter(Collider other)
    {
        Health playerHealth = other.GetComponentInParent<Health>();
        if (playerHealth == null) return;

        if (MaskCheck.Instance != null)
        {
            if (KnowledgeManager.Instance != null && !GameManager.Instance.knowledgeCheckPerformed)
            {
                KnowledgeManager.Instance.ShowKnowledgeCheck(null, null);
            }
            MaskCheck.Instance.Collect(pieceId);
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySoundEffect(SoundManager.Instance.maskFound);
            }
        }

        Destroy(gameObject);
    }
}
