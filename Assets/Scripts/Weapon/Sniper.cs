using UnityEngine;

public class Sniper : Weapon
{
    void Start()
    {
        damage = 25f;
        range = 300f;
        fireRate = 3f;

        MaxBulletsCount = 12;
        ReloadCoolDown = 1.5f;
    }

    private void Awake()
    {
        base.Awake();
    }

    void Update()
    {
        base.Update();

        if (Input.GetMouseButtonDown(0) && CanFire())
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
            Debug.Log("저격총 명중 : " + hit.transform.name); //명중한 대상의 이름 출력
            Target target = hit.transform.GetComponent<Target>();

            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }
    }
}
