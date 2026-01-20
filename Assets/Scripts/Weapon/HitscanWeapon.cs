using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HitscanWeapon : MonoBehaviour
{
    [SerializeField]
    private float damage = 10.0f;

    [SerializeField]
    private float range = 100.0f;

    [SerializeField]
    private float fireRate = 0.2f; //연사 속도

    [SerializeField]
    private Camera fpsCamera; //ray를 쏘기위한 카메라 정보

    [SerializeField]
    private ParticleSystem muzzleFlash; //후에 발사 이펙트를 추가하기 위한 변수

    [SerializeField]
    private GameObject hitEffectPrefab;

    [SerializeField]
    private GameObject CrossHair;

    [SerializeField]
    private int MaxBulletsCount = 31;

    [SerializeField]
    private TMP_Text RemainBullets; 

    private int BulletsCount;
    private float ReloadCoolDown = 1.5f;
    private float ReloadTimer = 0f;
    private bool isReloading;

    private float nextTimetoFire = 0f; //쿨타임 계산을 위한 변수


    private void Awake()
    {
        CrossHair.SetActive(false);
        BulletsCount = MaxBulletsCount;
    }


    private void Update()
    {
        //내가 마우스 버튼을 누르고 있고, 현재 시간이 nextTimetoFire보다 크다면(발사 쿨타임이 지났다면) true를 반환
        //MouseButton 0 : 좌클릭, 1 : 우클릭, 2: 휠버튼
        if (Input.GetMouseButton(0) && Time.time >= nextTimetoFire && BulletsCount > 0)
            //또는 Input.GetButton("Fire1")
        {
            nextTimetoFire = Time.time + fireRate;

            Shoot();
        }

        if (Input.GetMouseButton(1))
        {
            CrossHair.SetActive(true);
        }
        else
        {
            CrossHair.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            isReloading = true;
        }

        if (isReloading)
        {
            ReloadTimer += Time.deltaTime;
            RemainBullets.text = "Reloading...";
            if (ReloadTimer > ReloadCoolDown)
            {
                Reload();
                isReloading=false;
                ReloadTimer = 0f;
            }
        }

        if (!isReloading)
        {
            if (BulletsCount > 0)
            {
                RemainBullets.text = BulletsCount.ToString();
            }
            else
            {
                RemainBullets.text = "Out of Bullets";

            }
        }
    }


    void Shoot()
    {
        if (muzzleFlash != null)
        {
            muzzleFlash.Play(); //파티클 시스템으로 이루어진 이펙트를 재생시키는 함수
        }

        RaycastHit hit; //명중한 대상의 정보를 저장할 변수

        //발사위치, 방향, 대상정보를 저장할 변수, 거리
        bool isHit = Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, range);

        float distance = isHit ? hit.distance : range;
        Debug.DrawRay(fpsCamera.transform.position, fpsCamera.transform.forward * distance , Color.red, 0.5f);

        --BulletsCount;

        if (isHit == true)
        {
            Debug.Log("명중 대상 : " + hit.transform.name); //명중한 대상의 이름 출력
            Target target = hit.transform.GetComponent<Target>();

            if(target != null)
            {
                target.TakeDamage(damage);
            }

            if(hitEffectPrefab != null)
            {
                //hit.point : 충돌한 위치정보 -> 충돌한 위치에서 이펙트가 발생하도록
                //uaternion.LookRotation(hit.normal) : 충돌 면을 바라보게끔 회전 -> 충돌면쪽으로 이펙트가 발생하도록 해줌
                GameObject go = Instantiate(hitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(go, 2.0f); //2초 정도 뒤에 파괴.
            }
        }

    }


    void Reload()
    {
        BulletsCount = MaxBulletsCount;
    }
    
}   
