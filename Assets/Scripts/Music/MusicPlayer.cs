using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// class loads music from rescources/music
/// set my music to number of songs in folder
/// if you want random music to play turn random to true false will play in oder 
/// needs to be on game obgject with an audio listner 
/// </summary>
public class MusicPlayer : MonoBehaviour
{
    AudioSource audioSource; // this is audisource
    public bool random = false;// to check if they want music to be random
    Object[] myMusic; // array list to hold our music
    int i = 0; // keep track of what array song is playing
    public static float[] fftSamples = new float[64];// holds the fft data 
    public static float[] samples;//holds waveform data

    /// <summary>
    /// grabs all the music and adds it to an array
    /// adds waveform to samples float
    /// </summary>
    void Start()
    {
        myMusic = Resources.LoadAll("Music", typeof(AudioClip));
        GetComponent<AudioSource>().clip = myMusic[2] as AudioClip;

        audioSource = GetComponent<AudioSource>();
        
        
    }



    // Update is called once per frame
    /// <summary>
    /// checks if music is playing if not it plays a random song if set random is set to true
    /// or plays song in playlist oder 
    /// grabs the fft data
    /// </summary>
    void Update()
    {


        PlayRandomMusic();
        PlayMusic();
        AudioData();


    }
    /// <summary>
    /// gets a song from the array from a random song in the array based on from 0 and the length of the array then plays the chosen song
    ///</summary>
    void PlayRandomMusic()
    {
        if (!audioSource.isPlaying && random == true)
        {
            GetComponent<AudioSource>().clip = myMusic[Random.Range(0, myMusic.Length)] as AudioClip;
            GetComponent<AudioSource>().Play();
        }
    }
    /// <summary>
    /// grabs the FFT data and adds to fftsamples
    /// grabs the waveform data and adds to samples
    /// </summary>
    void AudioData()
    {   samples = new float[audioSource.clip.frequency * audioSource.clip.channels]; // 1 second worth of samples
        AudioListener.GetSpectrumData(fftSamples, 0, FFTWindow.Rectangular);
        audioSource.clip.GetData(samples, 0);
        

    }
    /// <summary>
    /// picks a song from the array and goes up by one every time the function is called and plays the next song
    /// when i is great than the lenght of the array gets set back to 0 and plays first song
    /// </summary>
    void PlayMusic()
    {
        if (!audioSource.isPlaying && random == false)
        {
            if (i < myMusic.Length)
            {
                GetComponent<AudioSource>().clip = myMusic[i] as AudioClip;
                GetComponent<AudioSource>().Play();
                i += 1;
                if (i == myMusic.Length)
                {
                    i = 0;
                }
            }


        }
    }
}

