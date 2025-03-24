using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour {

    private enum Mode {
        LookAt,
        LookAtInverted,
        CameraForward,
        CameraForwardInverted
    };
    [SerializeField] private Mode mode;
    // This function runs after Update()
    private void LateUpdate() {
        // No worries about performance since Unity now caches
        // the Camera.main game object instead of previously
        // where it cycles through all game objects to find it.
        switch (mode) {
            case Mode.LookAt:
                transform.LookAt(Camera.main.transform);
                break;
            case Mode.LookAtInverted:
                Vector3 dirFromCamera = transform.position - Camera.main.transform.position;
                transform.LookAt(transform.position + dirFromCamera);
                break;
            case Mode.CameraForward:
                transform.forward = Camera.main.transform.forward;
                break;
            case Mode.CameraForwardInverted:
                transform.forward = -Camera.main.transform.forward;
                break;
        }
        
    }
}
