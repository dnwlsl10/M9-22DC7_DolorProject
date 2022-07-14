using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneWall : MonoBehaviour
{
    public GameObject player;
    // private string playername;

    public int outZoneDamage = 10;
    public AudioClip insideZone;
    public AudioClip outsideZone;
    private bool inZone;
    // void Start()
    // {
        // player = GameObject.FindGameObjectWithTag("Player"); //플레이어 나중에 변경.
        
        // playername = player.transform.name; //플레이어의 이름은 플레이어 태그를 가진 것의 이름.
    // }

    // Update is called once per frame
    void OnTriggerExit(Collider other) //아웃 상태일 때
    {
        if (other.gameObject == GameManager.instance.myMech)
        {
            //포스트프로세싱 아웃버전 
            inZone = false;
            StartCoroutine(DamagePlayer());
            AudioPool.instance.Play(outsideZone.name, 2 , other.transform.position);
            LightManager.instance.OnDamageLight();
        }
    }
    void OnTriggerEnter(Collider other) //인 상태일 때
    {
        if(other.gameObject == GameManager.instance.myMech)
        {
            //포스트프로세싱 인버전 
            inZone = true; 
            AudioPool.instance.Play(insideZone.name, 2, other.transform.position);
            LightManager.instance.OFFDamageLight();
        }
    }
    IEnumerator DamagePlayer()
    {
        player = GameManager.instance.myMech;
        while(inZone == false)
        {
            if(player == null) yield break;

            player.gameObject.GetComponent<Status>().TakeDamage(outZoneDamage, Vector3.zero);
            yield return new WaitForSeconds (1f);
            // if(inZone == true)
            // yield break;
        }
        player = null;
    }

}