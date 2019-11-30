﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CurrentDataType
{
    NONE, PLAYLIST, MUSIC
}

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

        curDataType = CurrentDataType.NONE;
        buttons = new List<ButtonObject>();
    }

    //유니티 엔진 Inspector 창을 통해 할당
    public Sprite startButtonSprite;
    public Sprite stopButtonSprite;
    public Sprite playlistOnSprite;
    public Sprite playlistOffSprtie;

    public Image startButton;
    public Image playlistButton;

    public GameObject playlistCanvas;
    public GameObject btnPrefab;
    public Transform btnParentTransform;

    public void SetUIOnStartButtonClicked(bool isStarting)
    {
        if (isStarting) startButton.sprite = stopButtonSprite;
        else startButton.sprite = startButtonSprite;
    }

    public void SetUIOnPlaylistButtonClicked(bool isPlaylistSettingOn)
    {
        playlistCanvas.SetActive(isPlaylistSettingOn);

        if (isPlaylistSettingOn)
        {
            playlistButton.sprite = playlistOnSprite;
            LoadButtons();
        }
        else
        {
            playlistButton.sprite = playlistOffSprtie;
            curDataType = CurrentDataType.NONE;
        }
    }

    private CurrentDataType curDataType;

    private void LoadButtons()
    {
        List<IData> datas = new List<IData>();
        if (curDataType == CurrentDataType.NONE)
        {
            var playlists = DataManager.Instance.GetPlaylistAll();

            foreach (var playlist in playlists)
            {
                datas.Add(playlist);
            }

            curDataType = CurrentDataType.PLAYLIST;
        }
        else if (curDataType == CurrentDataType.PLAYLIST)
        {
            datas = CurBtn.GetButtonData().GetDatas();

            curDataType = CurrentDataType.MUSIC;
        }

        foreach(var b in buttons)
        {
            b.OnDestroyCallBack();
        }

        buttons = new List<ButtonObject>();

        foreach (var d in datas)
        {
            ButtonObject btn = Instantiate(btnPrefab, btnParentTransform).GetComponent<ButtonObject>();

            btn.Init(d);
            buttons.Add(btn);
        }

        CurBtn = null;
        IsSingleClicked = false;
    }

    private List<ButtonObject> buttons;

    private ButtonObject curBtn = null;
    private ButtonObject CurBtn
    {
        get { return curBtn; }
        set
        {
            if (curBtn != null)
                curBtn.IsBtnClicked = false;

            curBtn = value;

            if (curBtn != null)
                curBtn.IsBtnClicked = true;
        }
    }
    public void OnButtonsClicked(int index)
    {
        if (CurBtn == buttons[index])
        {
            if (IsSingleClicked) OnButtonDoubleClicked();
            else IsSingleClicked = true;
        }
        else
        {
            CurBtn = buttons[index];
            IsSingleClicked = true;
        }
    }

    public void OnButtonDoubleClicked()
    {
        switch (curDataType)
        {
            case CurrentDataType.NONE:
                break;
            case CurrentDataType.PLAYLIST:
                LoadButtons();
                break;
            case CurrentDataType.MUSIC:
                curDataType = CurrentDataType.NONE;
                break;
        }
    }

    private bool isSingleClicked = false;
    private bool IsSingleClicked
    {
        get { return isSingleClicked; }
        set
        {
            isSingleClicked = value;
            if (isSingleClicked)
                StartCoroutine(DoubleClickTimer());
        }
    }
    IEnumerator DoubleClickTimer()
    {
        yield return new WaitForSeconds(0.25f);

        isSingleClicked = false;
    }
}
