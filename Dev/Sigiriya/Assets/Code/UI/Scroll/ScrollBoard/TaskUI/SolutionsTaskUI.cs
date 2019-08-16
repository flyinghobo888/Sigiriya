using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SolutionsTaskUI : TaskUIBase
{
    [SerializeField] private Sprite checkboxTodo = null;
    [SerializeField] private Sprite checkboxComplete = null;
    [SerializeField] List<RectTransform> solutionSlots = new List<RectTransform>();
    private List<Task> solutionTasks = new List<Task>();

    public override bool InitBoardItemUI(ScriptableObject data, Sprite background)
    {
        bool success = base.InitBoardItemUI(data, background);
        if (success)
        {
            foreach (RectTransform slot in solutionSlots)
            {
                slot.GetComponentInChildren<Image>().sprite = checkboxTodo;
                slot.GetComponentInChildren<TextMeshProUGUI>().text = "";
                slot.gameObject.SetActive(false);
            }
        }

        return success;
    }

    public void InitSlots(List<Task> tasks)
    {
        solutionTasks = tasks;

        for (int i = 0; i < solutionTasks.Count && i < solutionSlots.Count; ++i)
        {
            RectTransform slot = solutionSlots[i];
            Task task = solutionTasks[i];
            TextMeshProUGUI slotText = slot.GetComponentInChildren<TextMeshProUGUI>(true);
            slotText.text = task.description;

            Image slotCheckbox = slot.GetComponentInChildren<Image>(true);
            slotCheckbox.sprite = GetCheckboxSprite(task.isTaskComplete);

            slot.gameObject.SetActive(true);
        }
    }

    private Sprite GetCheckboxSprite(bool isComplete)
    {
        return isComplete ? checkboxComplete : checkboxTodo;
    }

    public int GetSlotCount()
    {
        if (solutionSlots.Count == 0)
        {
            return 1;
        }
        else
        {
            return solutionSlots.Count;
        }
    }
}
