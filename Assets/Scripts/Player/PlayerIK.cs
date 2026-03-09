using UnityEngine;

public class PlayerIK : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Transform leftHandGrip; //총열에 추가한 빈 오브젝트의 트랜스폼. 왼손 부착용.

    [SerializeField]
    private Transform RighttHandGrip; //총열에 추가한 빈 오브젝트의 트랜스폼. 오른 손 부착용.


    private void OnAnimatorIK(int layerIndex)
    {
        if (leftHandGrip == null)
        {
            return;
        }

        //왼손의 위치와 회전을 타겟에 맞추라고 명령
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f); //1.0으로 설정해야 빈 오브젝트의 위치정보를 정확히 따라감.
        animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandGrip.position);

        //회전(손목 꺾임 등) 맞추기
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandGrip.rotation);


        if (RighttHandGrip == null)
        {
            return;
        }

        //오른손의 위치와 회전을 타겟에 맞추라고 명령
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f); //1.0으로 설정해야 빈 오브젝트의 위치정보를 정확히 따라감.
        animator.SetIKPosition(AvatarIKGoal.RightHand, RighttHandGrip.position);

        //회전(손목 꺾임 등) 맞추기
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);
        animator.SetIKRotation(AvatarIKGoal.RightHand, RighttHandGrip.rotation);
    }
}
