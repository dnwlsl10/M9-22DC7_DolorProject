using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public struct DoorValue
{
    public Transform leftOrigin;
    public Transform rightOrigin;
    public Transform leftMove;
    public Transform rightMove;
}

public class Garage : MonoBehaviour
{
    public Transform rightDoor;
    public Transform leftDoor;
    public Transform content;
    private List<Robot> robots = new List<Robot>();
    public SelectionMachine selectionMachine;
    private float speed = 3f;
    public DoorValue doorValue;
    public Animator leftAni;
    public Animator rightAni;
    private Robot setActiveObj;
    public void Init(RobotData robotData)
    {
        var prefab = Resources.Load<GameObject>("Prefab/" + robotData.prefab_name);
        var obj = Instantiate<GameObject>(prefab, content.transform);
        obj.gameObject.SetActive(false);
        var robot = obj.AddComponent<Robot>();
        robot.RobotInfo(robotData.id, robotData.name);
        robots.Add(robot);
        robot.gameObject.SetActive(false);
    }
    private void Start()
    {
        this.OpenDoors();
        this.CloseDoors();
    }
    void OpenDoors()
    {
        selectionMachine.OnSelected = (id) => {

            SelectRobot(id, () =>
            {
                leftAni.SetTrigger("open");
                rightAni.SetTrigger("open");
                StartCoroutine(StartOpenDoors(() => {
                    selectionMachine.OpenDoorCompelet();
                }));

            });
        };
    }
    void CloseDoors()
    {
        selectionMachine.OnCancle = () =>
        {
            leftAni.SetTrigger("close");
            rightAni.SetTrigger("close");
            StartCoroutine(StartCloseDoors(() => {
                if (setActiveObj != null)
                {
                    this.setActiveObj.gameObject.SetActive(false);
                    setActiveObj = null;
                }
                selectionMachine.CloseDoorCompelet();
            }));
        };
    }
    IEnumerator StartOpenDoors(System.Action OnCompelet)
    {
        while (Vector3.Distance(this.rightDoor.position, doorValue.rightMove.position) > 0.3f || Vector3.Distance(this.leftDoor.position , doorValue.leftMove.position) > 0.3f)
        {
            yield return null;
        }
        OnCompelet();
    }
    IEnumerator StartCloseDoors(System.Action OnCompelet)
    {
        while (Vector3.Distance(this.rightDoor.position, doorValue.rightOrigin.position) > 0.3f || Vector3.Distance(this.leftDoor.position , doorValue.leftOrigin.position) > 0.3f)
        {
            yield return null;
        }
        OnCompelet();
    }

    void SelectRobot(int selectID, System.Action OnCompelet)
    {
        var selectRobot = robots.Find(x => x.robotId == selectID);
        setActiveObj = selectRobot;
        selectRobot.gameObject.SetActive(true);
        OnCompelet();
    }
}
