using UnityEngine;
using UnityEngine.UI;

public class GameControll : MonoBehaviour
{
    [SerializeField]
    private GameObject Panel;


    //[SerializeField]
    //private FPSCameraController CameraCtrl;

    private bool isMenu = false;

    private void Awake()
    {
        Panel.SetActive(false);
        //if (CameraCtrl != null)
        //{
        //    FPSCameraController CameraCtrl = FindAnyObjectByType<FPSCameraController>();
        //}
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape)) 
        {
            if (!isMenu)
            {
                Menu();
            }
            else if (isMenu)
            {
                Resume();
            }
        }

        
    }

    public void Resume()
    {
        Time.timeScale = 1;
        Panel.SetActive(false);
        isMenu = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    void Menu()
    {
        Time.timeScale = 0;
        Panel.SetActive(true);
        isMenu = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
