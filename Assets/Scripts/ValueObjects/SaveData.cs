using System;

[Serializable]
public class SaveData
{
    public ScoreData score;

    public SaveData()
    {
        score = new ScoreData();
    }
}