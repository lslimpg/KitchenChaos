using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader {
    public enum Scene {
        MainMenuScene,
        GameScene,
        LoadingScene,
    }
    private static Scene targetScene;

    public static void Load(Scene targetScene) {
        Loader.targetScene = targetScene;

        // Load the loading scene first
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    public static void LoaderCallback() {
        // This method is called when the loading scene is loaded
        // Load the target scene
        SceneManager.LoadScene(targetScene.ToString());
    }
}