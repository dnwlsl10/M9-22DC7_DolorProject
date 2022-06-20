using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGame : MonoBehaviour
{
    private PracticeSystem practiceSystem;
    private Connect connect;
    public GameObject test;

    public void Init(Connect connect){
        this.practiceSystem = this.GetComponentInChildren<PracticeSystem>();
        this.connect = connect;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && !practiceSystem.isPracticeMode && !practiceSystem.isWorking)
        {
            practiceSystem.Init();
                Off();
        }

        if (Input.GetKeyDown(KeyCode.C) && practiceSystem.isPracticeMode && !practiceSystem.isWorking)
        {
            practiceSystem.Exit((obj)=>{
                On();
            });
      
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            connect.OnQuickStart();
        }
    }

    void Off(){
        test.SetActive(false);
    }

    void On(){
        test.SetActive(true);
    }
}
