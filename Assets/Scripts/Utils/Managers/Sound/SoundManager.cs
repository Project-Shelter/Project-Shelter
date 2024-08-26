using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public enum SoundType
{
    BGM,
    EFFECT,
}

public class SoundManager : MonoSingleton<SoundManager>
{
    [SerializeField] private AudioMixer audioMixer;
    private float currentBGMVolume, currentEffectVolume;
    private Dictionary<string, AudioClip> clipsDic;
    [SerializeField] private AudioClip[] preLoadClips;
    private List<TemporarySoundPlayer> instantiatedSounds;

    private void Start()
    {
        clipsDic = new Dictionary<string, AudioClip>();

        foreach (AudioClip clip in preLoadClips)
        {
            clipsDic.Add(clip.name, clip);
        }
        instantiatedSounds = new List<TemporarySoundPlayer>();

        InitVoumes(0, 0);
    }

    private AudioClip GetClip(string clipName)
    {
        AudioClip clip = clipsDic[clipName];

        if (clip == null)
        {
            Debug.LogError(clipName + "is not find");
            return null;
        }

        return clip;
    }

    private void AddToList(TemporarySoundPlayer soundPlayer)
    {
        instantiatedSounds.Add(soundPlayer);
    }

    public void StopLoopSound(string clipName)
    {
        foreach (TemporarySoundPlayer audioPlayer in instantiatedSounds)
        {
            if (audioPlayer.ClipName == clipName)
            {
                instantiatedSounds.Remove(audioPlayer);
                Destroy(audioPlayer.gameObject);
                return;
            }
        }
        Debug.LogError(clipName + "is not find (StopLoopSound)");
    }

    public void PlaySound2D(string clipName, float delay = 0f, bool isLoop = false, SoundType type = SoundType.EFFECT)
    {
        GameObject soundObj = new GameObject("TemporarySoundPlayer 2D");
        TemporarySoundPlayer soundPlayer = soundObj.AddComponent<TemporarySoundPlayer>();

        if (isLoop)
        {
            AddToList(soundPlayer);
        }
        soundPlayer.InitSound2D(GetClip(clipName));
        soundPlayer.Play(audioMixer.FindMatchingGroups(type.ToString())[0], delay, isLoop);
    }

    public void InitVoumes(float bgm, float effect)
    {
        SetVolumes(SoundType.BGM, bgm);
        SetVolumes(SoundType.EFFECT, effect);
    }

    public void SetVolumes(SoundType type, float value)
    {
        audioMixer.SetFloat(type.ToString(), value);
    }
}