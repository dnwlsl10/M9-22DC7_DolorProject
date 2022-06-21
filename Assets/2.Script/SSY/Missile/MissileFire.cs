using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileFire : MonoBehaviour
{
    public GameObject missileFactory;
    public GameObject target;
    public Transform firePosition;

    public int maxPosition = 20;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            count = 20;
            Invoke("MakeMissile",0.2f);
        }
    }
    int count; // 발사갯수


    void MakeMissile()
    {
        if(count <= 0)
        {
            return;
        }
        count--;//인보크로 발사갯수를 차감하고 카운트가 0과 같아지면 리턴. 그전까지는 발사시간(0.2f)마다 생성해서 발사해준다.
            GameObject missile = Instantiate(missileFactory);
            missile.transform.position = firePosition.transform.position;
            // 로켓에게 커브에 관련된 점배열을 알려주고싶다.
            Missile m = missile.GetComponent<Missile>();
            m.SetPath(MakePath());
            Invoke("MakeMissile",0.2f);
    }

    //PathData
    Vector3[] MakePath()
    {
        Vector3 dir = new Vector3
            (
            Random.Range(-1f, 1f),
            Random.Range(0.1f, 1f), 
            0
            );
        dir.Normalize();
        dir *= 3.85f;// Y값
        dir += new Vector3(0, 0, -8.25f); // Z값

        Vector3[] path = new Vector3[maxPosition];
        Vector3 p1 = firePosition.transform.position;
        Vector3 p2 = firePosition.transform.position + dir + new Vector3(0, 0, -8.25f);
        Vector3 p3 = target.transform.position;
        for (int i = 0; i < maxPosition; i++)
        {
            float t = (float)i / (maxPosition - 1); //강제타이캐스트
            path[i] = GetCurvePosition(p1, p2, p3, t);
        }
        return path;
    }

    //CurveData
    Vector3 GetCurvePosition(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);

        return Vector3.Lerp(ab, bc, t);
    }
}
