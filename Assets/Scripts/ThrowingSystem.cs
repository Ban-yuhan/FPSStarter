using UnityEngine;

public class ThrowingSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject noiseBallPrefab;

    [SerializeField]
    private Transform throwPoint;

    [SerializeField]
    private float throwForce = 15.0f;

    [SerializeField]
    private KeyCode throwKey = KeyCode.G;

    [SerializeField]
    private Transform cam;


    private void Update()
    {
        if (Input.GetKeyDown(throwKey) == true)
        {
            GameObject ball = Instantiate(noiseBallPrefab, throwPoint.position, Quaternion.identity);

            if (ball != null)
            {
                Rigidbody rb = ball.GetComponent<Rigidbody>();
                if(rb != null)
                {
                    Vector3 forceDirection = cam.transform.forward;
                }
            }
        }
    }
}
