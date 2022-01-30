using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioClip normalBgmClip;
    public AudioClip eventBgmClip;
    public AudioClip eventClearClip;
    public AudioClip eventFailClip;
    public AudioClip eventStartClip;

    public float bgmFadeTime = 1.0f;

    AudioSource bgmSource;
    AudioSource eventSeSource;

    Coroutine bgmFadeOutCorutine;

    private void Awake()
    {
        bgmSource = gameObject.AddComponent<AudioSource>();
        eventSeSource = gameObject.AddComponent<AudioSource>();
    }

    private void Start()
    {
        bgmSource.clip = normalBgmClip;
        bgmSource.Play();
        bgmSource.loop = true;
    }

    public void StartChangeBGM(bool isEventBGM)
    {
        if (bgmFadeOutCorutine != null)
        {
            StopCoroutine(bgmFadeOutCorutine);
            bgmFadeOutCorutine = null;
        }
        bgmFadeOutCorutine = StartCoroutine(ChangeBGM(isEventBGM));
    }

    IEnumerator ChangeBGM(bool isEventBGM)
    {
        float startVolume = bgmSource.volume;

        while (bgmSource.volume > 0)
        {
            bgmSource.volume -= startVolume * Time.deltaTime / bgmFadeTime;
            yield return null;
        }

        bgmSource.Stop();
        bgmSource.clip = isEventBGM ? eventBgmClip : normalBgmClip;
        bgmSource.Play();

        startVolume = 1f;

        while (bgmSource.volume < 1)
        {
            bgmSource.volume += startVolume * Time.deltaTime / bgmFadeTime;
            yield return null;
        }

        bgmSource.volume = 1f;
    }

    public void PlayEventStartSE()
    {
        eventSeSource.clip = eventStartClip;
        eventSeSource.Play();
    }

    public void PlayEventEndSE(bool isCleared)
    {
        eventSeSource.clip = isCleared ? eventClearClip : eventFailClip;
        eventSeSource.Play();
    }
}
