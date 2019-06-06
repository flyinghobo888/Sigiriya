using UnityEngine;
using UnityEngine.SceneManagement;

//Authors: Andrew Rimpici
//Responsible for keeping track of which scene we're in.
public class SceneNavigator : ManagerBase<SceneNavigator>
{
    public EnumScene CurrentScene { get; private set; }
    public EnumScene TargetScene { get; private set; }

    private void Awake()
    {
        CurrentScene = (EnumScene)SceneManager.GetActiveScene().buildIndex;
        TargetScene = CurrentScene;
    }

    //Register the events to listen for in OnEnable
    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneChanged;

        EventAnnouncer.OnRequestSceneChange += ChangeScene;
    }

    //Unregister the events in OnDisable
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneChanged;

        EventAnnouncer.OnRequestSceneChange -= ChangeScene;
    }

    private void SceneChanged(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Level was loaded!");
    }

    private void ChangeScene(EnumScene targetScene, bool shouldFade)
    {
        //TODO: Change the scene with/without fade
        Debug.Log("CHANGE THE SCENE WITH/WITHOUT FADE");
    }
}

//Authors: Andrew Rimpici
//This enum should match up with all the scenes we have in the game.
//The scene id should match with the enum id.
public enum EnumScene
{
    TITLE,  //Scene ID 0
    GAME    //Scene ID 1
}