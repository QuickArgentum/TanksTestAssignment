using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : Singleton<ScoreManager>
{
    public Text ScoreLabel;
    public Text HighScoreLabel;

    private ScoreData data;

    public void HandleKill(TankController tank)
    {
        if (tank.alignment == Alignment.ENEMY)
        {
            data.score++;
            if (data.score > data.highScore)
            {
                data.highScore = data.score;
            }
        }
        else
        {
            data.score = 0;
        }
        UpdateUI();
    }

    public void SetData(ScoreData data)
    {
        this.data = data;
        UpdateUI();
    }

    public ScoreData GetData()
    {
        return data;
    }

    private void UpdateUI()
    {
        ScoreLabel.text = data.score.ToString();
        HighScoreLabel.text = data.highScore.ToString();
    }
}
