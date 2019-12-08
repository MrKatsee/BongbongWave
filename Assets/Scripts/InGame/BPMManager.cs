
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BPMManager : MonoBehaviour
{
    public static BPMManager instance;
    public static bool _beatHalfQuad;
    public static bool _beatFull;
    public static int _beatCountFull;
    public static int _beatCountHalfQuad;

    public float _bpm;

    private float _beatInterval;
    private float _beatTimer;
    private float _beatIntervalHalfQuad;
    private float _beatTimerHalfQuad;

    private AudioSource _audioSource;
    public AudioSource AudioSource
    {
        get
        {
            if(_audioSource != null)
            {
                return _audioSource;
            }
            else
            {
                _audioSource = GameObject.Find("UICanvas").GetComponent<AudioSource>();
                return _audioSource;
            }
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        BeatDetection();
    }

    void BeatDetection()
    {
        //full beat count
        _beatFull = false;
        _beatInterval = 60/_bpm;
        _beatTimer += Time.deltaTime;
        if (_beatTimer >= _beatInterval)
        {
            _beatTimer -= _beatInterval;
            _beatFull = true;
            _beatCountFull++;
            Debug.Log("Full");
        }

        // divide beat count
        _beatHalfQuad = false;
        _beatIntervalHalfQuad = _beatInterval / 8;
        _beatTimerHalfQuad += Time.deltaTime;
        if (_beatTimerHalfQuad >= _beatIntervalHalfQuad)
        {
            _beatTimerHalfQuad -= _beatIntervalHalfQuad;
            _beatHalfQuad = true;
            _beatCountHalfQuad++;
            Debug.Log("Divided by 8");
        }

    }

    public void UpdateBPM()
    {
       
        _bpm = UniBpmAnalyzer.AnalyzeBpm(AudioSource.clip);
        Debug.Log(UniBpmAnalyzer.AnalyzeBpm(AudioSource.clip));
    }

    public void StopBPM()
    {
        _bpm = 0;
    }
}
