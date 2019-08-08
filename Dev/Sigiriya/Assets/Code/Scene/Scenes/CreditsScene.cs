using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsScene : SceneBase<CreditsScene>
{
    private void OnEnable()
    {
        Debug.Log("Credits scene enabled!");
    }

    public void MenuClicked()
    {
        EventAnnouncer.OnRequestSceneChange?.Invoke(EnumScene.TITLE, true);
    }
}
