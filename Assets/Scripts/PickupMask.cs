using System.Collections;
using UnityEngine;

public class PickupMask : MonoBehaviour
{
    public int pieceId = 1;
    private Collider col;

    void Start()
    {
        col = GetComponent<Collider>();
        StartCoroutine(EnableColliderAfterDelay(1.0f));
    }

    IEnumerator EnableColliderAfterDelay(float delay)
    {
        col.enabled = false;
        yield return new WaitForSeconds(delay);
        col.enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Health playerHealth = other.GetComponentInParent<Health>();
        if (playerHealth == null) return;

        if (MaskCheck.Instance != null)
        {
            MaskCheck.Instance.Collect(pieceId);
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySoundEffect(SoundManager.Instance.maskFound);
            }
        }

        Destroy(gameObject);
    }
}