using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RootTaskUI : TaskUIBase
{
    //Wasn't sure if it was a fancy image or just text.
    [SerializeField] private Image taskLogoImage = null;
    //[SerializeField] private TextMeshProUGUI nameText = null;
    [SerializeField] private TextMeshProUGUI description = null;
    private bool showTaskHeader = false;

    public override bool InitBoardItemUI(ScriptableObject data, Sprite background)
    {
        bool success = base.InitBoardItemUI(data, background);

        if (success)
        {
            ShowTaskLogo(false);

            Task task = data as Task;
            //nameText.text = task.name;
            description.text = task.description;
        }

        return success;
    }

    public void ShowTaskLogo(bool show)
    {
        showTaskHeader = show;
        taskLogoImage.gameObject.SetActive(showTaskHeader);
    }
}
