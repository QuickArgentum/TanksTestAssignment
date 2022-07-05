using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : Singleton<ScoreManager>
{
    public Text ScoreLabel;
    public Text HighScoreLabel;

    private ScoreData data;
    public ScoreData Data
    { 
        get
        {
            return data;
        }
        set
        {
            data = value;
            UpdateUI();
        }
    }

    public void HandleKill(TankController tank)
    {
        if (tank.alignment == Alignment.ENEMY)
        {
            Data.score++;
            if (Data.score > Data.highScore)
            {
                Data.highScore = Data.score;
            }
        }
        else
        {
            Data.score = 0;
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        ScoreLabel.text = Data.score.ToString();
        HighScoreLabel.text = Data.highScore.ToString();
    }
}
