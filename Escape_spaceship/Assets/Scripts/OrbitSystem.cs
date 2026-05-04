// using UnityEngine;

// public class OrbitSystem : MonoBehaviour
// {
//     public Transform[] spheres;   // assign 3 spheres here
//     public float radius = 2f;
//     public float speed = 1f;      // radians per second

//     void Update()
//     {
//         float time = Time.time * speed;

//         for (int i = 0; i < spheres.Length; i++)
//         {
//             float angle = time + i * Mathf.PI * 2f / spheres.Length;

//             float x = Mathf.Cos(angle) * radius;
//             float z = Mathf.Sin(angle) * radius;

//             spheres[i].position = transform.position + new Vector3(x, 0, z);
//         }
//     }
// }
using UnityEngine;

public class OrbitSystem : MonoBehaviour
{
    public Transform[] spheres;
    public float radius = 2f;

    public float normalSpeed = 1f;
    public float slowDownTime = 2f;

    public bool triggered = false;

    private float currentSpeed;

    void Update()
    {
        if (triggered)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0f, Time.deltaTime * slowDownTime);
        }
        else
        {
            currentSpeed = normalSpeed;
        }

        float time = Time.time * currentSpeed;

        for (int i = 0; i < spheres.Length; i++)
        {
            float angle = time + i * Mathf.PI * 2f / spheres.Length;

            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;

            spheres[i].position = transform.position + new Vector3(x, 0, z);
        }
    }
}
