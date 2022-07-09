using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultUI : MonoBehaviour
{
    [SerializeField] GameObject win;
    [SerializeField] GameObject lose;

    public void ShowResult(bool win)
    {
        StartCoroutine(FadeResult(win ? this.win : this.lose));
    }

    float dissolveMaxValue = 1;
    IEnumerator FadeResult(GameObject obj)
    {
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        Material mat = sr.material;

        float dissolveVal = 0;
        //소리 주아아앙
        //메테리얼-Fade 0부터 1까지
        while (dissolveVal > dissolveMaxValue)
        {
            dissolveVal += 0.001f;
            mat.SetFloat("_Fade", dissolveVal);
            yield return null;
        }
    }
}
