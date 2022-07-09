using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideSize : MonoBehaviour
{
    MeshRenderer mr;
   [SerializeField] Material mat;

    [Header("change value")]
    public float changeVal;

    [Header("defult value")]
    [SerializeField] float dissolveMinValue = 1.2f;
    [SerializeField] float dissolveMaxValue = -0.2f;
    void Start()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        mr = transform.GetChild(1).GetComponent<MeshRenderer>();
        mat = mr.material;
        changeVal = dissolveMinValue;
        mat.SetFloat("_Dissolve", changeVal);
        GameManager.instance.onGameStart += OnGameStart;

    }

    IEnumerator ChangeInsideSize()
    {
        while(changeVal > dissolveMaxValue)
        {
            changeVal -= 0.001f;
            mat.SetFloat("_Dissolve", changeVal);
            yield return new WaitForEndOfFrame();
        }
            transform.GetChild(0).gameObject.SetActive(true);
        StartCoroutine(ChangeInsideSizeSecond());
    }
    IEnumerator ChangeInsideSizeSecond()
    {
        while (changeVal < dissolveMinValue) //-0.2에서 1.2f가 될 때까지 반복
        {
            changeVal += 0.001f;
            mat.SetFloat("_Dissolve", changeVal);
            yield return new WaitForEndOfFrame();
        }
            transform.GetChild(1).gameObject.SetActive(false);
    }
    void OnGameStart()
    {
        StartCoroutine(ChangeInsideSize());
    }
}
