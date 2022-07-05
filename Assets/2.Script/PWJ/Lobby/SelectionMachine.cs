using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionMachine : MonoBehaviour
{
    List<UIRobot> selectRobotList = new List<UIRobot>();
    public GameObject UIRobotPrefab;
    public Transform target;
    public Button btnStart;
    public Text selectText;
    public Button btnRight;
    public Button btnLeft;

    private bool isStart;
    private bool isSelect;
    private int currentIndex;
    public int selectID { get; private set; }

    public System.Action<int> OnSelected;
    public System.Action OnCancle;

    public SelectionGrab rightSelectGrab;
    public SelectionGrab leftSelectGrab;
    public ShapeChangeValue shapeChange;
    public GameObject potal;
    public SmControllerLights controllerLights;

    public void Init(RobotData robotData)
    {
        this.shapeChange = this.GetComponentInChildren<ShapeChangeValue>();
        var obj = Instantiate<GameObject>(UIRobotPrefab, target);
        var robot = obj.GetComponent<UIRobot>();
        robot.Init(robotData);
        selectRobotList.Add(robot);
        potal.gameObject.SetActive(false);
        controllerLights.gameObject.SetActive(false);
    }
    void Start()
    {
        this.shapeChange = this.GetComponentInChildren<ShapeChangeValue>();
        this.isStart = true;
        btnStart.gameObject.SetActive(true);
        btnRight.gameObject.SetActive(false);
        btnLeft.gameObject.SetActive(false);
        this.OnSetBtn();
        potal.gameObject.SetActive(false);
        controllerLights.gameObject.SetActive(false);
    }

    void OnSetBtn()
    {
        rightSelectGrab.onRight = () =>
        {
            if (isSelect) BtnRight();
        };
        leftSelectGrab.onLeft = () =>
        {
            if (isSelect) BtnLeft();
        };
        shapeChange.OnSelected = () =>
        {
            if (isSelect) OnSelect();
        };
    }

    void OnStart()
    {
        if (selectRobotList.Count != 0)
        {
            controllerLights.gameObject.SetActive(true);
            isStart = false;
            isSelect = true;
            this.currentIndex = 0;
            selectRobotList[0].gameObject.SetActive(true);
            leftSelectGrab.OnStartValue();
            rightSelectGrab.OnStartValue();
        }
    }

    void OnSelect()
    {
        isSelect = false;
        this.selectID = selectRobotList[currentIndex].GetRobotID();
        selectRobotList[currentIndex].gameObject.SetActive(false);
        controllerLights.gameObject.SetActive(false);
        OnSelected(selectID);
        leftSelectGrab.OnDefultValue();
        rightSelectGrab.OnDefultValue();
    }
    void BtnLeft()
    {
        this.currentIndex--;
        selectRobotList[currentIndex + 1].gameObject.SetActive(false);
        selectRobotList[currentIndex].gameObject.SetActive(true);

        if (currentIndex == 0)
        {
            leftSelectGrab.isLeftExistence = false;
            rightSelectGrab.isRightExistence = true;
            this.rightSelectGrab.OnChangeGreen();
            this.leftSelectGrab.OnChangeRed();
        }
        else
        {
            leftSelectGrab.isLeftExistence = true;
            rightSelectGrab.isRightExistence = true;
            this.rightSelectGrab.OnChangeGreen();
            this.leftSelectGrab.OnChangeGreen();
        }
    }
    void BtnRight()
    {
        selectRobotList[currentIndex].gameObject.SetActive(false);
        selectRobotList[++currentIndex].gameObject.SetActive(true);

        if (selectRobotList.Count - 1 == currentIndex)
        {
            rightSelectGrab.isRightExistence = false;
            leftSelectGrab.isLeftExistence = true;
            this.rightSelectGrab.OnChangeRed();
            this.leftSelectGrab.OnChangeGreen();
        }
        else
        {
            rightSelectGrab.isRightExistence = true;
            leftSelectGrab.isLeftExistence = true;
            this.rightSelectGrab.OnChangeGreen();
            this.leftSelectGrab.OnChangeGreen();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isStart && other.gameObject.CompareTag("Player"))
        {
            OnStart();
            StartCoroutine(this.shapeChange.OnStart());

        }
    }

    public void OpenDoorCompelet()
    {
        this.shapeChange.isOpening = true;
        potal.gameObject.SetActive(true);
    }
    public void CloseDoorCompelet()
    {

    }
}
