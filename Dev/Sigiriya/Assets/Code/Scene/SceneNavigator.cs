using UnityEngine;
using UnityEngine.SceneManagement;

//Authors: Andrew Rimpici
//Responsible for keeping track of which scene we're in.
public class SceneNavigator : ManagerBase<SceneNavigator>
{
    public EnumScene CurrentScene { get; private set; }
    public EnumScene TargetScene { get; private set; }

    private static Fade sceneFade;

    private void Awake()
    {
        if (!sceneFade)
        {
            sceneFade = Instantiate(Resources.Load<GameObject>("Unity/Prefabs/_FADE")).GetComponent<Fade>();
            DontDestroyOnLoad(sceneFade);
        }

        CurrentScene = (EnumScene)SceneManager.GetActiveScene().buildIndex;
        TargetScene = CurrentScene;
    }

    //Register the events to listen for in OnEnable
    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneChanged;

        EventAnnouncer.OnRequestSceneChange += ChangeScene;
        EventAnnouncer.OnEndFadeIn += GoToNextScene;
    }

    //Unregister the events in OnDisable
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneChanged;

        EventAnnouncer.OnRequestSceneChange -= ChangeScene;
        EventAnnouncer.OnEndFadeIn -= GoToNextScene;
    }

    private void SceneChanged(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Level was loaded!");
    }

    private void ChangeScene(EnumScene targetScene, bool shouldFade)
    {
        //TODO: Change the scene with/without fade
        Debug.Log("CHANGE THE SCENE WITH/WITHOUT FADE");

        TargetScene = targetScene;

        if (shouldFade)
        {
            sceneFade.FadeIn(((int)TargetScene).ToString());
        }
        else
        {
            GoToNextScene();
        }
    }

    private void GoToNextScene(string fadeID)
    {
        if (fadeID == ((int)TargetScene).ToString())
        {
            GoToNextScene();
        }
    }

    private void GoToNextScene()
    {
        SceneManager.LoadScene((int)TargetScene);
        CurrentScene = TargetScene;
    }

    public void FadeOutToScene()
    {
        sceneFade.FadeOut(((int)CurrentScene).ToString());
    }
}

//Authors: Andrew Rimpici
//This enum should match up with all the scenes we have in the game.
//The scene id should match with the enum id.
public enum EnumScene
{
    TITLE,  //Scene ID 0
    GAME,   //Scene ID 1

    SIZE
}