using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby : MonoBehaviour
{
    public Button btnStartGame;
    public System.Action OnCompelet;
    public void Start()
    {
        this.btnStartGame.onClick.AddListener(() =>
        {
            this.OnCompelet();
        });
    }
}
