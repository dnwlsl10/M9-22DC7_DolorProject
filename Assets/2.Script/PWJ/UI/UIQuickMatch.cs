using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIQuickMatch : MonoBehaviour
{   
    public System.Action OnSelceted;

    private void OnTriggerEnter(Collider other) {
        OnSelceted();     
    }
}
