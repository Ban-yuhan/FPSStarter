using UnityEngine;

public class ExplosiveGrenade : MonoBehaviour
{
    [SerializeField]
    private float explosionRadius = 5.0f; //폭발 반경

    [SerializeField]
    private float explosionForce = 700.0f; //폭발력

    [SerializeField]
    private float damage = 50.0f; //데미지

    [SerializeField]
    private GameObject explosionEffect; //폭발 이펙트 프리팹

    private float fuseTime = 2.0f; //점화 시간
    private float fuseTimer;


    private void Awake()
    {
        fuseTimer = 0f;    
    }


    private void Update()
    {
        fuseTimer += Time.deltaTime;
        if(fuseTimer >= fuseTime)
        {
            Explode();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if(damageable != null)
        {
            Explode();
        }   
        
    }


    /// <summary>
    /// 폭발처리/
    /// </summary>
    void Explode()
    {
        if(explosionEffect != null)
        {
            GameObject go = Instantiate(explosionEffect, transform.position, transform.rotation);
            if(go != null)
            {
                Destroy(go, 3.0f); //3초 후 폭발 이펙트 제거
            }
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius); //폭탄 위치로부터 반경 내 충돌한 오브젝트를 배열에 담음

        for(int i = 0; i<colliders.Length; ++i)
        {
            IDamageable damageable = colliders[i].GetComponent<IDamageable>();
            if(damageable != null)
            {
                float DM = 1 - Vector3.Distance(transform.position, colliders[i].transform.position) / explosionRadius ;

                if(DM < 0f)
                {
                    DM = 0f;
                }
                if(DM>0 && DM <0.3f)
                {
                    DM = 0.25f;
                }
                else if(DM >=0.3f && DM <0.6f)
                {
                    DM = 0.5f;
                }
                else if(DM >=0.6f && DM < 0.8f)
                {
                    DM = 0.8f;
                }
                else if(DM >= 0.8f)
                {
                    DM = 1f;
                } 

                    damageable.TakeDamage(damage * DM);
                Debug.Log("적용된 폭발 데미지 : " + damage * DM);
            }

            Rigidbody rb = colliders[i].GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius); //폭발력, 폭발 중심, 폭발 반경
                //rb가 제공하는 함수. 폭발력을 오브젝트에 가해줌. 중심에서 멀수록 약하고, 가까울수록 강한 힘을 받음.
            }
        }

        Destroy(gameObject); //폭발 후 폭탄 오브젝트 제거
    }
}
