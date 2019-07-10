using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Author: Andrew Rimpici
//Manages the text and how it displays to the player.
public class TextController : ManagerBase<TextController>
{
    private char[] currentText;

    private string outputText;
    private int currentIndex;

    [SerializeField] private float fillSpeed = 10;
    [SerializeField] private float letterBucketSize = 10;

    private float fillCount;

    private Coroutine displayText = null;

    private void OnEnable()
    {
        //Listen for text to display fancily
        EventAnnouncer.OnDialogueUpdate += UpdateText;
    }

    private void OnDisable()
    {
        //Unlisten for text to display fancily
        EventAnnouncer.OnDialogueUpdate -= UpdateText;
    }

    private void UpdateText(TextMeshProUGUI display, string text)
    {
        currentText = text.ToCharArray();

        if (displayText != null)
        {
            StopCoroutine(displayText);
        }

        displayText = StartCoroutine(DisplayText(display));
    }

    //Speed
    //Speed threshold

    private IEnumerator DisplayText(TextMeshProUGUI display)
    {
        outputText = "";
        currentIndex = 0;
        fillCount = 0.0f;

        //While we still have text to fill...
        while (currentIndex < currentText.Length)
        {
            fillCount += fillSpeed;

            while (fillCount >= letterBucketSize)
            {
                fillCount -= letterBucketSize;
                outputText += currentText[currentIndex++];
                display.text = outputText;

                if (currentIndex >= currentText.Length)
                {
                    break;
                }
            }

            Debug.Log("HELLO");
            yield return null;
        }
    }
}
