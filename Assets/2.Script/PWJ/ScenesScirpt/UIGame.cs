using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGame : MonoBehaviour
{
    [SerializeField]
    public GameObject test;
    public static event System.Action<eRoomMode> OnPracticeMode =(e)=>{};
    public static event System.Action OnLobby =()=>{};
    public static event System.Action<eRoomMode> OnQucikMatch =(e)=>{};
    public bool bPracticeMode;
    public bool bQuickMatch;

    public void Init()  => test.SetActive(true);
    
    void EnterPracticeMode() => OnPracticeMode(eRoomMode.PracticeRoom);
    
    void EnterQuickMatch() => OnQucikMatch(eRoomMode.QuickMatchRoom);
      
    void EnterLobby() => OnLobby();
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A)) EnterPracticeMode();
        if(Input.GetKeyDown(KeyCode.B)) EnterQuickMatch();
        if(Input.GetKeyDown(KeyCode.C)) EnterLobby();
    }
}
