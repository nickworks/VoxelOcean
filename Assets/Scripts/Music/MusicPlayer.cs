using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// class loads music from rescources/music
/// set my music to number of songs in folder
/// if you want random music to play turn random to true false will play in oder 
/// needs to be on game obgject with an audio listner 
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    /// <summary>
    /// Audiosource source of audio
    /// </summary>
    AudioSource audioSource;
    /// <summary>
    /// Whether or not shuffle is enabled.
    /// </summary>
    public bool shuffle = false;
    /// <summary>
    ///  This List holds our music tracks.
    /// </summary>
    public List<AudioClip> songs = new List<AudioClip>();
    /// <summary>
    ///  Holds the current fft spectrum data. These are the amplitudes of various frequencies.
    /// </summary>
    public static float[] fftSamples = new float[64];
    /// <summary>
    /// Holds the raw waveform data for the next second of playback.
    /// </summary>
    public static float[] samples;

    /// <summary>
    /// grabs all the music and adds it to an array
    /// adds waveform to samples float
    /// </summary>
    void Start()
    {
        audioSource = GetComponent<AudioSource>();   
    }

    /// <summary>
    /// checks if music is playing if not it plays a random song if set random is set to true
    /// or plays song in playlist oder 
    /// grabs the fft data
    /// </summary>
    void Update()
    {
        PlayMusic();
        AudioData();
    }
    /// <summary>
    /// grabs the FFT data and adds to fftsamples
    /// grabs the waveform data and adds to samples
    /// </summary>
    void AudioData()
    {
        if (!audioSource.clip) return;
        samples = new float[audioSource.clip.frequency * audioSource.clip.channels]; // 1 second worth of samples
        int pos = audioSource.timeSamples;
        AudioListener.GetSpectrumData(fftSamples, 0, FFTWindow.Rectangular);
        audioSource.clip.GetData(samples, pos);
    }
    /// <summary>
    /// picks a song from the array and goes up by one every time the function is called and plays the next song
    /// when i is great than the lenght of the array gets set back to 0 and plays first song
    /// </summary>
    void PlayMusic()
    {
        if (audioSource.isPlaying) return;
        if (shuffle)
        {
            audioSource.clip = songs[Random.Range(0, songs.Count)];
            audioSource.Play();
        }
        else {
            int i = songs.IndexOf(audioSource.clip) + 1;
            i %= songs.Count;
            if (i >= songs.Count) return; // invalid track number, abort

            audioSource.clip = songs[i];
            audioSource.Play();
        }
    }
}

