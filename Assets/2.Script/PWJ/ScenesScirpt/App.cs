using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum eSceneType
{
    App, Logo, Title , Lobby, InGame
}
public class App : MonoBehaviour
{
    private eSceneType sceneType;

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

                        title.OnClick = () =>
                        {
                            this.ChangeScene(eSceneType.Lobby);
                        };
                    };
                }
                break;
            case eSceneType.Lobby:
                {
                    AsyncOperation ao = SceneManager.LoadSceneAsync("Lobby");
                    ao.completed += (obj) =>
                    {
                        Debug.Log(obj.isDone);

                        var lobby = GameObject.FindObjectOfType<Lobby>();

                        lobby.Init();

                        lobby.OnCompelet = (id) =>
                        {
                            this.ChangeScene(eSceneType.InGame);
                        };

                    };
                }
                break;
            case eSceneType.InGame:
                {
                    AsyncOperation ao = SceneManager.LoadSceneAsync("InGame");
                    ao.completed += (obj) =>
                    {
                        Debug.Log(obj.isDone);

                        var inGame = GameObject.FindObjectOfType<InGame>();

                    };
                }
                break;
        }

    }
}