using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRobot : MonoBehaviour
{
    public Transform robotTarget;
    public Text robotName;
    public Text robotDesc;

    RobotData robotData;
    public void Init(RobotData robotData)
    {
        var prefab = Resources.Load<GameObject>("Prefab/" + robotData.hologram_name);
        Instantiate<GameObject>(prefab, this.robotTarget);
        this.robotData = robotData;
        this.robotName.text = robotData.name;
        this.robotDesc.text = robotData.desc;
        this.gameObject.SetActive(false);
    }

    public int GetRobotID()
    {
        return this.robotData.id;
    }
}
