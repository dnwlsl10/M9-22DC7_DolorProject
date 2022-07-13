using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DoorEvent : MonoBehaviour
{
    public AudioClip openDoorSFX;
    public AudioClip inGameOpenDoorSFX;
    public GameObject smkVFX;
    void OpenDoorSound(){

        if (SceneManager.GetActiveScene().buildIndex == 5) AudioPool.instance.Play(inGameOpenDoorSFX.name, 2, this.transform.position);
        else AudioPool.instance.Play(openDoorSFX.name, 2, this.transform.position);

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
