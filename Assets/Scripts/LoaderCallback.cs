using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderCallback : MonoBehaviour {
    private bool isFirstUpdate = true;
    private void Update() {
        if (isFirstUpdate) {
            isFirstUpdate = false;
            // Load the main menu scene after the first frame
            Loader.LoaderCallback();
        }
    }
}
