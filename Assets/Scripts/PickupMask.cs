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
            MaskCheck.Instance.Collect(pieceId);
        }

        Destroy(gameObject);
    }
}
