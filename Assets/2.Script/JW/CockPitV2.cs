using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CockPitV2 : MonoBehaviour
{
    public UIEarth uIEarth;
    public UIManagerV2 uIManagerV2;

    private void OnEnable() {
        this.uIEarth.gameObject.SetActive(false);
        this.uIManagerV2.gameObject.SetActive(true);    
    }
}
