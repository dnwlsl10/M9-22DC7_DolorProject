using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class DotweenTest : MonoBehaviour
{
    public GameObject a;

    //public static Action uiAction;

    //private void Awake()
    //{
    //    uiAction = () =>
    //    {
    //        SpecialAttackComplete();
    //    };
    //}
    void Start()
    {
        Transform aRect = a.GetComponent<RectTransform>();
        SpecialAttackComplete(aRect);
    }
    void Update()
    {

    }

    public void SpecialAttackComplete(Transform aRect)
    {
        aRect.DOScaleX(aRect.localScale.x + 0.03f, 0.5f).OnComplete(() =>
        {
            aRect.DOScaleY(aRect.localScale.y + 0.03f, 0.5f);
        });
    }
}
