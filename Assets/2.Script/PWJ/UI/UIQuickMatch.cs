using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIQuickMatch : MonoBehaviour
{   
    public System.Action OnSelceted;
    public AudioClip onTouchSFX;
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == 10)
        {
            AudioPool.instance.Play(onTouchSFX.name, 2, this.transform.position);
            OnSelceted();
        }
    }
}
