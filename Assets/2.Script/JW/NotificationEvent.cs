using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NotificationEvent : MonoBehaviour
{
    [SerializeField] AnimationCurve curve;
    Tweener floatTweener;
    static Sequence showSequence;

    void Start()
    {
        showSequence = DOTween.Sequence().SetAutoKill(false).Pause()
        .Join(transform.DOLocalRotate(new Vector3(0, 180, 0), 0.5f).From().SetEase(curve))
        .Join(transform.DOScale(0, 0.5f).From().SetEase(curve))
        .AppendInterval(1.5f)
        .Append(transform.DOScale(0, 1f).SetEase(Ease.InBack, 1.6f));
    }

    public static void PlayNotification()
    {
        showSequence.Restart();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // hideSequence.Pause();
            showSequence.Restart();
        }
        // if (Input.GetKeyDown(KeyCode.Escape))
        // {
        //     // showSequence.Pause();
        //     hideSequence.Restart();
        // }
    }
}
