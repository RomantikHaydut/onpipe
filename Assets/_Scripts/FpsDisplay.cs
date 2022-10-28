using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FpsDisplay : MonoBehaviour
{
    private TMP_Text fpsText;

    private void Start()
    {
        fpsText = GetComponent<TMP_Text>();
        StartCoroutine(ShowFps());
    }

    IEnumerator ShowFps()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(1f);
            int fps = (int)(1 / Time.unscaledDeltaTime);
            fpsText.text = " Fps : " + fps.ToString();

        }
    }
}
