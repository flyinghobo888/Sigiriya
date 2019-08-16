using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TaskUIBase : ScrollBoardItemUIBase
{
    //If anything that all task pieces need, put here.
    [SerializeField] protected Task currentTask = null;

    public override bool InitBoardItemUI(ScriptableObject data, Sprite background)
    {
        if (!(data is Task))
        {
            return false;
        }

        Task task = (data as Task);

        itemBackgroundImage.sprite = background;
        parentData = task.RootTask;
        currentTask = task;

        return true;
    }
}
