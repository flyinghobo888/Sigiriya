using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Manages the text and how it displays to the player.
public class TextController : ManagerBase<TextController>
{
    private char[] currentText;

    private string outputText;
    private int currentIndex;

    public bool IsFinished { get; private set; }

    [SerializeField] private float fillSpeed = 10;
    [SerializeField] private float letterBucketSize = 10;

    private float fillCount;

    private Coroutine displayText = null;

    private void OnEnable()
    {
        //Listen for text to display fancily
        EventAnnouncer.OnDialogueUpdate += UpdateText;
        EventAnnouncer.OnDialogueRequestFinish += FinishText;
    }

    private void OnDisable()
    {
        //Unlisten for text to display fancily
        EventAnnouncer.OnDialogueUpdate -= UpdateText;
        EventAnnouncer.OnDialogueRequestFinish -= FinishText;
    }

    private void UpdateText(TextMeshProUGUI display, string text)
    {
        currentText = text.ToCharArray();

        StopText();
        displayText = StartCoroutine(DisplayText(display));
    }

    //Speed
    //Speed threshold

    private IEnumerator DisplayText(TextMeshProUGUI display)
    {
        outputText = "";
        currentIndex = 0;
        fillCount = 0.0f;
        IsFinished = false;

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
                    IsFinished = true;
                    break;
                }
            }

            yield return null;
        }
    }

    private void FinishText(TextMeshProUGUI display)
    {
        display.text = currentText.ArrayToString();
        StopText();
    }

    private void StopText()
    {
        if (displayText != null)
        {
            StopCoroutine(displayText);
            IsFinished = true;
        }
    }
}
