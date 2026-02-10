using System;
using UnityEngine;

[Serializable] //클래스를 직렬화 시키기 위한 문구.
public class GameData
{
    public int score;
    public int bestScore;
    public float bgmVolume;
    public string playerName;

    public Vector3 playerPosition;

    public GameData() //생성자. MonoBehavier의 Start와 같은 역할
    {
        score = 0;
        bestScore = 0;
        bgmVolume = 0.5f;
        playerName = "Player";
        playerPosition = new Vector3(0.0f, 1.0f, 0.0f);
    }
}
