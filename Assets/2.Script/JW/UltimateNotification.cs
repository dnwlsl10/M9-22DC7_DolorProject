using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UltimateNotification : NotificationBase
{    public override void SequenceMaking()
    {
        showSequence = DOTween.Sequence().SetAutoKill(false).Pause()
        .Join(transform.DOLocalRotate(new Vector3(0, 180, 0), 0.5f).From().SetEase(Ease.OutBounce))
        .Join(transform.DOScale(0, 0.5f).From().SetEase(Ease.OutBounce))
        .AppendInterval(1.5f)
        .Append(transform.DOScale(0, 1f).SetEase(Ease.InBack, 1.6f));
    }
}
