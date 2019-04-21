using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SignalFieldGenerator : MonoBehaviour
{
    public enum SignalType
    {
        AddOnly,
        SubtractOnly,
        Multiply,
        Average,
        Sphere,
        None
    }

    [System.Serializable]
    public class SignalField
    {
        public SignalType type;
        [Range(1, 100)] public float zoom = 20;
        [Range(-0.2f, 0.2f)] public float densityBias = 0;
        [Range(0, 1)] public float flattenAmount = 0;
        [Range(-20, 20)] public float flattenOffset = 0;

        static public SignalField Random()
        {
            SignalField res = new SignalField();
            res.zoom = UnityEngine.Random.Range(1f, 100);
            res.densityBias = UnityEngine.Random.Range(-0.2f, 0.2f);
            res.flattenAmount = UnityEngine.Random.Range(0f, 1);
            res.flattenOffset = 0;
            res.type = (SignalType)UnityEngine.Random.Range(0, 5);
            return res;
        }
    }

    public SignalField[] signalFields;

    /// <summary>
    /// Samples a given grid position and returns the noise value at this position.
    /// </summary>
    /// <param name="pos">A position in world space.</param>
    /// <returns>The density of the given position. Depending on the noise function used, this should be in the -1 to +1 range.</returns>
    public float GetDensitySample(Vector3 pos)
    {
        float res = 0;
        foreach (SignalField field in signalFields)
        {
            //Vector3 p = pos + transform.position; // convert from local coordinates to world coordinates
            float val = Noise.Sample(pos / field.zoom); // simplex.noise(pos.x, pos.y, pos.z);


            if (field.type == SignalType.Sphere)
            {
                float size = 8 + field.flattenOffset;
                size *= size;
                float d = pos.sqrMagnitude;
                val -= (d / size - size) * field.flattenAmount * .05f;
            }
            else
            {
                // use the vertical position to influence the density:
                val -= (pos.y + field.flattenOffset) * field.flattenAmount * .05f;
            }

            // adjust the final density using the densityBias:
            val += field.densityBias;

            // adjust how various fields are mixed together:
            switch (field.type)
            {
                case SignalType.Sphere:
                case SignalType.AddOnly:
                    if (val > 0 || res == 0) res += val;
                    break;
                case SignalType.SubtractOnly:
                    if (val < 0 || res == 0) res += val;
                    break;
                case SignalType.Multiply:
                    res *= val;
                    break;
                case SignalType.Average:
                    res = (val + res) / 2;
                    break;
                case SignalType.None:
                    break;
            }
        }
        return res;
    }
    public void RandomizeFields()
    {
        int fieldCount = Random.Range(1, 8);
        List<SignalField> fields = new List<SignalField>();
        for (int i = 0; i < fieldCount; i++)
        {
            fields.Add(SignalField.Random());
        }
        fields[0].type = SignalType.AddOnly;
        signalFields = fields.ToArray();
    }
    [CustomEditor(typeof(SignalFieldGenerator))]
    class SignalFieldGeneratorEditor : Editor
    {
        override public void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Randomize Noise"))
            {
                (target as SignalFieldGenerator).RandomizeFields();
            }
        }
    }
}
