using UnityEngine;

public class FootStep : MonoBehaviour
{
    [SerializeField]
    private AudioSource footstepSource;

    [SerializeField]
    private AudioClip dirtClip;

    [SerializeField]
    private AudioClip metalClip;

    [SerializeField]
    private AudioClip woodClip;

    [SerializeField]
    private AudioClip defaultClip;

    [SerializeField]
    private Transform rayStartPoint;

    [SerializeField]
    private float rayDistance = 1.5f;

    [SerializeField]
    private LayerMask groundmask;


    public void PlayFootStep()
    {
        RaycastHit hit; //Ray에 맞은 대상의 정보가 hit 변수에 저장

        if (Physics.Raycast(rayStartPoint.position, Vector3.down, out hit, rayDistance, groundmask) == true)
        {
            string surfaceTag = hit.collider.gameObject.tag; //해당 오브젝트의 태그정보를 저장.
            AudioClip clipToPlay = null;

            switch (surfaceTag)
            {
                case "Dirt":
                    {
                        clipToPlay = dirtClip;
                    }
                    break;

                case "Metal":
                    {
                        clipToPlay = metalClip;
                    }
                    break;

                case "Wood":
                    {
                        clipToPlay = woodClip;
                    }
                    break;

                default:
                    {
                        clipToPlay = defaultClip;
                    }
                    break;

            }

            if (clipToPlay != null)
            {
                footstepSource.clip = clipToPlay;
                footstepSource.volume = Random.Range(0.8f, 1.0f);
                footstepSource.pitch = Random.Range(0.9f, 1.1f); //pitch : 소리의 높낮이
                footstepSource.Play();
            }
        }
    }
}
