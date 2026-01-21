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

    protected override void Awake()
    {
        MaxBulletsCount = 31;
        base.Awake();
        BulletsCount = MaxBulletsCount;
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

        if (isHit == true)
        {
            Debug.Log("소총 명중 : " + hit.transform.name); //명중한 대상의 이름 출력
            Target target = hit.transform.GetComponent<Target>();

            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }
    }

}
   
