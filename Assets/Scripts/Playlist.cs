using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playlist : MonoBehaviour, IData
{
    //플레이리스트를 생성해서 Database로 넘겨줌
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

    public bool SkipByIndex(int index)
    {
        if (index >= maxCount)
        {
            return false;
        }

        playtime = 0f;
        curMusicIndex = index;

        return true;
    }

    public void AddMusic(string filePath)
    {
        Music music = Music.Init_Music(musics.Count, filePath);
        musics.Add(music);
        maxCount = musics.Count;

        DataManager.Instance.SavePlaylists();
    }

    public void DeleteMusic(int index)
    {
        if (index == curMusicIndex) Controller.Instance.MusicSkipToNext();

        musics.Remove(musics[index]);
        maxCount = musics.Count;

        if (index < curMusicIndex) curMusicIndex--;

        DataManager.Instance.SavePlaylists();
    }

    public AudioClip GetClip()
    {
        if (musics.Count == 0) return null;
        else if (curMusicIndex >= maxCount) return null;

        AudioClip clip = musics[curMusicIndex].clip;

        return clip;
    }

    public string GetCurrentMusicName()
    {
        if (musics.Count == 0) return "Empty";
        return musics[curMusicIndex].GetName();
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

    public List<Music> GetDatas()
    {
        return musics;
    }

    public override string ToString()
    {
        //구분자는 '^'
        //"PlaylisIndex^PlaylistName^musicPath1&musicPath2&...&musicPathN"
        string playlistInfo = $"{playlistIndex}^{playlistName}^";

        int i = 0;
        foreach (var music in musics)
        {
            playlistInfo = $"{playlistInfo}{music.ToString()}";

            if (++i < musics.Count)
                playlistInfo = $"{playlistInfo}&";
        }

        return playlistInfo;
    }

    public void LoadPlaylist(string data)
    {
        string[] splitedData = data.Split('^');

        SetIndex(int.Parse(splitedData[0]));
        SetName(splitedData[1]);

        string[] musicPaths = splitedData[2].Split('&');

        if (splitedData[2] == string.Empty) return;

        foreach(var path in musicPaths)
        {
            AddMusic(path);
        }
    }
}
