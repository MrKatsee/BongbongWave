using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizeSize : MonoBehaviour
{
    [Header("Behavior Setting")]
    public Transform _target;
    private float _currentSize;
    public float _growSize, _shrinkSize;
    [Range(0.8f,0.99f)]
    public float _shrinkFactor;

    [Header("Beat Setting")]
    [Range(0, 3)]
    public int _onFullBeat;
    [Range(0, 7)]
    public int[] _onBeatHalfQuad;
    private int _beatCountFull;

    // Start is called before the first frame update
    void Start()
    {
        _target = transform;

        if (_target == null)
            _target = this.transform;

        _currentSize = _shrinkSize;

        _onBeatHalfQuad = new int[1] { System.Convert.ToInt32(transform.name)};
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentSize > _shrinkSize)
        {
            _currentSize *= _shrinkFactor;
        }
        else
        {
            _currentSize = _shrinkSize;
        }
        CheckBeat();
        _target.localScale = new Vector3(_target.localScale.x, _currentSize, _target.localScale.z);

    }

    void SizeUp()
    {
        _currentSize = _growSize;
    }

    void CheckBeat()
    {
        // beat 가져오기 
        _beatCountFull = BPMManager._beatCountFull % 4;
        //_beatCountFull = (_beatCountFull + 1) % 4;
        for (int i = 0; i < _onBeatHalfQuad.Length; i++)
        {
            // beat detection 종류
            if (BPMManager._beatHalfQuad && _beatCountFull == _onFullBeat && BPMManager._beatCountHalfQuad % 8 == _onBeatHalfQuad[i])
                SizeUp();
        }
    }
}

