using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour {
    private Player player;
    private float footstepTimer = 0f;
    private float footstepTimerMax = 0.1f;

    private void Awake() {
        player = GetComponent<Player>();
    }

    private void Update() {
        footstepTimer -= Time.deltaTime;
        if (player.IsWalking() && footstepTimer <= 0f) {
            footstepTimer = footstepTimerMax;
            float volume = 1f;
            SoundManager.Instance.PlayFootstepSound(player.transform.position, volume);
        }
    }
}
