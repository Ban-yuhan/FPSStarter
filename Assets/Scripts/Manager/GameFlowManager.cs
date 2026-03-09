using UnityEngine;
using TMPro;

public class GameFlowManager : MonoBehaviour
{
    [SerializeField]
    private int targetKillCount = 1;

    private int currentKillCount = 0;

    [SerializeField]
    private TMP_Text missionStatusText;

    [SerializeField]
    private GameObject clearPanel;

    [SerializeField]
    private string targetAreaName = "ClearPoint";


    private void Start()
    {
        clearPanel.SetActive(false);
        UpdateUI();
    }

    private void OnEnable()
    {
        MissionEventBus.OnEnemyKilled += UpdateKillCount;
        MissionEventBus.OnAreaReached += CheckAreaMission;
    }


    private void OnDisable()
    {
        MissionEventBus.OnEnemyKilled -= UpdateKillCount;
        MissionEventBus.OnAreaReached -= CheckAreaMission;
    }


    void ShowClear()
    {
        clearPanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    void UpdateUI()
    {
        missionStatusText.text = "Kill Count : " + currentKillCount.ToString() + " / " + targetKillCount.ToString();
    }


    void CheckAreaMission(string areaName)
    {
        if(areaName == targetAreaName && currentKillCount >= targetKillCount)
        {
            ShowClear();
        }
    }


    void UpdateKillCount()
    {
        ++currentKillCount;
        UpdateUI();

        //if(currentKillCount >= targetKillCount)
        //{
        //    ShowClear();
        //}
    }
}
