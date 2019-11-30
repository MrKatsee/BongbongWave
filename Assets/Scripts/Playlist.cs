using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playlist : MonoBehaviour, IData
{
    public static Playlist Init_Playlist()
    {
        //MonoBehaviour을 상속한 클래스의 경우, GameObject에 Component로 존재해야한다.
        Playlist playlistObject = new GameObject("Playlist").AddComponent<Playlist>();

        playlistObject.Init();

        DataManager.Instance.AddPlaylist(playlistObject);

        return playlistObject;
    }

    private List<Music> musics;
    private int curMusicIndex = 0;
    private int maxCount;
    private int minCount;

    public float playtime = 0f;
    private string playlistName;
    private int playlistIndex;

    public void Init()
    {
        if (musics == null) musics = new List<Music>();

        maxCount = musics.Count;
        minCount = 0;
        curMusicIndex = 0;
        playtime = 0f;
    }

    public bool SkipToNext()
    {
        //가능하면 true 불가능하면 false
        playtime = 0f;

        if (++curMusicIndex >= maxCount)
        {
            curMusicIndex--;
            return false;
        }


        return true;
    }

    public bool SkipToPrev()
    {
        //가능하면 true 불가능하면 false
        playtime = 0f;

        if (--curMusicIndex < minCount)
        {
            curMusicIndex++;
            return false;
        }

        return true;
    }

    public void AddMusic(string filePath)
    {
        Music music = Music.Init_Music(musics.Count, filePath);
        musics.Add(music);
        maxCount = musics.Count;
    }

    public AudioClip GetClip()
    {
        if (musics.Count == 0) return null;

        AudioClip clip = musics[curMusicIndex].clip;

        return clip;
    }

    public void SetName(string name)
    {
        playlistName = name;
    }

    public string GetName()
    {
        return playlistName;
    }

    public void SetIndex(int index)
    {
        playlistIndex = index;
    }

    public int GetIndex()
    {
        return playlistIndex;
    }

    public List<IData> GetDatas()
    {
        var datas = new List<IData>();

        foreach (var m in musics)
        {
            datas.Add(m);
        }

        return datas;
    }
}
