using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets instance;

    public static GameAssets GetInstance()
    {
        if (instance == null) instance = Instantiate(Resources.Load<GameAssets>("GameAssets"));
        return instance;
    }

    #region AudioClipSound
    public AudioClipSound[] audioClips;

    [System.Serializable]
    public class AudioClipSound
    {
        public SoundEffectManager.Sound sound;
        public AudioClip audioClip;
    }

    public AudioClip GetSound(SoundEffectManager.Sound sound)
    {
        foreach(AudioClipSound audioClip in audioClips)
        {
            if(audioClip.sound == sound)
            {
                return audioClip.audioClip;
            }
        }
        Debug.LogError("No AudioClip found for: " + sound);
        return null;
    }
    #endregion



    #region ResourceSprite
    public ResourceSprite[] resourceSprites;

    [System.Serializable]
    public class ResourceSprite
    {
        public ResourceType type;
        public Sprite sprite;
    }

    public Sprite GetResourceSprite(ResourceType type)
    {
        foreach(ResourceSprite resourceSprite in resourceSprites)
        {
            if (resourceSprite.type == type)
                return resourceSprite.sprite;
        }
        Debug.LogError("There is no sprite for : " + type);
        return null;
    }
    #endregion

}
