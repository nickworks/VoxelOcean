using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeSlider : MonoBehaviour
{
    AudioSource audioSource;
    //Value from the slider, and it converts to volume level
    float m_MySliderValue;

    /// <summary>
    /// Gets the audio to be affected by the slider
    /// starts the slider half way
    /// </summary>
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        //Start the Slider half way
        m_MySliderValue = 0.5f;
    }

    /// <summary>
    /// Displays the slider to the screens upper left corner
    /// Highest value: 1
    /// Lowest value: 2
    /// </summary>
    void OnGUI()
    {
        //Create a horizontal Slider that controls volume levels. Its highest value is 1 and lowest is 0
        m_MySliderValue = GUI.HorizontalSlider(new Rect(0, 0, 200, 60), m_MySliderValue, 0.0F, 1.0F);
        //Makes the volume of the Audio match the Slider value
        audioSource.volume = m_MySliderValue;
    }
}
