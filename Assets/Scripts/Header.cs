using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Header : MonoBehaviour
{
    public Music curMusic = null;
    public Music CurMusic
    {
        get { return curMusic; }
        set { curMusic = value; }
    }
}
