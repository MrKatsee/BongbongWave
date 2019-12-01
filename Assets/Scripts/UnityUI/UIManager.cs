using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DataType
{
    NONE, PLAYLIST, MUSIC
}

public class UIManager : MonoBehaviour
{
    //유니티 UI 관련된 코-드
    /*
        해야할 것
        O Playlist UI 기능 구현 (Add, Delete)
        O Music UI 디자인
        O Music UI 기능 구현 (Select, Delete, UI_Text(PlaylistName))
        O Music -> Playlist 버튼 구현
        O 저장 / 불러오기
        - Controller에 현재 재생 중인 Music Name 출력

        애매한 것
        - Music -> Playlist? Playlist -> Music?
        - UIManager 이름? 대충 MusicSelectManager 정도가 더 나을듯

        이슈
        - 넣자마자 재생이 안됨
    */

    private static UIManager instance = null;
    public static UIManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null) instance = this;

        curDataType = DataType.NONE;
        buttons = new List<ButtonObject>();
    }

    //유니티 엔진 Inspector 창을 통해 할당
    public GameObject btnPrefab;
    public Transform btnParentTransform;
    public GameObject plusButton;
    public Text musicSelectCanvasText;

    public DataType curDataType;

    private Playlist selectedPlaylist = null;

    private string playlistsText = "Playlists";

    public void LoadButtons(DataType type)
    {
        List<IData> datas = new List<IData>();
        if (type == DataType.PLAYLIST)
        {
            var playlists = DataManager.Instance.GetPlaylistAll();

            foreach (var playlist in playlists)
            {
                datas.Add(playlist);
            }

            curDataType = DataType.PLAYLIST;

            plusButton.SetActive(true);

            musicSelectCanvasText.text = playlistsText;
        }
        else if (type == DataType.MUSIC)
        {
            var musics = selectedPlaylist.GetDatas();

            foreach (var music in musics)
            {
                datas.Add(music);
            }

            curDataType = DataType.MUSIC;

            plusButton.SetActive(false);

            musicSelectCanvasText.text = selectedPlaylist.GetName();
        }

        foreach (var b in buttons)
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
            case DataType.NONE:
                break;
            case DataType.PLAYLIST:
                int playlistIndex = CurBtn.GetButtonData().GetIndex();
                selectedPlaylist = DataManager.Instance.GetPlaylist(playlistIndex);

                LoadButtons(DataType.MUSIC);
                break;
            case DataType.MUSIC:
                int musicIndex = CurBtn.GetButtonData().GetIndex();

                Controller.Instance.CurPlaylist = selectedPlaylist;
                Controller.Instance.SetUIOnPlaylistButtonClicked(false);
                Controller.Instance.MusicSkipByIndex(musicIndex);

                selectedPlaylist = null;
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

    public void OnPlaylistPlusButtonClicked()
    {
        Playlist.Init_Playlist();
        curDataType = DataType.NONE;
        LoadButtons(DataType.PLAYLIST);
    }

    public void OnDeleteButtonClicked()
    {
        if (CurBtn == null)
            return;

        if (curDataType == DataType.PLAYLIST)
            PlaylistDelete();
        else if (curDataType == DataType.MUSIC)
            MusicDelete();
    }

    private void PlaylistDelete()
    {
        int index = CurBtn.GetButtonData().GetIndex();
        DataManager.Instance.DeletePlaylist(index);
        LoadButtons(DataType.PLAYLIST);
    }

    private void MusicDelete()
    {
        int index = CurBtn.GetButtonData().GetIndex();
        selectedPlaylist.DeleteMusic(index);
        LoadButtons(DataType.MUSIC);
    }

    public void OnBackButtonClicked()
    {
        switch (curDataType)
        {
            case DataType.NONE:
                break;
            case DataType.PLAYLIST:
                Controller.Instance.SetUIOnPlaylistButtonClicked(false);
                break;
            case DataType.MUSIC:
                LoadButtons(DataType.PLAYLIST);
                break;
        }
    }


    public void AddMusicInSelectedPlaylist(string filePath)
    {
        selectedPlaylist.AddMusic(filePath);
        LoadButtons(DataType.MUSIC);
    }
}
