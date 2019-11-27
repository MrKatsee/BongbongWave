using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using B83.Win32;
using System;

public class Controller : MonoBehaviour
{
    private Playlist curPlaylist = null;
    public Playlist CurPlaylist
    {
        get { return curPlaylist; }

        set
        {
            curPlaylist = value;

            curPlaylist.Init();

            MusicStart();
        }
    }

    private bool isMusicOn = false;

    private AudioSource audioSource;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (isMusicOn)
        {
            if (!audioSource.isPlaying)
                MusicSkipToNext();
        }
    }

    private void Init()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnStartButtonClicked()
    {
        if (isMusicOn) MusicStop();
        else MusicStart();
    }

    public void MusicStart()
    {
        //curPlaylist의 Header에 저장된 시점부터 재생한다.
        AudioClip clip = CurPlaylist.GetClip();
        if (clip == null) return;        

        audioSource.clip = clip;
        audioSource.Play();

        float playtime = CurPlaylist.playtime;
        audioSource.time = playtime;

        isMusicOn = true;
    }

    public void MusicStop()
    {
        //재생 중인 파일을 멈추고 Header에 정보를 저장한다.
        float playtime = audioSource.time;
        CurPlaylist.playtime = playtime;

        audioSource.Stop();

        isMusicOn = false;
    }

    public void MusicSkipToNext()
    {
        //넘길 수 있으면 Start
        //없으면 Stop
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
        if (CurPlaylist.SkipToPrev())
            MusicStart();
        else
        {
            MusicStart();
        }
    }

    public void OnOptionButtonClicked()
    {

    }

    private void AddMusic(string filePath)
    {
        //curPlaylist가 있으면, 거기에 Add
        //없으면, 새로운 Playlist를 만들어줌
        Music music = Music.Init_Music(filePath);

        if (CurPlaylist != null)
        {
            CurPlaylist.AddMusic(music);
        }
        else
        {
            Playlist playlist = Playlist.Init_Playlist();

            CurPlaylist = playlist;
            CurPlaylist.AddMusic(music);

            MusicStart();
        }

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
