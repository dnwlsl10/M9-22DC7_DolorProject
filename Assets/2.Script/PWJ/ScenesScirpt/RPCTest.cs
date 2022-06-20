using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class RPCTest : MonoBehaviour
{
    [PunRPC]
    public void GameStartRPC()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            SceneManager.LoadScene("InGame");
        }
        //if (PhotonNetwork.CurrentRoom.PlayerCount == 2) OnChangeScene();
    }
}
