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
    private bool isCancle;
    private int currentIndex;
    public int selectID { get; private set; }

    public System.Action<int> OnSelected;
    public System.Action OnCancle;

    public void Init(RobotData robotData)
    {
        var obj = Instantiate<GameObject>(UIRobotPrefab, target);
        var robot = obj.GetComponent<UIRobot>();
        robot.Init(robotData);
        selectRobotList.Add(robot);
    }
    void Start()
    {
        this.isStart = true;
        btnStart.gameObject.SetActive(true);
        btnRight.gameObject.SetActive(false);
        btnLeft.gameObject.SetActive(false);
        this.OnSetBtn();
    }
    void OnSetBtn()
    {
        btnStart.onClick.AddListener(() => {

            if (isStart) BtnStart();

            else if (isSelect) BtnSelect();

            else if (isCancle) BtnCancle();
        });
        btnRight.onClick.AddListener(() => {

            BtnRight();
        });
        btnLeft.onClick.AddListener(() => {

            BtnLeft();
        });
    }
    void BtnCancle()
    {
        isCancle = false;
        selectText.text = "Closeing...";
        btnStart.interactable = false;
        OnCancle();
    }
    void BtnStart()
    {
        if (selectRobotList.Count != 0)
        {
            isStart = false;
            isSelect = true;
            this.currentIndex = 0;
            selectRobotList[0].gameObject.SetActive(true);
            btnRight.gameObject.SetActive(true);
            selectText.text = "Select";
        }
    }
    public void OpenDoorCompelet()
    {
        isCancle = true;
        btnStart.interactable = true;
        selectText.text = "Cancle";
    }
    public void CloseDoorCompelet()
    {
        isStart = true;
        btnStart.interactable = true;
        selectText.text = "Start";
    }
    void BtnSelect()
    {
        isSelect = false;
        this.selectID = selectRobotList[currentIndex].GetRobotID();
        selectRobotList[currentIndex].gameObject.SetActive(false);
        selectText.text = "Opening...";
        btnStart.interactable = false;
        btnRight.gameObject.SetActive(false);
        btnLeft.gameObject.SetActive(false);
        OnSelected(selectID);
    }
    void BtnLeft()
    {
        this.currentIndex--;
        selectRobotList[currentIndex + 1].gameObject.SetActive(false);
        selectRobotList[currentIndex].gameObject.SetActive(true);

        if (currentIndex == 0)
        {
            btnLeft.gameObject.SetActive(false);
            btnRight.gameObject.SetActive(true);
        }
        else
        {
            btnLeft.gameObject.SetActive(true);
            btnRight.gameObject.SetActive(true);
        }
    }
    void BtnRight()
    {
        selectRobotList[currentIndex].gameObject.SetActive(false);
        selectRobotList[++currentIndex].gameObject.SetActive(true);

        if (selectRobotList.Count - 1 == currentIndex)
        {
            btnRight.gameObject.SetActive(false);
            btnLeft.gameObject.SetActive(true);
        }
        else
        {
            btnRight.gameObject.SetActive(true);
            btnLeft.gameObject.SetActive(true);
        }
    }
}
