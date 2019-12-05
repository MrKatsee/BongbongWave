using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineVisualizer : Visualizer
{
    VisualizerObject[] VisualizerObjects;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        VisualizerObjects = GetComponentsInChildren<VisualizerObject>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (enable) return;
        float[] spectrumData = new float[64];
        spectrumData = m_audioSource.GetSpectrumData(visualierSimples, 0, FFTWindow.Rectangular);

        for (int i = 0; i < VisualizerObjects.Length; i++)
        {
            /*
            Vector2 newSize = VisualizerObjects[i].GetComponent<RectTransform>().rect.size;
            newSize.y = Mathf.Lerp(newSize.y, minHeight + (spectrumData[i] * (maxHeight - minHeight) * 5.0f), updateSensetivity);
            newSize.y = newSize.y > 400 ? 400 : newSize.y;
            VisualizerObjects[i].GetComponent<RectTransform>().sizeDelta = newSize;
            VisualizerObjects[i].GetComponent<Image>().color = VisualizerColor;
            */
            Vector2 newSize = VisualizerObjects[i].GetComponent<Transform>().localScale;
            newSize.y = Mathf.Lerp(newSize.y, minHeight + (spectrumData[i] * (maxHeight - minHeight) * 5.0f), updateSensetivity);
            newSize.y = newSize.y > 20 ? 20 : newSize.y;
            VisualizerObjects[i].GetComponent<Transform>().localScale = newSize;
            VisualizerObjects[i].GetComponent<Renderer>().material.color = VisualizerColor;
        }
    }
}
