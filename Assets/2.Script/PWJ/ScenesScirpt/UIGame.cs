using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGame : MonoBehaviour
{
    [SerializeField]
    private PracticeSystem practiceSystem;
    private Connect connect;
    public GameObject test;
    public static event System.Action OnPracticeMode;
    public static event System.Action OnLobby;
    public static event System.Action OnQucikMatch;
    public bool isPractice;
    public bool isQuickmach;

    public void Init(Connect connect){
        this.practiceSystem = this.GetComponent<PracticeSystem>();
        this.connect = connect;
    }


    void OnClickParticelMode(){

        if (Input.GetKeyDown(KeyCode.T) && !isPractice && !practiceSystem.isWorking)
        {
            practiceSystem.Init((obj) =>
            {
                isPractice = obj;
                OnPracticeMode();
            });

            Off();
        }

        if (Input.GetKeyDown(KeyCode.C) && practiceSystem.isPracticeMode && !practiceSystem.isWorking)
        {
            practiceSystem.Exit((obj) =>
            {
                isPractice = obj;
                OnLobby();
                On();
            });

        }
    }

    void OnClickQuickMatchMode(){

        if (Input.GetKeyDown(KeyCode.Q))
        {
            connect.OnQuickStart();
        }
    }

    // Update is called once per frame
    void Update()
    {
        OnClickParticelMode();
        OnClickQuickMatchMode();
    }

    void Off(){
        test.SetActive(false);
    }

    void On(){
        test.SetActive(true);
    }
}
