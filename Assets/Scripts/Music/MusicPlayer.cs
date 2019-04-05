using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MusicPlayer : MonoBehaviour
{
    
    public  bool random = false;// to check if they want music to be random
    Object[] myMusic; // array list to hold our music
    int i = 0; // keep track of what array song is playing
    public static float[] fftSamples = new float[64];// holds the fft data
    /// <summary>
    /// grabs all the music and adds it to an array
    /// adds waveform to samples float
    /// </summary>
    void Start()
    {
        myMusic = Resources.LoadAll("Music", typeof(AudioClip));
        GetComponent<AudioSource>().clip = myMusic[2] as AudioClip;
         AudioSource audioSource = GetComponent<AudioSource>();
        float[] samples = new float[audioSource.clip.samples * audioSource.clip.channels];
        audioSource.clip.GetData(samples, 0);

        for (int i = 0; i < samples.Length; ++i)
        {
            samples[i] = samples[i] * 0.5f;
        }

        audioSource.clip.SetData(samples, 0);
    }



    // Update is called once per frame
    /// <summary>
    /// checks if music is playing if not it plays a random song if set random is set to true
    /// or plays song in playlist oder 
    /// grabs the fft data
    /// </summary>
    void Update()
    {
        AudioListener.GetSpectrumData(fftSamples, 0, FFTWindow.Rectangular);
        if (!GetComponent<AudioSource>().isPlaying && random == true)
        {
            playRandomMusic();
        }
        if (!GetComponent<AudioSource>().isPlaying && random == false)
        {
            playMusic();
        }

    }
    /// <summary>
    /// gets a song from the array from a random song in the array based on from 0 and the length of the array then plays the chosen song
    ///</summary>
    void playRandomMusic()
    {
        GetComponent<AudioSource>().clip = myMusic[Random.Range(0, myMusic.Length)] as AudioClip;
        GetComponent<AudioSource>().Play();
    }
    /// <summary>
    /// picks a song from the array and goes up by one every time the function is called and plays the next song
    /// when i is great than the lenght of the array gets set back to 0 and plays first song
    /// </summary>
    void playMusic()
    {
        if (i < myMusic.Length)
        {
            GetComponent<AudioSource>().clip = myMusic[i] as AudioClip;
            GetComponent<AudioSource>().Play();
            i += 1;
                if ( i == myMusic.Length)
            {
                i = 0;
            }
        }
    }
}

