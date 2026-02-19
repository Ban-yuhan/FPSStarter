using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Image healthImage;

    [SerializeField]
    private PlayerHealth playerHealth;

    [SerializeField]
    private Image damageFlashImage;

    [SerializeField]
    private Image crosshairImage;

    [SerializeField]
    private float flashSpeed = 2.0f; //피격 플래시가 사라지는 속도

    [SerializeField]
    private Color hitMarkerColor = Color.red;

    [SerializeField]
    private Weapon currentWeapon;

    [SerializeField]
    private HitscanWeapon hitscanWeapon;

    [SerializeField]
    private DamageVignette damageVignette;

    private Color originalCrosshairColor;


    private void Awake()
    {
        originalCrosshairColor = crosshairImage.color;
    }


    void OnEnable() //UI매니저가 활성화 될 때 실행되는 함수
    {
        if(playerHealth != null)
        {
            playerHealth.OnHealthChanged += UpdateHealthUI;
            playerHealth.OnHealthChanged += damageVignette.UpdateVignette;
        }

        if(currentWeapon != null)
        {
            currentWeapon.OnEnemyHit += ShowHitMarker;
        }

        if(hitscanWeapon != null)
        {
            hitscanWeapon.OnEnemyHit += ShowHitMarker;
        }

    }


    private void OnDisable()
    {
        if(playerHealth != null)
        {
            playerHealth.OnHealthChanged -= UpdateHealthUI;
        }

        if (currentWeapon != null)
        {
            currentWeapon.OnEnemyHit -= ShowHitMarker;
        }

        if(hitscanWeapon != null)
        {
            hitscanWeapon.OnEnemyHit -= ShowHitMarker;
        }
    }


    void UpdateHealthUI(float percent)
    {
        if(healthImage != null)
        {
            healthImage.fillAmount = percent;
        }

        if(percent < 1.0f && damageFlashImage != null)
        {
            StartCoroutine(DamageFlashRoutine()); 
        }
    }


    IEnumerator DamageFlashRoutine()
    {
        Color flashColor = damageFlashImage.color;
        flashColor.a = 0.5f; //반투명
        damageFlashImage.color = flashColor;

        //서서히 투명해지게
        while(damageFlashImage.color.a > 0f)
        {
            flashColor.a -= Time.deltaTime * flashSpeed;
            if (flashColor.a < 0.0f)
            {
                flashColor.a = 0.0f;
            }

            damageFlashImage.color = flashColor;
            yield return null; //다음 프레임까지 대기
        }
    }

    

    void ShowHitMarker()
    {
        if(crosshairImage != null)
        {
            StopCoroutine(HitMarkerRoutine());
            StartCoroutine(HitMarkerRoutine());
        }
    }


    IEnumerator HitMarkerRoutine()
    {
        crosshairImage.color = hitMarkerColor;
        yield return new WaitForSeconds(0.1f); 
        crosshairImage.color = originalCrosshairColor;
    }

    
    void UpdateRemainBullets(int curBullets)
    {

    }
}
