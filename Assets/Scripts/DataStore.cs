using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataStore
{
    private const string MUSIC_VOLUME_KEY = "MusicVolume";
    private const string SOUND_VOLUME_KEY = "SoundVolume";
    private const string SAVED_LEVEL_KEY = "Level";
    private const string SAVED_SCORE_KEY = "Score";
    private const float DEFAULT_VOLUME = 0.75f;

    public static void SaveOptions()
    {
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, Controller.ControllerInstance.NewAudio.MusicVolume);
        PlayerPrefs.SetFloat(SOUND_VOLUME_KEY, Controller.ControllerInstance.NewAudio.SfxVolume);
        PlayerPrefs.Save();
    }

    public static void LoadOptions()
    {
        Controller.ControllerInstance.NewAudio.MusicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, DEFAULT_VOLUME);
        Controller.ControllerInstance.NewAudio.SfxVolume = PlayerPrefs.GetFloat(SOUND_VOLUME_KEY, DEFAULT_VOLUME);
    }

    public static void SaveGame()
    {
        PlayerPrefs.SetInt(SAVED_LEVEL_KEY, Controller.ControllerInstance.CurrentLevel);
        PlayerPrefs.SetInt(SAVED_SCORE_KEY, Controller.ControllerInstance.NewScore.CurrentScore);
        PlayerPrefs.Save();
    }

    public static void LoadGame()
    {
        Controller.ControllerInstance.CurrentLevel = PlayerPrefs.GetInt(SAVED_LEVEL_KEY, 1);
        Controller.ControllerInstance.NewScore.CurrentScore = PlayerPrefs.GetInt(SAVED_SCORE_KEY, 0);
    }
}
