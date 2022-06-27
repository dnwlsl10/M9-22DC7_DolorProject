using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LoadingScreenProcess : MonoBehaviourPun
{
    public Image progressBar;
    public GameObject bg;
    public PhotonView pv;
    public IEnumerator LoadingPhotonScreenProcess(string sceneName , System.Action OnComplete)
    {
        Debug.Log(PhotonNetwork.IsMasterClient);
      
      
      yield break;
      
    }

    public IEnumerator LoadingNormalScreenProcess(string sceneName)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName);
        ao.allowSceneActivation = false;
        while (!ao.isDone)
        {
            yield return null;
            if (ao.progress < 0.9f)
            {
                Debug.Log("90%");
                progressBar.fillAmount = ao.progress;
            }
            else
            {
                progressBar.fillAmount += 0.01f;
                if (progressBar.fillAmount >= 1f)
                {
                    Debug.Log(ao.isDone);
                    ao.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
