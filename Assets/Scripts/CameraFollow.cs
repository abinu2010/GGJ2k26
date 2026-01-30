using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothing = 5f;
    Vector3 offSet;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        offSet = transform.position - target.position;
    }
    void FixedUpdate()
    {
        Vector3 targetCamPos = target.position+offSet;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
