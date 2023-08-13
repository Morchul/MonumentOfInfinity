using UnityEngine;
using UnityEngine.UI;

public static class SoundEffectManager
{

    private static GameObject soundGameObject;
    private static AudioSource soundAudioSource;
    public static void PlaySound(Sound sound, Vector3 position)
    {
        if(soundGameObject == null)
        {
            soundGameObject = new GameObject();
            soundAudioSource = soundGameObject.AddComponent<AudioSource>();
            soundAudioSource.spatialBlend = 1f;
        }
        soundGameObject.transform.position = position;
        soundAudioSource.PlayOneShot(GameAssets.GetInstance().GetSound(sound));
    }

    public static void AddButtonSound(this Button button)
    {
        button.onClick.AddListener(() => PlaySound(Sound.ButtonClick, new Vector3(0,0,0)));
    }

    public enum Sound
    {
        BuildBuilding,
        DestroyBuilding,
        ButtonClick,
        StoneHit,

        MainMusic1,
        MainMusic2,
        MainMusic3,
        EventMusic1,
        EventMusic2
    }
}
