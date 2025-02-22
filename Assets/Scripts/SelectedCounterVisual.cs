using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour {
    [SerializeField] private ClearCounter clearCounter;
    [SerializeField] private GameObject selectedCounterVisual;
    /**
    * Note: It's important that this event listener is added under
    * Start() so that the Player instance is already set (in Awake())
    */
    private void Start() {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }
    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e) {
        if (e.selectedCounter == clearCounter) {
            Show();
        } else {    
            Hide();
        }
    }
    private void Show() {
        selectedCounterVisual.SetActive(true);
    }
    private void Hide() {
        selectedCounterVisual.SetActive(false);
    }
}
