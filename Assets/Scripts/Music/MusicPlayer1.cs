using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

/// <summary>
/// class loads music from rescources/music
/// set my music to number of songs in folder
/// if you want random music to play turn random to true false will play in oder 
/// needs to be on game obgject with an audio listner 
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class MusicPlayer1 : MonoBehaviour
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

    private List<AudioClip> shuffledSongs = new List<AudioClip>();

    private bool paused = false;

    /// <summary>
    /// Assigns the name of each track to the text at the top of the player as well as the buttons in the player
    /// </summary>
    public Text musicName;
    public Text songOneName;
    public Text songTwoName;
    public Text songThreeName;

    /// <summary>
    /// Slider for scrubbing through songs
    /// </summary>
    public Slider scrubSlider;

    /// <summary>
    /// Slider for changing the volume of the music
    /// </summary>
    public Slider volSlider;

    /// <summary>
    ///  Holds the current fft spectrum data. These are the amplitudes of various frequencies.
    /// </summary>
    public static float[] fftSamples = new float[64];
    /// <summary>
    /// Holds the raw waveform data for the next second of playback.
    /// </summary>
    public static float[] samples;

    int musicNum = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        //starts the slider halfway
        volSlider.value = 0.5f;
    }

    /// <summary>
    /// checks if music is playing if not it plays a random song if set random is set to true
    /// or plays song in playlist oder 
    /// grabs the fft data
    /// </summary>
    void Update()
    {
        if (!paused && !audioSource.isPlaying) NextTrack();
        UpdateAudioData();

        if (!paused)
        {
            scrubSlider.value = audioSource.time / audioSource.clip.length;
            if (scrubSlider.value == 1)
            {
                scrubSlider.value = 0;
            }
        }
    }
    /// <summary>
    /// grabs the FFT data and adds to fftsamples
    /// grabs the waveform data and adds to samples
    /// </summary>
    void UpdateAudioData()
    {
        if (!audioSource.clip) return;
        samples = new float[audioSource.clip.frequency * audioSource.clip.channels]; // 1 second worth of samples
        int pos = audioSource.timeSamples;
        AudioListener.GetSpectrumData(fftSamples, 0, FFTWindow.Rectangular);
        audioSource.clip.GetData(samples, pos);

        //Assigns the song name in that slot to that text
        songOneName.text = songs[0].name;
        songTwoName.text = songs[1].name;
        songThreeName.text = songs[2].name;
    }
    void MakeShuffleList()
    {
        List<AudioClip> temp = songs.GetRange(0, songs.Count);
        shuffledSongs = new List<AudioClip>();
        while (temp.Count > 0)
        {
            int i = Random.Range(0, temp.Count);
            shuffledSongs.Add(temp[i]);
            temp.RemoveAt(i);
        }
    }
    List<AudioClip> GetPlayList()
    {
        if (shuffle && shuffledSongs.Count <= 0) MakeShuffleList();
        return shuffle ? shuffledSongs : songs;
    }
    
    public void ShuffleMusic()
    {
    shuffle = true;
    }

    public void ScrubMusic()
    {
        audioSource.time = audioSource.clip.length * scrubSlider.value;
    }
    public void MusicVol()
    {
        //Makes the volume of the Audio match the Slider value
        audioSource.volume = volSlider.value;
    }
    public void NextTrack()
    {
        List<AudioClip> tracks = GetPlayList();

        int i = tracks.IndexOf(audioSource.clip) + 1;
        i %= tracks.Count;

        scrubSlider.value = 0;

        paused = true;
        if (i >= tracks.Count) return; // invalid track number, abort
        paused = false;

        audioSource.clip = tracks[i];
        audioSource.Play();

        musicName.text = audioSource.clip.name;
    }
    public void PrevTrack()
    {
        List<AudioClip> tracks = GetPlayList();

        int i = tracks.IndexOf(audioSource.clip) - 1;
        if (i < 0) i = tracks.Count - 1;

        scrubSlider.value = 0;

        paused = true;
        if (i < 0) return; // invalid track number, abort
        paused = false;

        audioSource.clip = tracks[i];
        audioSource.Play();

        musicName.text = audioSource.clip.name;
    }

    public void ChangeSong1()
    {
        musicNum = 0;
        audioSource.clip = songs[musicNum];
        scrubSlider.value = 0;
        musicName.text = audioSource.clip.name;
        audioSource.Play();
        print("Song 1");
    }

    public void ChangeSong2()
    {
        musicNum = 1;
        audioSource.clip = songs[musicNum];
        scrubSlider.value = 0;
        musicName.text = audioSource.clip.name;
        audioSource.Play();
        print("Song 2");
    }

    public void ChangeSong3()
    {
        musicNum = 2;
        audioSource.clip = songs[musicNum];
        scrubSlider.value = 0;
        musicName.text = audioSource.clip.name;
        audioSource.Play();
        print("Song 3");
    }

    public void Pause()
    {
        paused = !paused;
        if (paused) audioSource.Pause();
        else audioSource.UnPause();
        musicName.text = audioSource.clip.name;
    }
}
[CustomEditor(typeof(MusicPlayer1))]
class MusicPlayerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Toggle Pause")) (target as MusicPlayer1).Pause();
        if (GUILayout.Button("Next Track")) (target as MusicPlayer1).NextTrack();
        if (GUILayout.Button("Prev Track")) (target as MusicPlayer1).PrevTrack();
    }
}

