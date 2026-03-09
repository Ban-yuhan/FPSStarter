using UnityEngine;

//투척물에 부착하여 소음의 강도와 위치를 전달하는 컴포넌트
public class NoiseBall : MonoBehaviour
{
    [SerializeField]
    private float noiseRange = 10.0f; //소음의 범위

    [SerializeField]
    private LayerMask zombieLayer; //소음을 감지할 오브젝트의 레이어

    private void OnCollisionEnter(Collision collision)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, noiseRange, zombieLayer);

        for(int i=0; i < hitColliders.Length; ++i)
        {
            INoiseHearable noiseHearable = hitColliders[i].GetComponent<INoiseHearable>();

            if(noiseHearable != null)
            {
                noiseHearable.OnHearNoise(transform.position, 1.0f);
            }
        }
        Destroy(gameObject);
    }
}
