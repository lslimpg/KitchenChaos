using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour {
    [SerializeField] private CuttingCounter cuttingCounter;
    [SerializeField] private Image barImage;

    private void Start() {
        cuttingCounter.OnCuttingProgressChanged += CuttingCounter_OnProgressChanged;
        barImage.fillAmount = 0f;
        Hide();
    }

    // private void Update() {
    //     if (cuttingCounter.HasKitchenObject()) {
    //         // Only show the progress bar if there is a kitchen object being cut
    //         Show();
    //     } else {
    //         // Hide the progress bar if there is no kitchen object
    //         Hide();
    //     }
    // }

    private void CuttingCounter_OnProgressChanged(object sender, CuttingCounter.OnCuttingProgressChangedEventArgs e) {
        barImage.fillAmount = e.cuttingProgressNormalized;
    
        if (e.cuttingProgressNormalized == 0F || e.cuttingProgressNormalized == 1F) {
            Hide();
        } else {
            Show();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
