using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIExit : MonoBehaviour
{
    public System.Action OnSelceted;
    public AudioClip onTouchSFX;
    private void OnTriggerEnter(Collider other)
    {

            AudioPool.instance.Play(onTouchSFX.name, 2, this.transform.position);
            OnSelceted();

    }
}
