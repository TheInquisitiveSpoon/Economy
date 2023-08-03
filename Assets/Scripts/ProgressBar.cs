using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Script to update a progress bar type UI.
public class ProgressBar : MonoBehaviour
{
    public RectTransform Fill;
    int MaxNumber;

    //  Shows the progress bar, resets the fill scale so it will not show, and determines
    //  the maximum value of the progression.
    public void BeginProgress(int maxNumber)
    {
        gameObject.SetActive(true);
        MaxNumber = maxNumber;
        Fill.transform.localScale.Set(0.0f, 1.0f, 1.0f);
    }

    //  Scales the fill bar using a percentage in the range of 0-1 of the current number over the maximum
    public void UpdateValue(int currentNumber)
    {
        float fillXScale = 1 - ((float)currentNumber / (float)MaxNumber);
        Fill.transform.localScale = new Vector3(fillXScale, 1.0f, 1.0f);
    }

    //  Hides and resets the progress bar.
    public void ResetProgress()
    {
        Fill.transform.localScale = new Vector3(0.0f, 1.0f, 1.0f);
        gameObject.SetActive(false);
    }
}
