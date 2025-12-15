using NUnit.Framework;
using System.Collections;
using UnityEngine;

public class MusicAmbiance : MonoBehaviour
{
    [SerializeField] private string[] musics;

    [SerializeField] private bool enable;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (enable)
        PlayMusic(musics[0]);

        for (int i = 0; i < musics.Length; i++)
        {
            
        }
    }

    private void PlayMusic(string name)
    {
        if (AudioManager.instance != null && name != null)
        {
            AudioManager.instance.Play(name);
        }
    }
}
