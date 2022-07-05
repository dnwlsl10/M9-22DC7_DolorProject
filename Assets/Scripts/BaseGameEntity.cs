using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGameEntity : MonoBehaviour
{
    //실재(實在, reality)란 인식 주체로부터 독립해 객관적으로 존재한다고 여겨지는 것을 말한다
    //다음유효한아뒤
    private static int nextValidID = 0;

    //BaseGameEntity를 상속받는 모든 게임오브젝트는 ID번호를 부여받는데
    //이 번호는 0부터 시작해서 1씩 증가 (현실의 주민등록번호처럼 사용)
    private int id;

    public int ID
    {
        set
        {
            id = value;
            nextValidID++;
        }
        get
        {
            return id;
        }
    }

    private string entityName; // 에이전트 이름 

    /// <summary>
    /// 파생 클래스에서 base.SetUp()으로 호출 
    /// </summary>
    /// <param name="agent 이름"></param>
    public virtual void SetUp(string name)
    {
        ID = nextValidID;

        entityName = name;

        PrintText(ID, name);
    }

    //GameController 클래스에서 모든 에이전트 Updated()를 호출해 에이전트를 구동한다.
    public abstract void Updated();

    public void PrintText(int id, string name)
    {
        Debug.Log(ID + " : " + entityName);
    }

    public void PrintText(string text)
    {
        Debug.Log(text);
    }
}
