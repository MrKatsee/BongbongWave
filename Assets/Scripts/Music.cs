using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

public class Music : MonoBehaviour
{
    public static Music Init_Music(string path)
    {
        //MonoBehaviour을 상속한 클래스의 경우, GameObject에 Component로 존재해야한다.
        Music musicObject = new GameObject("Music").AddComponent<Music>();

        musicObject.Init(path);

        return musicObject;
    }

    public string filePath = string.Empty;

    public AudioClip clip;

    public void Init(string path)
    {
        filePath = $"file:///{path}";       //윈도우 환경의 경우, file:///, mac의 경우 file://
        StartCoroutine(LoadMusic());
        //if (clip == null) MyDebug.Log("NULL!!!!!!!!!");
    }

    IEnumerator LoadMusic()
    {
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
}
