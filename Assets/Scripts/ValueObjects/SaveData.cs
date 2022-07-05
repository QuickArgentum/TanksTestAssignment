using System;

[Serializable]
public class SaveData
{
    public ScoreData score;
    public AudioData audio;

    public SaveData()
    {
        score = new ScoreData();
        audio = new AudioData();
    }
}