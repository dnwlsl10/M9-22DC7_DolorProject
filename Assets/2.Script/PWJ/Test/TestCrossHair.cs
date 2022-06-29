using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCrossHair : MonoBehaviour
{

    public Transform centerEye;
    public Transform enemyTarget;
    public Renderer imageRenderer;
    public Transform crossHairImage;
    public LineRenderer lr;
    private Vector3 origin;

    public LayerMask screenLayer;
    private bool isLook;

    void Start(){
        origin = crossHairImage.position;
        StartCoroutine(GuidedMissileCrossHair());
    }

    IEnumerator CrossHairMove() {

        Vector3 direction = origin + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0f);
        float floatingSpeed = Random.Range(1f, 4f);
        float rotateSpeed = Random.Range(-3f, 3f);

        while (Vector3.Distance(crossHairImage.position, direction) > 0.03f)
        {
            crossHairImage.position = Vector3.Lerp(crossHairImage.position, direction, 0.05f);
            crossHairImage.rotation = Quaternion.Euler(crossHairImage.rotation.eulerAngles + new Vector3(0f, 0f, rotateSpeed));
            yield return null;
        }
    }



    IEnumerator GuidedMissileCrossHair()
    {
        imageRenderer.enabled = true;

        while (true)
        {
            yield return null;

            var dir = enemyTarget.position - centerEye.position;
            var reveresDir = Camera.main.transform.position - enemyTarget.position;
            Ray ray = new Ray(centerEye.position, dir);
            Ray reveresRay = new Ray(enemyTarget.position, reveresDir);
           
            if (Physics.Raycast(ray, out RaycastHit targetHit))
            {
                if (targetHit.transform.CompareTag("Enemy"))
                {
                    isLook = true;
                    StopCoroutine(CrossHairMove());
                    lr.SetPosition(0, ray.origin);
                    lr.SetPosition(1, targetHit.point);
                    if(Physics.Raycast(reveresRay, out RaycastHit screenHit , 100f, screenLayer)){
                        Debug.Log("test");
                        lr.SetPosition(0, reveresRay.origin);
                        lr.SetPosition(1, screenHit.point);
                        crossHairImage.position = Vector3.Lerp(this.crossHairImage.position, screenHit.point, Time.deltaTime * 5f);
                        if(!isLook)
                        {
                            crossHairImage.LookAt(Camera.main.transform.position);
                            isLook = true;
                        }
                        crossHairImage.rotation = Quaternion.Euler(crossHairImage.rotation.eulerAngles + new Vector3(0f, 0f, -1f));
                    }
                }
                else
                {
                    lr.SetPosition(0, ray.origin);
                    lr.SetPosition(1, targetHit.point);
                    crossHairImage.LookAt(Camera.main.transform.position);
                    yield return StartCoroutine(CrossHairMove());
                }
            }
        }

    }
}
