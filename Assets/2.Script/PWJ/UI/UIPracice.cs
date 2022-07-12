using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPracice : MonoBehaviour
{
    public System.Action OnSelceted;
    public AudioClip onTouchSFX;
    private void OnTriggerEnter(Collider other)
    {

<<<<<<< HEAD
            AudioPool.instance.Play(onTouchSFX.name, 2, this.transform.position);
            OnSelceted();
        
=======
        AudioPool.instance.Play(onTouchSFX.name, 2, this.transform.position);
        OnSelceted();

>>>>>>> develop
    }
}
