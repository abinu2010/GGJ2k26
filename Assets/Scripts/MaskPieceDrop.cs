using UnityEngine;

public class MaskPieceDrop : MonoBehaviour
{
    public PickupMask pickupPrefab;
    public int pieceId = 1;
    public Vector3 offset = new Vector3(0f, 0.6f, 0f);

    public void Drop()
    {
        if (pickupPrefab == null) return;

        float posX = transform.position.x + offset.x;
        float posY = transform.position.y + offset.y;
        float posZ = transform.position.z + offset.z;

        PickupMask pickup = Instantiate(pickupPrefab, new Vector3(posX, posY, posZ), Quaternion.identity);
        pickup.pieceId = pieceId;
    }
}
