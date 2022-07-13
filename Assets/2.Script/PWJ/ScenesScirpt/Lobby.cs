using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Lobby : MonoBehaviour
{
    public SelectionMachine selectionMachine;
    public Garage garage;
    public System.Action<int> OnCompelet;
    public ScenecTigger scenecTigger;
    public GameObject blackBG;
    [SerializeField]
    private bool isTest;

    void Awake(){
        if(PhotonNetwork.IsConnected) PhotonNetwork.Disconnect();
    }
    public void Start()
    {
        if (isTest)
        {
            Debug.Log("Test 모드");
            this.Init();
        }
    }

    public void Init()
    {
        scenecTigger.OnChangeScene = () =>
        {
            Debug.Log(selectionMachine.selectID);
            StartCoroutine(FadeOut());
        };
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
            StartCoroutine(FadeOut());
        };
    }

    public IEnumerator FadeOut()
    {
        yield return new WaitForEndOfFrame();
        blackBG.SetActive(true);

        yield return new WaitForSeconds(3f);
        OnCompelet(selectionMachine.selectID);
    }
}
