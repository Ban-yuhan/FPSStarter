using UnityEngine;
using System.Collections.Generic;

public class RagdollController : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private List<Rigidbody> ragdollRigidbodies = new List<Rigidbody>();


    private void Start()
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>(); //자기 자신을 포함한 자식 오브젝트들의 모든 rigidbody를 가져옴

        for (int i = 0; i < rigidbodies.Length; ++i)
        {
            if (rigidbodies[i].gameObject == gameObject) //rigidbody가 부여되어있는 gameobject가 자신일 경우 패스. 자식 오브젝트들만 리스트에 추가
            {
                continue;
            }

            ragdollRigidbodies.Add(rigidbodies[i]);
        }

        DisableRagdoll();
    }


    public void DisableRagdoll() //랙돌 비활성화. 이게 없으면 시작하자마자 랙돌이 실행되어 픽 쓰러짐.
    {
        for(int i = 0; i < ragdollRigidbodies.Count; ++i) //ragdollRigidbodies의 개수 만큼 반복
        {
            ragdollRigidbodies[i].isKinematic = true; //각 rigidbody를 키네마틱으로 설정하여 물리 영향을 받지 않도록 함
        }

        if(animator != null)
        {
            animator.enabled = true; //애니메이터 활성화
        }
    }


    public void EnableRagdoll() //랙돌 활성화.
    {
        for (int i = 0; i < ragdollRigidbodies.Count; ++i) //ragdollRigidbodies의 개수 만큼 반복
        {
            ragdollRigidbodies[i].isKinematic = false; //각 rigidbody를 키네마틱으로 설정하여 물리 연산이 동작하게끔 함.
        }

        if (animator != null)
        {
            animator.enabled = false; //애니메이터 비활성화
        }
    }
}
