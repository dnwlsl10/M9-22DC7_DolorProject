using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby : MonoBehaviour
{
    public SelectionMachine selectionMachine;
    public Garage garage;
    public System.Action<int> OnCompelet;
    public ScenecTigger scenecTigger;
    [SerializeField]
    private bool isTest;
    public void Start()
    {
        if (isTest)
        {
            Debug.Log("Test �Դϴ�.");
            this.Init();
        }
        scenecTigger.OnChangeScene = () =>
        {
            Debug.Log(selectionMachine.selectID);
            OnCompelet(selectionMachine.selectID);
        };
    }

    public void Init()
    {
        DataManager.GetInstance().LoadDatas();
        var robotDatas = DataManager.GetInstance().dicRobotDatas;
        foreach (var data in robotDatas)
        {
            var robotData = data.Value;
            selectionMachine.Init(robotData);
            garage.Init(robotData);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            this.OnCompelet(selectionMachine.selectID);
        };
    }
}
