using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Audio
{
    /// <summary>
    /// Воспроизведение звука из массива
    /// </summary>
    /// <param name="clipName">Имя звука</param>

    public void PlaySound(string clipName)
    {
        SourceSFX.PlayOneShot(GetSound(clipName), SfxVolume);
    }

    /// <summary>
    /// Воспроизведение звука из массива со случайной частотой
    /// </summary>
    /// <param name="clipName">Имя звука</param>

    public void PlaySoundRandomPitch(string clipName)
    {
        SourceRandomPitchSFX.pitch = Random.Range(0.7f, 1.3f);
        SourceRandomPitchSFX.PlayOneShot(GetSound(clipName),
        SfxVolume);
    }

    /// <summary>
    /// Воспроизведение музыка
    /// </summary>
    /// <param name="menu">для главного меню?</param>

    public void PlayMusic(bool menu)
    {
        if (menu)
            SourceMusic.clip = menuMusic;
        else
            SourceMusic.clip = gameMusic;
        SourceMusic.volume = MusicVolume;
        SourceMusic.loop = true;
        SourceMusic.Play();
    }

    /// <summary>
    /// Поиск звука в массиве
    /// </summary>
    /// <param name="clipName">Имя звука</param>
    /// <returns>Звук. Если звку не найден, возвращается значение переменной defaultClip</returns>

    private AudioClip GetSound(string clipName)
    {
        for (var i = 0; i < sounds.Length; i++)
            if (sounds[i].name == clipName)
                return sounds[i];
        Debug.LogError("Can not find clip " + clipName);
        return defaultClip;
    }

    #region Private_Variables
    //Ссылка на источник звука для воспроизведения звуков
    private AudioSource sourceSFX;
    //Ссылка на источник звука для воспроизведения музыки private
    private AudioSource sourceMusic;
    //Ссылка на источник звука для воспроизведения звуков со случайной частотой
    private AudioSource sourceRandomPitchSFX;
    //громкость музыки
    private float musicVolume = 1f;
    //громкость звуков
    private float sfxVolume = 1f;
    //Массив звуков
    [SerializeField] private AudioClip[] sounds;
    //Звук по умолчанию, на случай, если в массиве отсутствует требуемый
    [SerializeField] private AudioClip defaultClip;
    //Музыка для главного меню
    [SerializeField] private AudioClip menuMusic;
    //Музыка для игры на уровнях
    [SerializeField] private AudioClip gameMusic;
    #endregion

    #region Public_Properties
    public AudioSource SourceSFX
    {
        get { return sourceSFX; }
        set { sourceSFX = value; }
    }
    public AudioSource SourceMusic
    {
        get { return sourceMusic; }
        set { sourceMusic = value; }
    }
    public AudioSource SourceRandomPitchSFX
    {
        get { return sourceRandomPitchSFX; }
        set { sourceRandomPitchSFX = value; }
    }
    public float MusicVolume
    {
        get { return musicVolume; }
        set
        {
            musicVolume = value;
            SourceMusic.volume = musicVolume;
            DataStore.SaveOptions();
        }
    }
    public float SfxVolume
    {
        get { return sfxVolume; }
        set
        {
            sfxVolume = value;
            SourceSFX.volume = sfxVolume;
            SourceRandomPitchSFX.volume = sfxVolume;
            DataStore.SaveOptions();
        }
    }
    #endregion
}
