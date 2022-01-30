using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class StartGame : MonoBehaviour
{
    AudioSource audioSource;
    void LoadingNewScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void StartLoadingNewScene(string sceneName)
    {
        StartCoroutine(PlaySoundAndLoadNewScene(sceneName));
    }
    IEnumerator PlaySoundAndLoadNewScene(string sceneName)
    {
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);

        LoadingNewScene(sceneName);
    }
}