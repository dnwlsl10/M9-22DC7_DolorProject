using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBackGround : MonoBehaviour
{
    public GameObject blackbg;
    public void StartChangeSceanBlackBackGround(){
        blackbg.gameObject.SetActive(true);
    }
}
