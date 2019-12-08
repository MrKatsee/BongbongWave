using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 비트에 맞춰 색을 바꿈
public class VisualizeColor : MonoBehaviour
{
    [Header("Behavior Setting")]
    public Transform _target;
    private MeshRenderer _meshRenderer;
    public Material material;
    private Material myMaterial;
    public Color color;
    public string _colorProperty;

    private float paintRatio=1;
    [Range(0.8f, 0.99f)]
    public float _fallbackFactor;
    [Range(1,4)]
    public float _paintMultiplier = 1;

    [Header("Beat Setting")]
    [Range(0, 3)]
    public int _onFullBeat;
    [Range(0, 7)]
    public int[] _onBeatHalfQuad;
    private int _beatCountFull;

    // Start is called before the first frame update
    void Start()
    {
        if(_target != null)
        {
            _meshRenderer = _target.GetComponent<MeshRenderer>();
        }
        else
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }
        paintRatio = 0;
        myMaterial = new Material(material);
        _meshRenderer.material = myMaterial;
        _paintMultiplier = 1;
        _onBeatHalfQuad = new int[1] { System.Convert.ToInt32(transform.name) };
    }

    // Update is called once per frame
    void Update()
    {
        if(paintRatio > 0)
        {
            paintRatio *= _fallbackFactor;
        }
        else
        {
            paintRatio = 0;
        }

        CheckBeat();
        //materialInstance.SetColor("color", color*strength*_colorMultiplier);
        myMaterial.color =  color* paintRatio * _paintMultiplier;
    }

    void Paint()
    {
        paintRatio = 1;
    }

    void CheckBeat()
    {
        // beat 가져오기 
        _beatCountFull = BPMManager._beatCountFull % 4;
        //_beatCountFull = (_beatCountFull + 1) % 4;
        for (int i = 0; i < _onBeatHalfQuad.Length; i++)
        {
            // beat detection 종류
            if(BPMManager._beatHalfQuad && _beatCountFull == _onFullBeat && BPMManager._beatCountHalfQuad % 8 == _onBeatHalfQuad[i])
                Paint();
        }
    }
}
