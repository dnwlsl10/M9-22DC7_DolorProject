using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponEvent
{
    void OnSecondButton();
    void OffSecondButton();
    void EventValue(float current, float max);
}
