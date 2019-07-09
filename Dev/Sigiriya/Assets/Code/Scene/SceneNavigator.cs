using UnityEngine;
using UnityEngine.SceneManagement;

//Authors: Andrew Rimpici
//Responsible for keeping track of which scene we're in.
public class SceneNavigator : ManagerBase<SceneNavigator>
{
    [SerializeField] private bool shouldFade;

    public EnumScene CurrentScene { get; private set; }
    public EnumScene TargetScene { get; private set; }

    private static Fade screenFadeRef;

    private void Awake()
    {
        CurrentScene = (EnumScene)SceneManager.GetActiveScene().buildIndex;
        TargetScene = CurrentScene;

        screenFadeRef = GameMaster.Instance.GetFade();
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

    private void ChangeScene(EnumScene targetScene, bool fade)
    {
        TargetScene = targetScene;
        shouldFade = fade;

        if (shouldFade)
        {
            screenFadeRef.FadeIn("scene_fade");
        }
        else
        {
            GoToNextScene();
        }
    }

    private void GoToNextScene(string fadeID)
    {
        if (fadeID.CompareTo("scene_fade") == 0)
        {
            GoToNextScene();
        }
    }

    private void GoToNextScene()
    {
        Debug.Log("GO TO SCENE: " + TargetScene);
        SceneManager.LoadScene((int)TargetScene);
        CurrentScene = TargetScene;
    }

    public void FadeOutToScene()
    {
        if (shouldFade)
        {
            screenFadeRef.FadeOut("scene_fade");
        }
        else
        {
            screenFadeRef.FadeOutNow();
        }
    }
}

//Authors: Andrew Rimpici
//This enum should match up with all the scenes we have in the game.
//The scene id should match with the enum id.
public enum EnumScene
{
    TITLE,      //Scene ID 0
    GAME,       //Scene ID 1
    CREDITS,    //Scene ID 2

    SIZE
}