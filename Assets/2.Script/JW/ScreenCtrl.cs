using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScreenCtrl : MonoBehaviour
{
    public GameObject[] imagUI;
    private MeshRenderer screenMesh; // 스크린 메쉬
    [SerializeField] private List<Material> screenMat = new List<Material>(); // UV별 Material
    //------------------------ 세팅값
    float min = 0f;
    float max = 1f;
    public float turnTime = 7f;
    public float bufferRate = 0.01f;

    private void Awake()
    {
        screenMesh = GetComponent<MeshRenderer>();
        for (int i = 1; i < screenMesh.materials.Length; i++) //초기 검은 화면 세팅
        {
            if (i == 1 || i == 2)
            {
                screenMesh.sharedMaterials[i].SetVector("Vector2_52CEF5F", new Vector4(-1f, 0f, 0f, 0f)); // Offset
                screenMesh.sharedMaterials[i].SetVector("Vector2_E829674E", new Vector4(-1f, 0f, 0f, 0f)); // Rgb Vector Split
            }
            else if (i >= 3 && i <= 5)
            {
                screenMesh.sharedMaterials[i].SetVector("Vector2_52CEF5F", new Vector4(0f, 1f, 0f, 0f)); // Offset
                screenMesh.sharedMaterials[i].SetVector("Vector2_E829674E", new Vector4(0f, 1f, 0f, 0f)); // Rgb Vector Split
            }
            screenMesh.sharedMaterials[i].SetFloat("Vector1_4D0B16C4", max); // Blur
            screenMesh.sharedMaterials[i].SetFloat("Vector1_2599837B", min); // Exposure
            screenMat.Add(screenMesh.sharedMaterials[i]);
        }
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) //버튼으로 스크린 테스트
        {
            StartCoroutine("OnScreenDelay");
            for (int i = 0; i < imagUI.Length; i++)
            {
                imagUI[i].SetActive(true);
            }
        }
    }

    IEnumerator OnScreenDelay()
    {
        for (int i = 0; i < screenMat.Count; i++) //Mat순서대로 코루틴 동작
        {
            yield return StartCoroutine(ChangeScreen(screenMat[i]));
        }
    }

    IEnumerator ChangeScreen(Material mat) //Mat별 값 수정
    {
        float screenExposure = mat.GetFloat("Vector1_2599837B");
        float screenBlur = mat.GetFloat("Vector1_4D0B16C4");
        while (screenBlur > min)
        {
            screenBlur -= 1 / (7 / bufferRate) * 6;
            mat.SetFloat("Vector1_4D0B16C4", screenBlur);
            mat.SetVector("Vector2_E829674E", new Vector4(screenBlur,0f,0f,0f));
            screenExposure += 1 / (7 / bufferRate) * 6;
            mat.SetFloat("Vector1_2599837B", screenExposure);
            mat.SetVector("Vector2_52CEF5F", new Vector4(screenExposure, 0f, 0f, 0f));

            yield return null;
        }
        yield return new WaitForSeconds(bufferRate);
    }
}
