using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    Text scoreText;

    private int score;
    private int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
            UpdateText();
        }
    }

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GameObject.Find("Score").GetComponent<Text>();
    }

    public void ResetScore()
    {
        Score = 0;
    }

    public void IncreaseScore()
    {
        Score++;
    }

    void UpdateText()
    {
        scoreText.text = score.ToString();
    }
}
