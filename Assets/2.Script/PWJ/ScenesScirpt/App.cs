using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum eSceneType
{
    App, Logo, Title , Lobby, Connect, InGame , Loading
}
public class App : MonoBehaviour
{
    private eSceneType sceneType;
    private UserInfo userInfo;
    private void Awake()
    {
        Screen.SetResolution(1920, 1080, false);
    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        this.ChangeScene(eSceneType.Logo);
    }

    public void ChangeScene(eSceneType sceneType)
    {

        switch (sceneType)
        {
            case eSceneType.Logo:
                {
                    AsyncOperation ao = SceneManager.LoadSceneAsync("Logo");
                    ao.completed += (obj) =>
                    {
                        Debug.Log(obj.isDone);

                        var logo = GameObject.FindObjectOfType<Logo>();

                        logo.Init();

                        logo.onComplete = () =>
                        {
                            this.ChangeScene(eSceneType.Title);
                        };
                    };
                }
                break;

            case eSceneType.Title:
                {
                    AsyncOperation ao = SceneManager.LoadSceneAsync("Title");
                    ao.completed += (obj) =>
                    {
                        Debug.Log(obj.isDone);

                        var title = GameObject.FindObjectOfType<Title>();

                        title.Init();

                        title.OnClick = () =>
                        {
                            this.ChangeScene(eSceneType.Lobby);
                        };
                    };
                }
                break;
            case eSceneType.Lobby:
                {
                    var lobby = GameObject.FindObjectOfType<Lobby>();

                    lobby.Init();

                    lobby.OnCompelet = (id) =>
                    {
                        userInfo = new UserInfo(id);
                        this.ChangeScene(eSceneType.Connect);
                    };
                }
                break;
            case eSceneType.Connect:
                {
                    AsyncOperation ao = SceneManager.LoadSceneAsync("Connect");
                    ao.completed += (obj) =>
                    {
                        Debug.Log(obj.isDone);

                        var connect = GameObject.FindObjectOfType<Connect>();

                        connect.Init(userInfo);

                        connect.OnCompelet = () =>
                        {
                            this.ChangeScene(eSceneType.InGame);
                        };

                    };
                }
                break;
            case eSceneType.InGame:
                {
                    var inGame = GameObject.FindObjectOfType<GameManager>();

                    inGame.Init(userInfo);

                    inGame.OnChangeLobby = () => 
                    {
                        this.ChangeScene(eSceneType.Lobby);
                    };
                }
                break;
        }

    }
}
