using UnityEngine;

public class GrenadeLauncher : MonoBehaviour
{
    [SerializeField]
    private GameObject grenadePrefab;

    [SerializeField]
    private Transform firePoint; //발사 위치

    [SerializeField]
    private float throwForce = 15.0f; //투척 힘(투척 힘)


    private void Update()
    {
        if(Input.GetMouseButtonDown(2)) //마우스 중간 버튼 클릭 감지
        {
            LaunchGrenade();
        }
    }


    void LaunchGrenade()
    {
        GameObject go = Instantiate(grenadePrefab, firePoint.position, firePoint.rotation);

        if (go != null)
        {
            Rigidbody rb = go.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddForce(firePoint.forward * throwForce, ForceMode.Impulse); //힘 적용 방향 / ForceMode.Impulse: 순간적인 힘 적용 
            }
        }
    }
}
