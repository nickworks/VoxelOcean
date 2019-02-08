using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateHUD : MonoBehaviour
{
    public Text percentText;
    public Button button;

    public VoxelUniverse universe;

    public void Generate()
    {
        if (universe)
        {
            universe.RandomizeFields();
            universe.Create();
        }
    }
    void Update()
    {
        if (universe)
        {
            button.gameObject.SetActive(true);
            if (universe.isGenerating)
            {
                percentText.gameObject.SetActive(true);
                percentText.text = Mathf.Round(100 * universe.percentGenerated) + "%";
            }
            else
            {
                percentText.gameObject.SetActive(false);
            }
        } else
        {
            button.gameObject.SetActive(false);
            percentText.gameObject.SetActive(true);
            percentText.text = "ERROR";
        }
    }

}
