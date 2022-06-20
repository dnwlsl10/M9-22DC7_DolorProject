using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PracticeSystem : MonoBehaviour
{
   public Animator leftDoor;
   public Animator rigthDoor;
   public DoorValue doorValue;
   public bool isPracticeMode {get; private set;}
   public bool isWorking {get; private set; }

    public void Init()
    {
        isWorking = true;
        leftDoor.SetTrigger("open");
        rigthDoor.SetTrigger("open");
        StartCoroutine(OnCheckDoors(() =>{
            isPracticeMode = true;
            isWorking = false;
        }));
    }

    public void Exit(System.Action<bool> OnCompelet){
        isWorking = true;
        leftDoor.SetTrigger("close");
        rigthDoor.SetTrigger("close");
        StartCoroutine(OnCheckDoors(() =>{
            OnCompelet(isPracticeMode = false);
            isWorking = false;
        }));
    }
    IEnumerator OnCheckDoors(System.Action OnCompelet)
    {
        yield return new WaitForSeconds(3f);
        OnCompelet();
    }
}
