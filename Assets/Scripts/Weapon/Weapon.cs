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

    public GameObject hitEffectPrefab;

    private float nextFireTime = 0f;

    private GameObject CrossHair;

    private TMP_Text RemainBullets;


    protected void Awake()
    {
        BulletsCount = MaxBulletsCount;

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
