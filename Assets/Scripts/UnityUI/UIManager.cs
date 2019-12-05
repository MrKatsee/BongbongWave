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
        //버튼과 창을 불러온다
        List<IData> datas = new List<IData>();
        if (type == DataType.PLAYLIST)
        {
            //List<Playlist> -> List<IData> 암시적 변환이 불가능해서 하나씩 넣어줘야 함 
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
            //마찬가지로 일일이 넣어줌
            var musics = selectedPlaylist.GetDatas();

            foreach (var music in musics)
            {
                datas.Add(music);
            }

            curDataType = DataType.MUSIC;

            plusButton.SetActive(false);

            musicSelectCanvasText.text = selectedPlaylist.GetName();
        }

        //이미 존재하는 버튼을 지움
        foreach (var b in buttons)
        {
            b.OnDestroyCallBack();
        }

        buttons = new List<ButtonObject>();

        //버튼 생성
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

    //선택된 버튼
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

    //ButtonObject.OnButtonClicked()에서 자신 Index와 함께 넘겨줌
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

    //CurBtn이 더블클릭되었을 때
    public void OnButtonDoubleClicked()
    {
        switch (curDataType)
        {
            case DataType.NONE:
                break;
            case DataType.PLAYLIST:
                //해당 Playlist의 Music List를 ButtonObject로 불러옴
                int playlistIndex = CurBtn.GetButtonData().GetIndex();
                selectedPlaylist = DataManager.Instance.GetPlaylist(playlistIndex);

                LoadButtons(DataType.MUSIC);
                break;
            case DataType.MUSIC:
                //Controller에 해당 Music을 선택했다고 알림
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
