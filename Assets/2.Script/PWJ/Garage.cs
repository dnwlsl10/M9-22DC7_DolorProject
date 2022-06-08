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
    private float speed = 100f;
    public DoorValue doorValue;

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
    private void Update()
    {
        this.OpenDoors();
        this.CloseDoors();
    }
    void OpenDoors()
    {
        selectionMachine.OnSelected = (id) => {

            SelectRobot(id, () =>
            {
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
        while (this.rightDoor.position != doorValue.rightMove.position || this.leftDoor.position != doorValue.leftMove.position)
        {
            this.rightDoor.position = Vector3.Lerp(this.rightDoor.position, doorValue.rightMove.position, Time.deltaTime * this.speed);
            this.leftDoor.position = Vector3.Lerp(this.leftDoor.position, doorValue.leftMove.position, Time.deltaTime * this.speed);

            yield return new WaitForSeconds(0.1f);
        }
        OnCompelet();
    }
    IEnumerator StartCloseDoors(System.Action OnCompelet)
    {
        while (this.rightDoor.position != doorValue.rightOrigin.position || this.leftDoor.position != doorValue.leftOrigin.position)
        {
            this.rightDoor.position = Vector3.Lerp(this.rightDoor.position, doorValue.rightOrigin.position, Time.deltaTime * this.speed);
            this.leftDoor.position = Vector3.Lerp(this.leftDoor.position, doorValue.leftOrigin.position, Time.deltaTime * this.speed);

            yield return new WaitForSeconds(0.1f);
        }
        OnCompelet();
    }
    private Robot setActiveObj;
    void SelectRobot(int selectID, System.Action OnCompelet)
    {
        var selectRobot = robots.Find(x => x.robotId == selectID);
        setActiveObj = selectRobot;
        selectRobot.gameObject.SetActive(true);
        OnCompelet();
    }
}
