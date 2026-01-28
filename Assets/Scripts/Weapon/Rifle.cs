using UnityEngine;

public class Rifle : Weapon //Weapon부모 클래스를 상속
{
    private void Start()
    {
        damage = 10;
        range = 100;
        fireRate = 0.1f;

        ReloadCoolDown = 1.5f;
    }

    private  void Awake()
    {
        MaxBulletsCount = 31;
        base.Awake();
    }

    private void Update()
    {
        base.Update();

        if(Input.GetMouseButton(0) && CanFire())
        {
            Shoot();
        }
    }

    public override void Shoot()
    {
        base.Shoot(); //부모클래스 Shoot()함수의 내용을 가져옴


        RaycastHit hit;

        bool isHit = Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, range);

        if (hitEffectPrefab != null)
        {
            //hit.point : 충돌한 위치정보 -> 충돌한 위치에서 이펙트가 발생하도록
            //uaternion.LookRotation(hit.normal) : 충돌 면을 바라보게끔 회전 -> 충돌면쪽으로 이펙트가 발생하도록 해줌
            GameObject go = Instantiate(hitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(go, 2.0f); //2초 정도 뒤에 파괴.
        }

        if (isHit == true)
        {
            Debug.Log("소총 명중 : " + hit.transform.name); //명중한 대상의 이름 출력
            //Target target = hit.transform.GetComponent<Target>();
            IDamageable target = hit.transform.GetComponent<IDamageable>();

            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }
    }

}
   
