using NUnit.Framework;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MusicAmbiance : MonoBehaviour
{
    [SerializeField] private string[] musics;

    [SerializeField] private bool enable;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (enable)
            StartCoroutine(LoopAmbiance());
    }

    private void PlayMusic(string name)
    {
        if (AudioManager.instance != null && name != null)
        {
            AudioManager.instance.Play(name);
        }
    }

    private IEnumerator LoopAmbiance()
    {
        while (enable)
        {

            for (int i = 0; i <= musics.Length; i++)
            {
                PlayMusic(musics[i]);

                float timeMusic = 0f;

                if (AudioManager.instance != null && musics[i] != null)
                {
                    Sound sound = AudioManager.instance.GetSound(musics[i]);
                    if (sound != null)
                    {
                        timeMusic = sound.clip.length;
                    }
                }

                yield return new WaitForSeconds(timeMusic);

            }
        }
    }
}
