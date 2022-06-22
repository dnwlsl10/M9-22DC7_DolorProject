using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletW : MonoBehaviour
{
    private void OnDisable()
    {
        ObjectPooler.ReturnToPool(gameObject);
    }
}
