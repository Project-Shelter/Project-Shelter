using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class TemporarySoundPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    public string ClipName
    {
        get
        {
            return audioSource.clip.name;
        }
    }

    public void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(AudioMixerGroup audioMixer, float delay, bool isLoop)
    {
        audioSource.outputAudioMixerGroup = audioMixer;
        audioSource.loop = isLoop;
        audioSource.Play();

        if (!isLoop)
        {
            StartCoroutine(COR_DestoyWhenFinish(audioSource.clip.length));
        }
    }

    public void InitSound2D(AudioClip clip)
    {
        audioSource.clip = clip;
    }

    private IEnumerator COR_DestoyWhenFinish(float clipLength)
    {
        yield return new WaitForSeconds(clipLength);
        Destroy(gameObject);
    }
}