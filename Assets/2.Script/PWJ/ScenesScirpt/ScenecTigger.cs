using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenecTigger : MonoBehaviour
{

    public System.Action OnChangeScene;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) {
            OnChangeScene();
        }
    }
}
