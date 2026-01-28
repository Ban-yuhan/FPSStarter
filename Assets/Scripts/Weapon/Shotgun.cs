using UnityEngine;

public class Shotgun : Weapon
{
    public int pelletscount = 8; //한 번에 나가는 총알 수
    public float spreadAngle = 5.0f; //탄퍼짐 각도

    private void Start()
    {
        damage = 5f;
        range = 30f;
        fireRate = 1f;

        ReloadCoolDown = 3f;
    }


    private void Awake()
    {
        MaxBulletsCount = 8;
        base.Awake();
    }



    private void Update()
    {
        base.Update();

        if(Input.GetMouseButtonDown(0) && CanFire())
        {
            Shoot();
        }
    }

    public override void Shoot()
    {
        base.Shoot();
        for (int i = 0; i < pelletscount; ++i)
        {
            //insideUnitCircle : 반지름이 1인 원
            Vector2 randomCircle = Random.insideUnitCircle * spreadAngle * 0.1f; ;
            //반지름이 1인 원 안에서 랜덤한 점(x,y)가 반환

            Vector3 shootDirection = fpsCamera.transform.forward;
            shootDirection.x += randomCircle.x;
            shootDirection.y += randomCircle.y;

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
                Debug.Log("샷건 파편 명중 : " + hit.transform.name); //명중한 대상의 이름 출력
                Target target = hit.transform.GetComponent<Target>();

                if (target != null)
                {
                    target.TakeDamage(damage);
                }
            }
        }
    }
}
