using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; } = null;

    private List<Playlist> playlists;

    private void Awake()
    {
        Instance = this;

        Init();
    }

    public void Init()
    {
        LoadPlaylists();
    }

    public void Test()
    {
        PlayerPrefs.DeleteAll();
    }

    public void AddPlaylist(Playlist playlist)
    {
        string playlistName = $"{playlists.Count + 1}. Playlist {playlists.Count + 1}";

        playlist.SetName(playlistName);
        playlist.SetIndex(playlists.Count);

        playlists.Add(playlist);

        SavePlaylists();
    }

    public void DeletePlaylist(int index)
    {
        playlists.Remove(playlists[index]);

        int i = 0;
        foreach(var pl in playlists)
        {
            pl.SetIndex(i);
            pl.SetName($"{i + 1}. Playlist {i + 1}");    //Name을 수정 가능하게 짤 수도 있지만, 일부러 Index와 같은 이름을 갖도록 함
            i++;
        }

        SavePlaylists();
    }

    public Playlist GetPlaylist(int i)
    {
        if (i >= playlists.Count) return null;

        return playlists[i];
    }

    public List<Playlist> GetPlaylistAll()
    {
        return playlists;
    }

    public void SavePlaylists()
    {
        if (!isLoadComplete) return;

        PlayerPrefs.SetInt("PLAYLISTNUM", playlists.Count);

        string prefix = "PLAYLIST_";
        int i = 0;
        foreach (var pl in playlists)
        {
            string key = prefix + i;
            string value = playlists[i].ToString();
            Debug.Log($"{i}th playlist saved : {value}");

            PlayerPrefs.SetString(key, value);

            i++;
        }
        PlayerPrefs.Save();
    }

    private bool isLoadComplete = false;
    public void LoadPlaylists()
    {
        playlists = new List<Playlist>();

        int num = PlayerPrefs.GetInt("PLAYLISTNUM", 0);

        string prefix = "PLAYLIST_";
        int i = 0;
        for (i = 0; i < num; i++)
        {
            string key = prefix + i;
            string value = PlayerPrefs.GetString(key);
            Debug.Log($"{i}th playlist loaded : {value}");

            Playlist playlist = Playlist.Init_Playlist();
            playlist.LoadPlaylist(value);
        }

        isLoadComplete = true;
    }
}
