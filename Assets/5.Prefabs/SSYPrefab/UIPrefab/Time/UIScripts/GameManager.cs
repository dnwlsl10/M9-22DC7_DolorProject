using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviourPunCallbacks
{

    [Header("Time")]
    bool isGameStart; //게임이 실행 중 이라면
    public TextRound[] timeText; //외부에서 넣은 텍스트 모두 넣기.
    private float playTime; //설정해준 플레이타임(current=150/s)

    [Header("Result")]
    public GameObject victory;
    public GameObject defeat;
    MeshRenderer mr;
    Material mat;
    [Header("change value")]
    public float dissolveVal;

    [Header("defult value")]
    [SerializeField] float dissolveMinValue = 0f;
    [SerializeField] float dissolveMaxValue = 1f;

    public static GameManager instance;

    [SerializeField] GameObject mechPrefab;
    [SerializeField] List<Transform> spawnPoint;
    [SerializeField] GameObject networkObjectPool;
    public GameObject myMech { get; private set; }

    [SerializeField] private List<PhotonView> players = new List<PhotonView>();
    private int playerCount = 0;
    public event System.Action onGameStart;
    public System.Action OnChangeLobby;
    public void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else
            instance = this;

        Transform spawn = spawnPoint[PhotonNetwork.IsMasterClient ? 0 : 1];

        myMech = PhotonNetwork.Instantiate(mechPrefab.name, spawn.position, spawn.rotation);
        Instantiate(networkObjectPool);

        photonView.RPC("Ready", RpcTarget.MasterClient);
    }
    public void RegisterMech(PhotonView mech) => players.Add(mech);

    [PunRPC]
    private void Ready()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            playerCount++;
            StartCoroutine(CheckBeforeStart());
        }
    }

    IEnumerator CheckBeforeStart()
    {
        while (playerCount != players.Count || players.Count != PhotonNetwork.CurrentRoom.PlayerCount)
            yield return null;

        photonView.RPC("GameStart", RpcTarget.AllViaServer);
    }

    [PunRPC]
    private void GameStart() => StartCoroutine(StartCountDown());

    IEnumerator StartCountDown()
    {
        yield return new WaitForSecondsRealtime(3);

        foreach (var door in GameObject.FindObjectsOfType<DoorSystem>())
        {
            door.Open();
            StartCoroutine(Deactivate(door.transform.root.gameObject));
        }
        onGameStart?.Invoke();
    }

    IEnumerator Deactivate(GameObject obj)
    {
        yield return new WaitForSecondsRealtime(3f);
        obj.SetActive(false);
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel(0);
    }

    void Start()
    {
        InGameManager2.instance.onGameStart += OnGameStart;
        playTime = 150f; //플레이타임 시간 설정

        // victory.gameObject.SetActive(false);
        // defeat.gameObject.SetActive(false);
         mr = transform.GetComponent<MeshRenderer>(); //월드에서 어떻게 가져올지 질문!!!!!!!!!
        // mat = mr.material;
        // dissolveVal = dissolveMinValue;
        // mat.SetFloat("_Fade", dissolveVal);

        // UI에게 승패여부 전달 --> UI가 승리 패배 띄우기

    }

    void FixedUpdate()
    {
        if (isGameStart == true)
        {
            playTime -= Time.fixedDeltaTime; //시간이 떨어지게 해준다.
            if (playTime <= 0)
            {
                for (int i = 0; i < timeText.Length; i++)
                {
                    timeText[i].text = "TimeOver"+"                                          "; //스페이스바 필요
                }
           
                if (PhotonNetwork.IsMasterClient)
                {
                    photonView.RPC("SendHp", RpcTarget.All);

                }
               return;
            }

            int min = (int)(playTime / 60);
            int second = (int)(playTime % 60);
            string time = string.Format("{0:00}", min) + ":" + string.Format("{0:00}", second) + "                                          "; //스페이스바 필요
            for (int i = 0; i < timeText.Length; i++)
            {
                timeText[i].text = time;
            }
        }
    }

    float enemyHp;
    float myHp;

    [PunRPC]
    private void SendHp()
    {
        photonView.RPC("HP", RpcTarget.All, myMech.GetComponent<Status>().HP); 
    }

    [PunRPC]
    private void HP(float hp, PhotonMessageInfo info)
    {
        if (info.photonView.IsMine) return;

        enemyHp = hp;
        myHp = myMech.GetComponent<Status>().HP;

        result(myHp, enemyHp);
    }

    void OnGameStart()
    {
        isGameStart = true;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //상대방이 자리를 떠났을 때
        GameWin();
    }
    void result(float myhp, float enemyhp) // 적과 내 HP를  비교
    {
        //내 HP가 상대 HP보다 크면 VICTORY
        if (myhp > enemyhp)
        {
            GameWin();
        }
        //내 HP가 상대 HP보다 작으면 DEFEAT
        else if (myhp < enemyhp)
        {
            GameLose();
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
                GameWin();
            else
                GameLose();
        }
        //동일하면 마스터 승리
    }

    void GameWin()
    {
        ResultUI ru = myMech.GetComponentInChildren<ResultUI>();//지정해주기
        ru.ShowResult(true);
        // StartCoroutine(nameof(VictorySG)); <-이거는 UI에서
    }
    void GameLose()
    {
        // UI.instance.ShowResult(false);
        // StartCoroutine(nameof(DefeatSG));
    }
}
