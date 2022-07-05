using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private string[] arrayEnemys; //에너미들의 이름 배열 
    [SerializeField]
    private GameObject enemyPrefab; //에너미 타입의 프리팹

    //재생 제어를 위한 모든 에이전트 리스트 
    private List<BaseGameEntity> entitys;

    public static bool IsGameStop { set; get; } = false;

    private void Awake()
    {
        entitys = new List<BaseGameEntity>();

        for(int i =0; i< arrayEnemys.Length; i++)
        {
            GameObject clone = Instantiate(enemyPrefab);
            Enemy entity = clone.GetComponent<Enemy>();
            entity.SetUp(arrayEnemys[i]);

            entitys.Add(entity);
        }
    }

    private void Update()
    {
        if (IsGameStop == true) return;

        for(int i = 0; i< entitys.Count; i++)
        {
            entitys[i].Updated();
        }
    }

    public static void Stop(BaseGameEntity entity)
    {
        IsGameStop = true;

        entity.PrintText("종료합니다");
    }
}
