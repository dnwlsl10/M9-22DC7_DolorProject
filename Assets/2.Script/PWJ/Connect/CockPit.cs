using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CockPit : MonoBehaviour
{
    public UIEarth uIEarth;
    public UIScreen uIScreen;

    private void OnEnable() {
        this.uIEarth.gameObject.SetActive(false);
        this.uIScreen.gameObject.SetActive(true);    
    }
}
