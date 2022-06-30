using UnityEngine;
using Photon.Pun;

public class SceneAttribute : MonoBehaviour
{
    public enum Online {OfflineMode, OnlineMode}
    public Online sceneMode;
    private void Reset() => transform.SetSiblingIndex(0);
    private void Awake() 
    {
        PhotonNetwork.SingleMode = sceneMode == Online.OfflineMode;
        #if UNITY_EDITOR
            Debug.LogWarning(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + " is " + sceneMode.ToString() + " Scene.");
        #endif
    }
}
