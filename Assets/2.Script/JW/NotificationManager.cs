using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager instance;

    Dictionary<NotiType, NotificationBase> type_To_Base;

    public void AddToDictionary(NotiType type, NotificationBase notibase)
    {
        type_To_Base.Add(type, notibase);
    }

    public void Noti(NotiType type)
    {
        type_To_Base[type].showSequence.Restart();
    }
}
