using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleVisualizer : Visualizer
{
    public GameObject arc_origin;
    public GameObject[] arcs;

    public float[] Radius = { 0, 0, 0, 0, 0, 0, 0, 0};
    public Vector3 center = new Vector3(0.0f, 0.0f, 0.0f);
    // Start is called before the first frame update
    new void Start()
    {
        arcs = new GameObject[8];

        for(int idx=0; idx < arcs.Length; idx++ )
        {
            arcs[idx] = Instantiate(arc_origin) as GameObject;
            arcs[idx].transform.RotateAround(center, new Vector3(0,0,1), idx * 45.0f);
        }

        base.Start();
    }

    // Update is called once per frame
    void Update()   
    {
        float[] spectrumData = new float[64];
        Vector3 f= new Vector3(0, 1, 0);
        spectrumData = m_audioSource.GetSpectrumData(visualierSimples, 0, FFTWindow.Rectangular);
        for (int i = 0; i < Radius.Length; i++)
        {
            Radius[i] = Mathf.Lerp(Radius[i], minHeight + (spectrumData[i] * (maxHeight - minHeight) * 5.0f), updateSensetivity);
            Radius[i] = Radius[i] > 400 ? 400 : Radius[i];
            //UnityEditor.Handles.DrawSolidArc(center, new Vector3(0, 0, 1), f, 360.0f / Radius.Length, Radius[i]);
            transform.RotateAround(f, new Vector3(0, 0, 1), 360.0f / Radius.Length);
        }
    }
}
