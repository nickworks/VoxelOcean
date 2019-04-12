using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicPlayer : MonoBehaviour
{
    int musicNum = 0;
    AudioSource audioSource;
    public AudioClip[] songNames;

    public Text musicName;

    public Slider slider;
    private bool stop = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartAudio();
    }

    // Update is called once per frame
    void Update()
    {
        if (!stop)
        {
            slider.value = audioSource.time / audioSource.clip.length;
            if (slider.value == 1)
            {
                musicNum++;
                slider.value = 0;

                if(musicNum >= songNames.Length)
                {
                    musicNum = 0;
                }
                StartAudio();
            }
        }
    }

    public void ScrubMusic()
    {
        audioSource.time = audioSource.clip.length * slider.value;
    }

    public void StopAudio()
    {
        audioSource.Stop();
        stop = true;
    }

    public void ChangeSong1()
    {
        musicNum = 0;
        audioSource.clip = songNames[musicNum];
        slider.value = 0;
        musicName.text = audioSource.clip.name;
        audioSource.Play();
        print("Song 1");
    }

    public void ChangeSong2()
    {
        musicNum = 1;
        audioSource.clip = songNames[musicNum];
        slider.value = 0;
        musicName.text = audioSource.clip.name;
        audioSource.Play();
        print("Song 2");
    }

    public void ChangeSong3()
    {
        musicNum = 2;
        audioSource.clip = songNames[musicNum];
        slider.value = 0;
        musicName.text = audioSource.clip.name;
        audioSource.Play();
        print("Song 3");
    }

    public void StartAudio(int changeMusic = 0)
    {
        musicNum += changeMusic;
        slider.value = 0;
        if(musicNum >= songNames.Length)
        {
            musicNum = 0;
        }
        else if(musicNum < 0)
        {
            musicNum = songNames.Length - 1;
        }

        if(audioSource.isPlaying && changeMusic == 0)
        {
            return;
        }

        if (stop)
        {
            stop = false;
        }

        audioSource.clip = songNames[musicNum];
        musicName.text = audioSource.clip.name;
        //slider.maxValue = audioSource.clip.length;
        //slider.value = 0;
        audioSource.Play();
    }
}
