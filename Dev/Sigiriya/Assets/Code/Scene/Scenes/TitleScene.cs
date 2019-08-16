using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : SceneBase<TitleScene>
{
    private void OnEnable()
    {
        //Debug.Log("Title scene enabled!");
    }

    public void StartClicked()
    {
        EventAnnouncer.OnRequestSceneChange?.Invoke(EnumScene.GAME, true);
    }

    public void CreditsClicked()
    {
        EventAnnouncer.OnRequestSceneChange?.Invoke(EnumScene.CREDITS, true);
    }
}
