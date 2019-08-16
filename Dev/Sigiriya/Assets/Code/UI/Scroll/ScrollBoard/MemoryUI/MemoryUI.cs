using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MemoryUI : ScrollBoardItemUIBase
{
    [SerializeField] private Image memoryImage = null;
    [SerializeField] private TextMeshProUGUI description = null;

    public override bool InitBoardItemUI(ScriptableObject memoryData, Sprite background)
    {
        if (!(memoryData is Memory))
        {
            return false;
        }

        Memory memory = (memoryData as Memory);

        itemBackgroundImage.sprite = background;
        parentData = memoryData;

        memoryImage.sprite = memory.memoryImage;
        description.text = memory.memoryText;

        return true;
    }
}
