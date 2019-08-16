using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GoalTaskUI : TaskUIBase
{
    //[SerializeField] private TextMeshProUGUI nameText = null;
    [SerializeField] private Sprite checkboxTodo = null;
    [SerializeField] private Sprite checkboxComplete = null;
    [SerializeField] private TextMeshProUGUI description = null;
    [SerializeField] private Image checkbox = null;


    public override bool InitBoardItemUI(ScriptableObject data, Sprite background)
    {
        bool success = base.InitBoardItemUI(data, background);
        if (success)
        {
            Task task = data as Task;
            //nameText.text = task.name;
            description.text = task.description;
            checkbox.sprite = GetCheckboxSprite(task.isTaskComplete);
        }

        return success;
    }

    private Sprite GetCheckboxSprite(bool isComplete)
    {
        return isComplete ? checkboxComplete : checkboxTodo;
    }
}
