using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    public int robotId { get; private set; }
    public string robotName { get; private set; }

    public void RobotInfo(int robotId, string robotName)
    {
        this.robotId = robotId;
        this.robotName = robotName;
    }
}
