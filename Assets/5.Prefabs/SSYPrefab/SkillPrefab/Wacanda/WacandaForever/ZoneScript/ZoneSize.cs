using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneSize : MonoBehaviour
{   
    [Header("ChangeTime")]
    [SerializeField] float changeFirstTime; //50
    [SerializeField] float changeSecondTime; //90
    [SerializeField] float changeThirdTime; //130

    [Header("ChangeScale")]
    [SerializeField] float startSize; //6.5
    [SerializeField] float secondSize; //4.5
    [SerializeField] float thirdSize; //3

    public void Init() {
        GameManager.instance.onGameStart += OnGameStart;
    }


    void OnGameStart()
    {
        StartCoroutine(TestChangeSize());
    }
  
    IEnumerator TestChangeSize()
    {
        transform.localScale = Vector3.one * startSize; //초기 사이즈

        float currentSize = transform.localScale.x; //현재 사이즈

        yield return new WaitForSeconds(changeFirstTime); //50초가 지난 후에

        //바닥에 선그리기

        while(transform.localScale.x > secondSize) //현재 사이즈가 두번째 사이즈까지 반복
        {
            currentSize -=  Time.deltaTime * 0.008f;
            transform.localScale = Vector3.one * currentSize;
            yield return null;
        }
        
        yield return new WaitForSeconds(changeSecondTime - changeFirstTime);

        //바닥에 선그리기

        while (transform.localScale.x > thirdSize)
        {
            currentSize -= Time.deltaTime * 0.008f;
            transform.localScale = Vector3.one * currentSize;
            yield return null;
        }
    }

    private void OnDisable() {
        GameManager.instance.onGameStart -= OnGameStart;
    }
}
