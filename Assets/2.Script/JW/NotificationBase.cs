using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;
public enum NotiType{HP,Ultimate,BlueZone};
public class NotificationBase : MonoBehaviourPun
{
    protected NotiType type;

    protected void Start() {
        NotificationManager.instance.AddToDictionary(type, this);
        SequenceMaking();
    }

    public Sequence showSequence;
    public virtual void SequenceMaking()
    {
    }
}
