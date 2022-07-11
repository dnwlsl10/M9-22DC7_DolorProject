using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorEvent : MonoBehaviour
{
    public AudioClip openDoorSFX;
    public GameObject smkVFX;
    void OpenDoorSound(){
        AudioPool.instance.Play(openDoorSFX.name, 2, this.transform.position);
        if(smkVFX !=null){
            smkVFX.gameObject.SetActive(true);
        }

    }

    void CloseSoudn(){
        if (smkVFX != null)
        {
            smkVFX.gameObject.SetActive(false);
        }
    }

}
