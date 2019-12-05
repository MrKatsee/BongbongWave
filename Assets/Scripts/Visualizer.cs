using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Visualizer : MonoBehaviour
{
    public float minHeight = 0.2f;
    public float maxHeight = 6.0f;
    public float updateSensetivity = 0.5f;
    public Color VisualizerColor = Color.cyan;
    [Space(15)]
    public AudioClip audioClip;
    public bool loop = true;
    public bool enable = false;
    [Space(15), Range(64, 8192)]
    public int visualierSimples = 64;

    protected AudioSource m_audioSource;

    // Start is called before the first frame update
    public void Start()
    {
        if (enable = !audioClip) return;
        m_audioSource = new GameObject("AudioSource").AddComponent<AudioSource>();
        m_audioSource.loop = loop;
        m_audioSource.clip = audioClip;
        m_audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
