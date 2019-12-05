using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

public class Music : MonoBehaviour, IData
{
    //path를 받아 Music을 생성
    public static Music Init_Music(int i, string path)
    {
        //MonoBehaviour을 상속한 클래스의 경우, GameObject에 Component로 존재해야한다.
        Music musicObject = new GameObject("Music").AddComponent<Music>();

        musicObject.Init(i, path);

        return musicObject;
    }

    public string path = string.Empty;
    private string musicName;
    private int musicIndex;

    public AudioClip clip;

    public void Init(int i, string path)
    {
        this.path = path;       //윈도우 환경의 경우, file:///, mac의 경우 file://

        musicIndex = i;

        string[] splitedPath = this.path.Split('\\');
        string[] splitedName = splitedPath[splitedPath.Length - 1].Split('.');
        musicName = splitedName[0];

        StartCoroutine(LoadMusic());
        //if (clip == null) MyDebug.Log("NULL!!!!!!!!!");
    }

    IEnumerator LoadMusic()
    {
        string filePath = $"file:///{path}";

        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(filePath, AudioType.WAV))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                MyDebug.Log(www.error);
            }
            else
            {
                clip = DownloadHandlerAudioClip.GetContent(www);
            }
        }
    }

    public string GetName()
    {
        return musicName;
    }

    public int GetIndex()
    {
        return musicIndex;
    }

    public override string ToString()
    {
        return $"{path}";
    }

}
