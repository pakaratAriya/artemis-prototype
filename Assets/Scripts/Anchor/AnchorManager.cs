using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.Experimental.Utilities;
using UnityEngine;
using UnityEngine.VR;
using UnityEngine.XR.WSA;
using UnityEngine.XR.WSA.Persistence;

public class AnchorManager : WorldAnchorManager {
    WorldAnchorStore MyWorldAnchorStore;

    WorldAnchor savedAnchor = null;
    // Start is called before the first frame update
    void Start() {
        //MyWorldAnchorStore = WorldAnchorStore.GetAsync(AnchorStoreLoaded);
    }

    // Update is called once per frame
    void Update() {

    }

    private void SaveAnchor() {
    }
}
