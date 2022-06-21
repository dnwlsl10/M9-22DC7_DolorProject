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

    private List<Link> linksList = new List<Link>();

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (linkPrefab != null)
            {

                Link newLink = new Link() { link = Instantiate(linkPrefab) as GameObject };
                newLink.connector = newLink.link.GetComponent<Connector>();
                newLink.targetName = other.name;
                linksList.Add(newLink);

                if (newLink.connector != null)
                {
                    print("적 발견(속도저하, 위치발각)");
                    newLink.connector.MakeConnection(transform.position, other.transform.position);
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
                    //적의 이동속도를 저하 시켜줌.
                    //적의 HP를 0.1씩 깎아줌. (상대방이 스스로 들켰다는 걸 알 수 있게끔.)
                }
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (linksList.Count > 0)
        {
            for (int i = 0; i < linksList.Count; i++)
            {
                if (other.name == linksList[i].targetName)
                {
                    print("Exited");
                    Destroy(linksList[i].link);
                }
            }
        }

    }
}
