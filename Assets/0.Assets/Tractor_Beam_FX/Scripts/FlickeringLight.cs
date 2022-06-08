using UnityEngine;
using System.Collections;

public class FlickeringLight : MonoBehaviour {

public Light light;
public float lightMin;
public float lightMax;

    private float lightIntensity = 10;


void Start (){

}

void Update (){

    lightIntensity = (Random.Range (lightMin, lightMax));
    light.intensity = lightIntensity;

}
}