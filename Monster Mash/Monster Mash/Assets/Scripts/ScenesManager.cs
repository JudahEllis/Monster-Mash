using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public enum Scene
    {
        QuickPlay,
        TestRoom,
        MainMenu,
        BAS_NewOrEdit,
        BuildAScare,
        Load
    }

    public void LoadScene(Scene scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }

    // Loads the "first" level, in this case it will be BuildAScare mode.
    // Change as necessary.
    public void LoadNewGame()
    {
        SceneManager.LoadScene(Scene.BuildAScare.ToString());
    }

    // For iterating through different "levels"
    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Use this to load the MainMenu
    //public void LoadMainMenu()
    //{
    //    SceneManager.LoadScene(Scene.MainMenu.ToString());
    //}
}
