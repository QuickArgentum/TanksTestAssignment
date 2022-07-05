using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    private AudioData data;
    public AudioData Data
    {
        get
        {
            return data;
        }
        set
        {
            data = value;
            UpdateVolume();
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown(InputNames.MUTE_AUDIO))
        {
            Data.muted = !Data.muted;
            UpdateVolume();
        }
    }

    private void UpdateVolume()
    {
        AudioListener.volume = Data.muted ? 0.0f : 1.0f;
    }
}
