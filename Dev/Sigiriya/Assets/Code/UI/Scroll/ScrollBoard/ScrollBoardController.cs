using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollBoardController : ManagerBase<ScrollBoardController>
{
    [SerializeField] private RectTransform journalContainer = null;
    [SerializeField] private GameObject backgroundBlurContainer = null;
    [Header("The scroll content container")]
    [SerializeField] private RectTransform contentContainer = null;
    [Header("The top image of the scroll")]
    [SerializeField] private Sprite topBoardBackground = null;
    [Space]
    [Header("The other image for the scroll")]
    [SerializeField] private Sprite middleBoardBackground = null;
    [Space]
    [Header("The task ui prefabs")]
    [SerializeField] private RootTaskUI rootTaskUIPrefab = null;
    [SerializeField] private GoalTaskUI goalTaskUIPrefab = null;
    [SerializeField] private SolutionsTaskUI solutionTaskUIPrefab = null;
    [Space]
    [Header("The memory ui prefabs")]
    //[SerializeField] private RootMemoryUI rootMemoryUIPrefab = null;

    private List<List<ScrollBoardItemUIBase>> activeElementsInList = new List<List<ScrollBoardItemUIBase>>();
    private List<ScrollBoardItemUIBase> taskElements = new List<ScrollBoardItemUIBase>();
    private List<ScrollBoardItemUIBase> memoryElements = new List<ScrollBoardItemUIBase>();

    public void Awake()
    {
        activeElementsInList.Clear();
        activeElementsInList.Add(taskElements);
        activeElementsInList.Add(memoryElements);
    }

    public void ShowJournal(bool show)
    {
        backgroundBlurContainer.SetActive(show);
        journalContainer.gameObject.SetActive(show);
    }

    public void ResetUIList()
    {
        foreach (List<ScrollBoardItemUIBase> list in activeElementsInList)
        {
            for (int i = 0; i < list.Count;)
            {
                Destroy(list[i]);
            }
        }
    }

    public void ResetTaskUIList()
    {
        for (int i = 0; i < taskElements.Count;)
        {
            Destroy(taskElements[i]);
        }
    }

    //Ideally I would want Task and Memory to have a parent scriptable object, 
    //but I don't know if restructuring that code will lose all the data in editor
    public void AddUIItem(ScriptableObject data)
    {
        if (data is Task)
        {
            AddTask(data as Task);
        }
        else if (data is Memory)
        {
            AddMemory(data as Memory);
        }
    }

    private int solutionCount;
    private bool shouldInstantiate;
    private List<Task> solutionTasks = new List<Task>();
    private Task prevGoalTask;
    private SolutionsTaskUI currentSolutionsTask = null;

    //Create the new ui pieces for the task!
    private void AddTask(Task task)
    {
        Sprite backgroundSprite = GetBackgroundSprite();
        ScrollBoardItemUIBase boardPrefab;

        switch (task.TaskType)
        {
            case EnumTaskType.ROOT:
                boardPrefab = rootTaskUIPrefab;
                shouldInstantiate = true;
                solutionCount = 0;
                break;
            case EnumTaskType.GOAL:
                boardPrefab = goalTaskUIPrefab;
                prevGoalTask = task;
                shouldInstantiate = true;
                solutionCount = 0;
                break;
            case EnumTaskType.SOLUTION:
            default:
                if (solutionCount == 0)
                {
                    shouldInstantiate = true;
                }
                else
                {
                    shouldInstantiate = solutionCount >= solutionTaskUIPrefab.GetSlotCount();
                    if (shouldInstantiate)
                    {
                        solutionCount = 0;
                        solutionTasks.Clear();
                    }
                }

                boardPrefab = solutionTaskUIPrefab;
                solutionTasks.Add(task);
                ++solutionCount;

                if (currentSolutionsTask)
                {
                    currentSolutionsTask.InitSlots(solutionTasks);
                }
                break;
        }

        if (shouldInstantiate)
        {
            ScrollBoardItemUIBase uiElement = Instantiate(boardPrefab, contentContainer, false);

            if (uiElement is SolutionsTaskUI)
            {
                currentSolutionsTask = uiElement as SolutionsTaskUI;
                currentSolutionsTask.InitSlots(solutionTasks);
            }
            else
            {
                currentSolutionsTask = null;
            }

            if (uiElement.InitBoardItemUI(task, backgroundSprite))
            {
                //Add task element to the list
                registerTaskUIElement(uiElement);
            }

            shouldInstantiate = false;
        }

        foreach (Task subTask in task.subTasks)
        {
            AddTask(subTask);
        }
    }

    private void AddMemory(Memory memory)
    {
        //DO THIS IN THE FUTURE
    }

    private Sprite GetBackgroundSprite()
    {
        if (GetActiveElementsCount() == 0)
        {
            return topBoardBackground;
        }
        else
        {
            return middleBoardBackground;
        }
    }

    private void registerTaskUIElement(ScrollBoardItemUIBase uiElement)
    {
        if (taskElements.Count == 0 && uiElement is RootTaskUI)
        {
            RootTaskUI rootUI = uiElement as RootTaskUI;
            rootUI.ShowTaskLogo(true);
        }

        taskElements.Add(uiElement);
        uiElement.gameObject.SetActive(true);
    }

    private void registerMemoryUIElement(ScrollBoardItemUIBase uiElement)
    {
        memoryElements.Add(uiElement);
        uiElement.gameObject.SetActive(true);
    }

    private int GetActiveElementsCount()
    {
        int count = 0;
        foreach (List<ScrollBoardItemUIBase> list in activeElementsInList)
        {
            count += list.Count;
        }

        return count;
    }
}
