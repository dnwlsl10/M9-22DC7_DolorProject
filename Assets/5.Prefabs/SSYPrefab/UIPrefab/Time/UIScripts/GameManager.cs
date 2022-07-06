using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public TextRound[] timeText;

    bool isGameStart;
    float playTime;
    void Start()
    {
        InGameManager2.instance.onGameStart += OnGameStart;
        playTime = 150f;
    }
    void FixedUpdate()
    {
        if (isGameStart == true)
        {
            playTime -= Time.fixedDeltaTime; //시간이 떨어지게 해준다.
            int min = (int)(playTime/60);
            int second = (int)(playTime % 60);
            string time = string.Format("{0:00}", min) + ":" + string.Format("{0:00}", second)+"                                          ";
            for(int i = 0; i < timeText.Length; i++)
            {
                timeText[i].text = time;
            }
            
        }
    }

    void OnGameStart()
    {

        isGameStart = true;
    }
}
