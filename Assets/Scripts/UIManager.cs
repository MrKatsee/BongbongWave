using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //유니티 UI 관련된 코-드

    private static UIManager instance = null;
    public static UIManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    //유니티 엔진 Inspector 창을 통해 할당
    public Sprite startButtonSprite;
    public Sprite stopButtonSprite;
    public Sprite playlistOnSprite;
    public Sprite playlistOffSprtie;

    public Image startButton;
    public Image playlistButton;

    public GameObject playlistCanvas;

    public void SetUIOnStartButtonClicked(bool isStarting)
    {
        if (isStarting) startButton.sprite = stopButtonSprite;
        else startButton.sprite = startButtonSprite;
    }

    public void SetUIOnPlaylistButtonClicked(bool isPlaylistSettingOn)
    {
        if (isPlaylistSettingOn) playlistButton.sprite = playlistOnSprite;
        else playlistButton.sprite = playlistOffSprtie;

        playlistCanvas.SetActive(isPlaylistSettingOn);
    }
}
