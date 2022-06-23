using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGame : MonoBehaviour
{
    [SerializeField]
    public QuickMatchSystem quickMatchSystem;
    public PracticeSystem practiceSystem;
    public static event System.Action<eRoomMode> OnPracticeMode = (e) => { };
    public static event System.Action OnLobby = () => { };
    public static event System.Action<eRoomMode> OnQucikMatch = (e) => { };

    public void Init(GameObject obj)
    {
        var cokpit = obj.GetComponentInChildren<CockPit>();
        quickMatchSystem.Init(cokpit);
    }

    void EnterPracticeMode() => OnPracticeMode(eRoomMode.PracticeRoom);

    void EnterQuickMatch() => OnQucikMatch(eRoomMode.QuickMatchRoom);

    public void FindOtherPlayer(System.Action OnComplete) => quickMatchSystem.OnAction();

    void EnterLobby() => OnLobby();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) EnterPracticeMode();
        if (Input.GetKeyDown(KeyCode.B)) EnterQuickMatch();
        if (Input.GetKeyDown(KeyCode.C)) EnterLobby();
        if(Input.GetKeyDown(KeyCode.V)) quickMatchSystem.OnAction();
    }
}
