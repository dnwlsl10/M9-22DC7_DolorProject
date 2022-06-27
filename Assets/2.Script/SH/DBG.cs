using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBG : MonoBehaviour
{
    [ContextMenu("Dbg")]
    void DBGg()
    {
        print(Mathf.RoundToInt(-0.51f));
        print(Mathf.RoundToInt(0.51f));
    }
}
