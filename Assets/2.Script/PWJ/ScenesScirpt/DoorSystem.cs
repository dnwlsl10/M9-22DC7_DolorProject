using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class DoorSystem : MonoBehaviour
{
    public Animator leftDoor;
    public Animator rigthDoor;
    public DoorValue doorValue;

 
    public virtual void Init(eRoomMode eRoom){}
    public virtual void Exit(){}

    public virtual void Open(System.Action OnOpen)
    {
        leftDoor.SetTrigger("open");
        rigthDoor.SetTrigger("open");
        StartCoroutine(OnCheckDoors(() =>
        {
            OnOpen();
        }));
    }

    public virtual void Close(System.Action OnClose)
    {
        leftDoor.SetTrigger("close");
        rigthDoor.SetTrigger("close");
        StartCoroutine(OnCheckDoors(() =>
        {
            OnClose();
        }));
    }
    IEnumerator OnCheckDoors(System.Action OnCompelet)
    {
        yield return new WaitForSeconds(3f);
        OnCompelet();
    }
}