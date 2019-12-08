using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using B83.Win32;
using System;

public class Controller : MonoBehaviour
{
    private static Controller instance = null;
    public static Controller Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private Playlist curPlaylist = null;
    public Playlist CurPlaylist
    {
        get { return curPlaylist; }

        set
        {
            curPlaylist = value;

            if (curPlaylist == null)
                return;

            curPlaylist.Init();
            SetMusicDescriptionText(CurPlaylist.GetCurrentMusicName());
        }
    }

    private bool isMusicOn = false;
    private bool isPlaylistOn = false;

    private AudioSource audioSource;

    private void Start()
    {
        Init();
        //Test();
    }

    private void Update()
    {
        if (isMusicOn)
        {
            if (!audioSource.isPlaying)
                MusicSkipToNext();
        }
    }

    private void Test()
    {
        //AddMusic(@"C:\Users\duryk\Desktop\VisualWave\Assets\Resources\asYouWish.wav");
    }

    private void Init()
    {
        audioSource = GetComponent<AudioSource>();
        if (CurPlaylist == null)
            CurPlaylist = DataManager.Instance.GetPlaylist(0);
    }

    public void OnStartButtonClicked()
    {
        if (CurPlaylist == null) return;
        if (isMusicOn) MusicStop();
        else MusicStart();
    }

    public void MusicStart()
    {
        //curPlaylist의 Header에 저장된 시점부터 재생한다.
        AudioClip clip = CurPlaylist.GetClip();
        if (clip == null) return;

        audioSource.clip = clip;
        // 클립 정보가 갱신 됐으므로 BPM을 다시 계산한다
        BPMManager.instance.UpdateBPM();

        audioSource.Play();

        float playtime = CurPlaylist.playtime;
        audioSource.time = playtime;

        isMusicOn = true;
        SetUIOnStartButtonClicked(isMusicOn);
        SetMusicDescriptionText(CurPlaylist.GetCurrentMusicName());
    }

    public void MusicStop()
    {
        //재생 중인 파일을 멈추고 Header에 정보를 저장한다.
        float playtime = audioSource.time;
        CurPlaylist.playtime = playtime;

        // BMP를 0으로 해서 비주얼라이저 정지
        BPMManager.instance.StopBPM();

        audioSource.Stop();

        isMusicOn = false;
        SetUIOnStartButtonClicked(isMusicOn);
    }

    public void MusicSkipToNext()
    {
        //넘길 수 있으면 Start
        //없으면 Stop
        if (CurPlaylist == null) return;

        if (CurPlaylist.SkipToNext())
            MusicStart();
        else
        {
            MusicStop();
            CurPlaylist.Init();
        }
    }

    public void MusicSkipToPrev()
    {
        if (CurPlaylist == null) return;

        if (CurPlaylist.SkipToPrev())
            MusicStart();
        else
        {
            MusicStart();
        }
    }

    public void MusicSkipByIndex(int index)
    {
        if (CurPlaylist.SkipByIndex(index))
            MusicStart();
    }

    public void OnOptionButtonClicked()
    {

    }

    public void OnPlaylistButtonClicked()
    {
        if (isPlaylistOn) SetPlaylistOff();
        else SetPlaylistOn();
    }

    private void SetPlaylistOn()
    {
        isPlaylistOn = true;
        SetUIOnPlaylistButtonClicked(isPlaylistOn);
    }

    private void SetPlaylistOff()
    {
        isPlaylistOn = false;
        SetUIOnPlaylistButtonClicked(isPlaylistOn);
    }

    private void AddMusic(string filePath)
    {
        //curPlaylist가 있으면, 거기에 Add
        //없으면, 새로운 Playlist를 만들어줌
        Debug.Log(filePath);

        if (isPlaylistOn)
        {
            if (UIManager.Instance.curDataType == DataType.MUSIC)
            {
                UIManager.Instance.AddMusicInSelectedPlaylist(filePath);
                return;
            }
        }

        if (CurPlaylist != null)
        {
            CurPlaylist.AddMusic(filePath);
        }
        else
        {
            Playlist playlist = Playlist.Init_Playlist();

            CurPlaylist = playlist;
            CurPlaylist.AddMusic(filePath);
        }
    }

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
        playlistCanvas.SetActive(isPlaylistSettingOn);
        isPlaylistOn = isPlaylistSettingOn;

        if (isPlaylistSettingOn)
        {
            playlistButton.sprite = playlistOnSprite;
            UIManager.Instance.LoadButtons(DataType.PLAYLIST);
        }
        else
        {
            playlistButton.sprite = playlistOffSprtie;
            UIManager.Instance.curDataType = DataType.NONE;
        }
    }

    public Text musicNameText;
    private void SetMusicDescriptionText(string description)
    {
        musicNameText.text = description;
    }

    #region Drag and Drop(Reference Code)
    // important to keep the instance alive while the hook is active.
    UnityDragAndDropHook hook;
    void OnEnable()
    {
        // must be created on the main thread to get the right thread id.
        hook = new UnityDragAndDropHook();
        hook.InstallHook();
        hook.OnDroppedFiles += OnFiles;
    }
    void OnDisable()
    {
        hook.UninstallHook();
    }

    void OnFiles(List<string> aFiles, POINT aPos)
    {
        // do something with the dropped file names. aPos will contain the 
        // mouse position within the window where the files has been dropped.
        MyDebug.Log("Dropped : " + aFiles[0]);

        foreach(var f in aFiles)
        {
            AddMusic(f);
        }
    }
    #endregion
}
