using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Int3
{
    public int x;
    public int y;
    public int z;
    public static Int3 one = new Int3(1);
    public Int3(int v)
    {
        x = y = z = v;
    }
    public Int3(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    static public Int3 operator +(Int3 a, Int3 b)
    {
        return new Int3(a.x + b.x, a.y + b.y, a.z + b.z);
    }
    static public Int3 operator -(Int3 a, Int3 b)
    {
        return new Int3(a.x - b.x, a.y - b.y, a.z - b.z);
    }
    public Vector3 toVector(float scale = 1)
    {
        return new Vector3(x, y, z) * scale;
    }
    static public Int3 fromVector(Vector3 pos)
    {
        // Round the numbers down to nearest int:
        if (pos.x < 0) pos.x--;
        if (pos.y < 0) pos.y--;
        if (pos.z < 0) pos.z--;
        int x = (int)pos.x;
        int y = (int)pos.y;
        int z = (int)pos.z;
        return new Int3(x, y, z);
    }
    static public Int3 Dis(Int3 a, Int3 b)
    {
        int x = a.x - b.x;
        int y = a.y - b.y;
        int z = a.z - b.z;
        if (x < 0) x = -x;
        if (y < 0) y = -y;
        if (z < 0) z = -z;
        return new Int3(x, y, z);
    }
    public override string ToString()
    {
        return $"[{x}, {y}, {z}]";
    }
}

