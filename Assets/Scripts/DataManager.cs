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
        string playlistName = $"Playlist_{playlists.Count + 1}";

        playlist.SetName(playlistName);
        playlist.SetIndex(playlists.Count);

        playlists.Add(playlist);
    }

    public Playlist GetPlaylist(int i)
    {
        return playlists[i];
    }

    public List<Playlist> GetPlaylistAll()
    {
        return playlists;
    }

    public void SavePlaylists()
    {

    }

    public void LoadPlaylists()
    {

    }
}
