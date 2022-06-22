using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabEvent : MonoBehaviour
{
    public bool isRightGrab;
    public bool isLeftGrab;

    public void OnGrabeRight(bool grabbing) => isRightGrab = grabbing;
    public void OnGrabeLeft(bool grabbing) => isLeftGrab = grabbing;
}
