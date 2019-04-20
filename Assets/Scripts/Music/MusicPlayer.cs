using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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

    private List<AudioClip> shuffledSongs = new List<AudioClip>();

    private bool paused = false;

    /// <summary>
    ///  Holds the current fft spectrum data. These are the amplitudes of various frequencies.
    /// </summary>
    public static float[] fftSamples = new float[64];
    /// <summary>
    /// Holds the raw waveform data for the next second of playback.
    /// </summary>
    public static float[] samples;

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
        if(!paused && !audioSource.isPlaying) NextTrack();
        UpdateAudioData();
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
    }
    void MakeShuffleList()
    {
        List<AudioClip> temp = songs.GetRange(0, songs.Count);
        shuffledSongs = new List<AudioClip>();
        while(temp.Count > 0)
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
    public void NextTrack()
    {
        List<AudioClip> tracks = GetPlayList();

        int i = tracks.IndexOf(audioSource.clip) + 1;
        i %= tracks.Count;

        paused = true;
        if (i >= tracks.Count) return; // invalid track number, abort
        paused = false;

        audioSource.clip = tracks[i];
        audioSource.Play();
    }
    public void PrevTrack()
    {
        List<AudioClip> tracks = GetPlayList();

        int i = tracks.IndexOf(audioSource.clip) - 1;
        if (i < 0) i = tracks.Count - 1;

        paused = true;
        if (i < 0) return; // invalid track number, abort
        paused = false;

        audioSource.clip = tracks[i];
        audioSource.Play();
    }
    public void Pause()
    {
        paused = !paused;
        if (paused) audioSource.Pause();
        else audioSource.UnPause();
    }
}
[CustomEditor(typeof(MusicPlayer))]
class MusicPlayerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Toggle Pause")) (target as MusicPlayer).Pause();
        if (GUILayout.Button("Next Track")) (target as MusicPlayer).NextTrack();
        if (GUILayout.Button("Prev Track")) (target as MusicPlayer).PrevTrack();
    }
}

