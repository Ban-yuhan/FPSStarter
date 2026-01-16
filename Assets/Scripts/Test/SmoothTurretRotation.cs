using UnityEngine;

public class SmoothTurretRotation : MonoBehaviour
{
    [SerializeField]
    private Transform targetTransform;

    [SerializeField]
    private float rotationSpeed = 5.0f;

    // Update is called once per frame
    void Update()
    {
        RotateTowardTarget();
    }

    void RotateTowardTarget()
    {
        Vector3 directionToTarget = targetTransform.position - transform.position;
        directionToTarget.y = 0.0f;

        // LookRotation : 특정 방향 벡터를 바라보는 회전값을 만들어 주는 함수.
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        Debug.DrawRay(transform.position, directionToTarget, Color.cyan);
    }
}
