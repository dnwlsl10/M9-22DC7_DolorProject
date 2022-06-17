using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CleanSlider : MonoBehaviour
{
    Slider slHP;
    public GameObject fillArea;
    float fSliderBarTime;
    // Start is called before the first frame update
    void Start()
    {
        slHP = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (slHP.value <= 0) fillArea.SetActive(false);
        else fillArea.SetActive(true);
    }
}
