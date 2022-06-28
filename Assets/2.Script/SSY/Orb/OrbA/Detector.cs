using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link
{
    public GameObject link;
    public Connector connector;
    public string targetName;
}
public class Detector : MonoBehaviour
{
    public GameObject linkPrefab;
    SkillShield skillShield;
    private List<Link> linksList = new List<Link>();


    // other가 skillshild스크립트를 가지고 있으면(if) skillshield의 isLink를 true로 한다.
    // 그리고 skillshield의 stopweaponaction을 호출한다. //이미 상대방이 쉴드를 사용 중이었다면 강제로 끊어내준다.
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy") //내가 아닌 나 == 상대방
        {
            if (linkPrefab != null) //링크프리팹이 들어있다면 그대로 진행
            {

                Link newLink = new Link() { link = Instantiate(linkPrefab) as GameObject }; //게임 오브젝트 생성해서 
                newLink.connector = newLink.link.GetComponent<Connector>(); 
                newLink.targetName = other.name;
                linksList.Add(newLink);

                    skillShield = GetComponent<SkillShield>();
                if (newLink.connector != null)
                {
                    print("적 발견(속도저하, 위치발각)");
                    newLink.connector.MakeConnection(transform.position, other.transform.position); //오브에서 적을 연결!
                    //skillShield.isLinked = true;
                    skillShield.StopWeaponAction();
                }
            }

        }
    }

    void OnTriggerStay(Collider other)
    {
        if (linksList.Count > 0)
        {
            for (int i = 0; i < linksList.Count; i++)
            {
                if (other.name == linksList[i].targetName)
                {
                    print("update link");
                    linksList[i].connector.MakeConnection(transform.position, other.transform.position);
                }
            }
        }
    }

    // isLinked를 다시 false로 바꾼다
    void OnTriggerExit(Collider other)
    {
        if (linksList.Count > 0)
        {
            for (int i = 0; i < linksList.Count; i++)
            {
                if (other.name == linksList[i].targetName)
                {
                    print("Exited");
                    skillShield = GetComponent<SkillShield>();
                    // skillShield.isLinked = false;
                    Destroy(linksList[i].link);
                }
            }
        }

    }
}
