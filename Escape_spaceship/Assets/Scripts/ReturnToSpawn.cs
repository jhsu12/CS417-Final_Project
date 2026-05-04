using UnityEngine;

public class ReturnToSpawn : MonoBehaviour
{
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Rigidbody rb;

    void Awake()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        rb = GetComponent<Rigidbody>();
    }

    public void ResetToSpawn()
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        transform.position = startPosition;
        transform.rotation = startRotation;

        if (rb != null)
        {
            rb.isKinematic = false;
        }
    }
}
