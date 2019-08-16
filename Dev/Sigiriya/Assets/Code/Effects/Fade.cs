using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    [SerializeField] private float fadeLength = 1.0f;
    [SerializeField] private CanvasGroup fadeImage = null;

    //Fades the image to 100% opacity
    private IEnumerator fadeIn;

    //Fades the image to 0% opacity
    private IEnumerator fadeOut;

    private float fadeCounter;
    private float fadeAlpha;

    public void FadeIn(string fadeID)
    {
        if (fadeIn == null && fadeOut == null)
        {
            fadeIn = FadeInCoroutine(fadeID, fadeImage, fadeLength);
            StartCoroutine(fadeIn);
        }
        else
        {
            //Debug.Log("Could not fade in, already fading");
        }
    }

    public void FadeOut(string fadeID)
    {
        if (fadeIn == null && fadeOut == null)
        {
            fadeOut = FadeOutCoroutine(fadeID, fadeImage, fadeLength);
            StartCoroutine(fadeOut);
        }
        else
        {
            //Debug.Log("Could not fade out, already fading");
        }
    }

    public void FadeInNow()
    {
        fadeImage.alpha = 1.0f;
    }

    public void FadeOutNow()
    {
        fadeImage.alpha = 0.0f;
    }

    private IEnumerator FadeInCoroutine(string fadeID, CanvasGroup image, float fadeLength)
    {
        fadeCounter = 0.0f;
        fadeAlpha = 0.0f;

        while (fadeCounter < fadeLength)
        {
            fadeCounter += Time.deltaTime;
            fadeAlpha = (fadeCounter / fadeLength);
            image.alpha = fadeAlpha;
            yield return null;
        }

        fadeIn = null;

        //Debug.Log("Fade in ended");
        EventAnnouncer.OnEndFadeIn?.Invoke(fadeID);
    }

    private IEnumerator FadeOutCoroutine(string fadeID, CanvasGroup image, float fadeLength)
    {
        image.alpha = 0.0f;
        fadeCounter = 0.0f;

        while (fadeCounter < fadeLength)
        {
            fadeCounter += Time.deltaTime;
            fadeAlpha = 1.0f - (fadeCounter / fadeLength);
            image.alpha = fadeAlpha;
            yield return null;
        }

        fadeOut = null;

        //Debug.Log("Fade out ended");
        EventAnnouncer.OnEndFadeOut?.Invoke(fadeID);
    }
}
