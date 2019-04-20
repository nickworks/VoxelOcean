using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateHUD : MonoBehaviour
{

    public VoxelUniverse universe;

    public void Generate()
    {
        if (universe)
        {
            universe.RandomizeFields();
            universe.Create();
        }
    }
}
