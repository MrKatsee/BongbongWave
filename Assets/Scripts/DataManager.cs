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
        playlists = new List<Playlist>();
        LoadPlaylists();
    }

    public void AddPlaylist(Playlist playlist)
    {
        playlists.Add(playlist);
    }

    public Playlist GetPlaylist(int i)
    {
        return playlists[i];
    }

    public void SavePlaylists()
    {

    }

    public void LoadPlaylists()
    {

    }
}
