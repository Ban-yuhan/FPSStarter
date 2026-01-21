using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    protected float damage;
    protected float range; //사거리
    protected float fireRate; //연사 속도

    protected int MaxBulletsCount; //최대 탄환 수
    protected float ReloadCoolDown; //재장전에 걸릴 시간

    protected int BulletsCount;
    private float ReloadTimer = 0f;
    private bool isReloading;

    public Camera fpsCamera;
    public ParticleSystem muzzleFlash; //총 발사 시 총구에서 보여줄 발사 이펙트

    private float nextFireTime = 0f;

    [SerializeField]
    private GameObject CrossHair;

    [SerializeField]
    private TMP_Text RemainBullets;

    
    protected virtual void Awake()
    {
        CrossHair = GameObject.Find("CrossHair");

        GameObject TMPBulletsCount = GameObject.Find("BulletsCountText");
        if(TMPBulletsCount != null)
        {
            RemainBullets = TMPBulletsCount.GetComponent<TMP_Text>();
        }
    }


    protected void Update()
    {
        if (CrossHair != null)
        {
            if (Input.GetMouseButton(1))
            {
                CrossHair.SetActive(true);
            }
            else
            {
                CrossHair.SetActive(false);
            }
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
            }
        }
        if (!isReloading)
        {
           if (BulletsCount <= 0)
            {
                RemainBullets.text = "Out of Bullets";
            }
            else
            {
                RemainBullets.text = BulletsCount.ToString();
            }
        }
    }


    public virtual void Shoot()
    {
        if(muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        Debug.Log("기본 무기 발사");
        --BulletsCount;
    }
    

    public bool CanFire()
    {
        if(Time.time >= nextFireTime && BulletsCount > 0 && !isReloading)
        {
            nextFireTime = Time.time + fireRate;
            return true;
        }

        return false;
    }

    protected void Reload()
    {
        BulletsCount = MaxBulletsCount;
        isReloading = false;
        ReloadTimer = 0f;
    }
}
