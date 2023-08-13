using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    static AudioSource audioSource;
    static DestroyedListener listener;

    private static bool eventRunning, musicPlaying;
    [SerializeField] float musicPauseTime;
    private float musicPauseTimer;

    private static List<IDestroyable> currentEvents;
    
    public static void StartEvent(SoldierAssignableEvent _event)
    {
        currentEvents.Add(_event);
        _event.AddDestroyedListener(listener);

        if (!eventRunning)
        {
            eventRunning = true;

            //prevent from a bug where Event Music never stops
            if(currentEvents.Count > 1)
            {
                currentEvents.Clear();
                _event.RemoveDestroyedListener(listener);
                return;
            }

            SoundEffectManager.Sound eventMusic = SoundEffectManager.Sound.EventMusic1;
            switch (Random.Range(0, 2))
            {
                case 0: eventMusic = SoundEffectManager.Sound.EventMusic1; break;
                case 1: eventMusic = SoundEffectManager.Sound.EventMusic1; break;
            }
            StartMusic(eventMusic, true, 7, 5);
        }
        
    }

    public static void EndEvent(IDestroyable destroyedEvent)
    {
        currentEvents.Remove(destroyedEvent);
        if(currentEvents.Count == 0)
        {
            eventRunning = false;
            StopMusic();
        }
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        currentEvents = new List<IDestroyable>();
        listener = EndEvent;
        eventRunning = false;
        musicPauseTimer = 35;
    }

    private void StartMusic()
    {
        SoundEffectManager.Sound mainMusic = SoundEffectManager.Sound.MainMusic1;
        switch (Random.Range(0, 3))
        {
            case 0: mainMusic = SoundEffectManager.Sound.MainMusic1; break;
            case 1: mainMusic = SoundEffectManager.Sound.MainMusic2; break;
            case 2: mainMusic = SoundEffectManager.Sound.MainMusic3; break;
        }
        StartMusic(mainMusic, false, 10, 10);
    }

    private static void StopMusic()
    {
        currentMusicLength = 0;
    }

    private static float currentMusicLength;
    private static float riseVolumeTime, fallVolumeTime;
    private static void StartMusic(SoundEffectManager.Sound sound, bool loop, float riseTime, float fallTime)
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        audioSource.clip = GameAssets.GetInstance().GetSound(sound);
        audioSource.loop = loop;
        audioSource.ignoreListenerVolume = true;
        riseVolumeTime = riseTime;
        fallVolumeTime = fallTime;
        currentMusicLength = loop ? 10000 : audioSource.clip.length;
        musicTimer = 0;
        volume = 0;
        audioSource.Play();
    }

    private static float musicTimer;
    private static float volume;
    private static float maxVolume = 0.3f;
    private float GetVolume()
    {
        musicTimer += Time.deltaTime;
        if (musicTimer > currentMusicLength - fallVolumeTime)
        {
            if (volume > 0)
                volume -= Time.deltaTime * maxVolume / fallVolumeTime;
            else
                audioSource.Stop();
        }
        else
        {
            if(volume < maxVolume)
            {
                volume += Time.deltaTime * maxVolume / riseVolumeTime;
            }
        }
        return volume;
    }

    private void Update()
    {
        if (!eventRunning)
        {
            if (!musicPlaying)
            {
                musicPauseTimer += Time.deltaTime;
                if(musicPauseTimer > musicPauseTime)
                {
                    StartMusic();
                    musicPauseTimer = 0;
                    musicPlaying = true;
                }
            }
            else
            {
                if (!audioSource.isPlaying)
                {
                    musicPlaying = false;
                }                
            }
        }
        if (audioSource.isPlaying)
            audioSource.volume = GetVolume();
    }
}
