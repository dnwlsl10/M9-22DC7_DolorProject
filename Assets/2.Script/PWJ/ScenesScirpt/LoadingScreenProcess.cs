using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LoadingScreenProcess : MonoBehaviourPun
{
    public Connect connect;
    public Image progressBar;
    public GameObject bg;
    public IEnumerator LoadingPhotonScreenProcess(int sceneindex)
    {

        PhotonNetwork.LoadLevel(sceneindex);
        bg.SetActive(true);
     
        PhotonNetwork._AsyncLevelLoadingOperation.allowSceneActivation = false;

        while (!PhotonNetwork._AsyncLevelLoadingOperation.isDone)
        {
            yield return null;

            if (PhotonNetwork._AsyncLevelLoadingOperation.progress < 0.9f)
            {
                // Debug.Log(PhotonNetwork._AsyncLevelLoadingOperation.progress);
                progressBar.fillAmount = PhotonNetwork._AsyncLevelLoadingOperation.progress;
            }
            else
            {
                progressBar.fillAmount += 0.01f;
                if (progressBar.fillAmount >= 1f)
                {
                    PhotonNetwork._AsyncLevelLoadingOperation.completed += (ao) =>{
                        connect.OnCompelet();
                    };
                    PhotonNetwork._AsyncLevelLoadingOperation.allowSceneActivation = true;
                    yield break;
                }
            }
        }

        // while(PhotonNetwork.LevelLoadingProgress < 1)
        // {
        //     yield return null;

        //     progressBar.fillAmount = PhotonNetwork.LevelLoadingProgress;
        // }
    }

    public IEnumerator LoadingPhotonScreen(int sceneindex)
    {

        PhotonNetwork.LoadLevel(sceneindex);
        
        PhotonNetwork._AsyncLevelLoadingOperation.allowSceneActivation = false;

        while (!PhotonNetwork._AsyncLevelLoadingOperation.isDone)
        {
            yield return null;

            if (PhotonNetwork._AsyncLevelLoadingOperation.progress < 0.9f)
            {
                // Debug.Log(PhotonNetwork._AsyncLevelLoadingOperation.progress);
            }
            else
            {
                yield return new WaitForSeconds(3f);
                PhotonNetwork._AsyncLevelLoadingOperation.allowSceneActivation = true;
            }
        }
    }

    public IEnumerator LoadingNormalScreenProcess(string sceneName , System.Action<AsyncOperation> OnComplete)
    {
        yield return new WaitForSeconds(3f);
        AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName);
        ao.allowSceneActivation = false;
        while (!ao.isDone)
        {
            yield return null;
            if (ao.progress < 0.9f)
            {
                progressBar.fillAmount = ao.progress;
            }
            else
            {
                progressBar.fillAmount += 0.01f;
                if (progressBar.fillAmount >= 1f)
                {
                    OnComplete(ao);
                    yield break;
                }
            }
        }
    }
}
