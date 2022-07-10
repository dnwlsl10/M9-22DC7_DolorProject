using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultUI : MonoBehaviour
{
    [Header("Result")]
    [SerializeField] GameObject victory;
    [SerializeField] GameObject defeat;

    [Header("change value")]
    public float dissolveVal=0;

    [Header("defult value")]
    [SerializeField] float dissolveMinValue = 0f;
    [SerializeField] float dissolveMaxValue = 1f;
    
    public void ShowResult(bool win)
    {
        StartCoroutine(FadeResult(win ? this.victory : this.defeat));
    }

    
    IEnumerator FadeResult(GameObject obj)
    {
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        Material mat = sr.material;

        //소리 주아아앙
        //메테리얼-Fade 0부터 1까지
        while (dissolveVal < dissolveMaxValue)
        {
            dissolveVal += 0.001f;
            mat.SetFloat("_Fade", dissolveVal);
            yield return null;
        }
    }
}
