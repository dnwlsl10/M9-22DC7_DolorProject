using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorEvent : MonoBehaviour
{
    public AudioClip openDoorSFX;
    void OpenDoorSound(){
        AudioPool.instance.Play(openDoorSFX.name, 2, this.transform.position);
    }

}
