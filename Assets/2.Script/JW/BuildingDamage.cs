using UnityEngine;
using System.Collections;

public class BuildingDamage : MonoBehaviour, IDamageable
{
    float hp = 100;

    void IDamageable.TakeDamage(float damage, Vector3 position)
    {
        hp -= damage;

        // if (hp <= 0)
        // {
        //     GetComponent<Project.Scripts.Fractures.FractureThis>().BuildingExplosion();
        // }
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(10);
        GetComponent<Project.Scripts.Fractures.FractureThis>().TEST();
    }
}


// class A()
// {
//     private void OnEnable() {
//         StartCoroutine(Q(10));
//     }
//     IEnumerator Q(float t)
//     {
//         yield return new WaitForSeconds(t);
//         GameObject.setactive(false);
//     }

//     private void OnCollisionEnter(Collision other) {
//         Startcoroutine(Q(3));
//     }

//     private void OnDisable() {
//         GameObject.transform.localPosition = Vector3.zero;
//     }
// }