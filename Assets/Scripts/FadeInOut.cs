using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    [SerializeField] private TextMeshPro tmpText;
    [SerializeField] private float fadeDuration = 0.175f;

    private Color faceStartColor;
    private Color faceEndColor;


    private Color outlineStartColor;
    private Color outlineEndColor;

    private void Start()
    {


        // Get initial face color (white-ish like your screenshot)
        faceStartColor = tmpText.faceColor;
        faceStartColor.a = 0f; // invisible
        faceEndColor = tmpText.faceColor; // fully visible

        tmpText.faceColor = faceStartColor;

        outlineStartColor = tmpText.faceColor;
        outlineStartColor.a = 0f; // invisible
        outlineEndColor = tmpText.outlineColor; // fully visible

        tmpText.outlineColor = outlineStartColor;

    }

    public void StartTransition()
    {
        StartCoroutine(FadeSequence());
    }
    private IEnumerator FadeSequence()
    {
        // Fade in
        float t = 0f;
        while (t < 0.1)
        {
            t += Time.deltaTime;
            tmpText.faceColor = Color.Lerp(faceStartColor, faceEndColor, t / 0.1f);
            tmpText.outlineColor = Color.Lerp(outlineStartColor, outlineEndColor, t / 0.1f);
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);

        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            tmpText.faceColor = Color.Lerp(faceEndColor, faceStartColor, t / fadeDuration);
            tmpText.outlineColor = Color.Lerp(outlineEndColor, outlineStartColor, t / fadeDuration);
            yield return null;
        }
        gameObject.SetActive(false);
        tmpText.faceColor = faceStartColor;
        tmpText.outlineColor = outlineStartColor;
    }
}