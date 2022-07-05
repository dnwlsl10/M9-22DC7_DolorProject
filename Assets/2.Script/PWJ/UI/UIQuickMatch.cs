using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIQuickMatch : MonoBehaviour
{   
    public System.Action OnSelceted;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == 10)
        {
            OnSelceted();
        }
    }
}
