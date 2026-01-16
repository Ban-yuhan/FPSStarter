using UnityEngine;

public class ExplosionScanner : MonoBehaviour
{
    [SerializeField]
    private float explosionRadius = 5.0f;

    [SerializeField]
    private LayerMask targetLayer;


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) == true)
        {
            ScanArea();
        }
    }


    void ScanArea()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius, targetLayer);

        if(hitColliders.Length > 0)
        {
            Debug.Log("감지된 적의 수 : " + hitColliders.Length);

            for (int i = 0; i < hitColliders.Length; ++i)
            {
                if (hitColliders[i] != null)
                {
                    Debug.Log("타겟 발견!!! " + hitColliders[i].gameObject.name); //.name : 해당 오브젝트의 이름정보를 가져옴
                }
            }
        }
        else
        {
            Debug.Log("발견하지 못했음");
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius); //폭발반경 표시
    }

}
