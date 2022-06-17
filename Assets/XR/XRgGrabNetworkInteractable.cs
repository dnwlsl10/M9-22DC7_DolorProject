using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class XRgGrabNetworkInteractable : XRGrabInteractable
{
    private PhotonView photonView;
    private void Start() {
        photonView = GetComponent<PhotonView>();
    }
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        photonView.RequestOwnership();
        base.OnSelectEntered(args);
    }
}
