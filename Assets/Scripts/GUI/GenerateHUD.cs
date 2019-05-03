using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateHUD : MonoBehaviour
{

    public void Generate()
    {
        if (VoxelUniverse.main)
        {
            VoxelUniverse.main.Create(true);
        }
    }
}
