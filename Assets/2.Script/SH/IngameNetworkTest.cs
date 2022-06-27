using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class IngameNetworkTest : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject simulator;
    public bool testMode;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.Instantiate(playerPrefab.name, Vector3.one * Random.Range(0, 3), Quaternion.identity);
        // Instantiate(objPool);
        if (testMode) Invoke("SpawnSimulator", 1);
    }

    private void SpawnSimulator()
    {
        Instantiate(simulator);
    }

}
