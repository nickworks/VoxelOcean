using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class CoralFingers : MonoBehaviour
{
    MeshFilter meshFilter;

    public Vector3 branchScaling = new Vector3(.25f, 1, .25f);
    void Start()
    {

    }

    // Update is called once per frame
    public void Build()
    {
        int iterations = Random.Range(8, 11);
        List<CombineInstance> meshes = new List<CombineInstance>();
        Grow2(iterations, meshes, Vector3.zero, Quaternion.identity, 1);
        Grow(iterations, meshes, Vector3.zero, Quaternion.identity, 1);
        Grow3(iterations, meshes, Vector3.zero, Quaternion.identity, 1);
        Grow4(iterations, meshes, Vector3.zero, Quaternion.identity, 1);
        //Grow8(iterations, meshes, Vector3.zero, Quaternion.identity, 1);
        //Grow7(iterations, meshes, Vector3.zero, Quaternion.identity, 1);
        //Grow6(iterations, meshes, Vector3.zero, Quaternion.identity, 1);
        //Grow5(iterations, meshes, Vector3.zero, Quaternion.identity, 1);
        //Grow9(iterations, meshes, Vector3.zero, Quaternion.identity, 1);
        //Grow10(iterations, meshes, Vector3.zero, Quaternion.identity, 1);
        //Grow11(iterations, meshes, Vector3.zero, Quaternion.identity, 1);
        //Grow12(iterations, meshes, Vector3.zero, Quaternion.identity, 1);
        //Grow13(iterations, meshes, Vector3.zero, Quaternion.identity, 1);
        //Grow14(iterations, meshes, Vector3.zero, Quaternion.identity, 1);
        //Grow15(iterations, meshes, Vector3.zero, Quaternion.identity, 1);
        //Grow16(iterations, meshes, Vector3.zero, Quaternion.identity, 1);

        Mesh mesh = new Mesh();
        mesh.CombineMeshes(meshes.ToArray());
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;

    }
    private void Grow16(int num, List<CombineInstance> meshes, Vector3 pos, Quaternion rot, float scale)
    {
        int twist = 45;
        int twist2 = Random.Range(20, 30);
        int branch1 = Random.Range(0, 6);
        int branch2 = Random.Range(0, 6);
        int branch3 = Random.Range(0, 6);
        int branch4 = Random.Range(0, 6);
        int branch5 = Random.Range(-15, -5);
        int branch6 = Random.Range(5, 10);
        if (num <= 0) return;
        CombineInstance inst = new CombineInstance();
        inst.mesh = MakeCube();
        inst.transform = Matrix4x4.TRS(pos, rot, branchScaling * scale);
        meshes.Add(inst);
        num--;
        pos = inst.transform.MultiplyPoint(new Vector3(0, 1, 0));
        //Vector3 sidePos = inst.transform.MultiplyPoint(new Vector3(0, 2, 0));
        Quaternion rot1 = rot * Quaternion.Euler(0, twist2, -twist2);
        Quaternion rot2 = rot * Quaternion.Euler(0, twist2, -twist2);
        Quaternion rot3 = rot * Quaternion.Euler(0, twist, -twist);
        Quaternion rot4 = rot * Quaternion.Euler(0, -twist, -twist);
        Quaternion rot5 = rot * Quaternion.Euler(0, twist, twist);
        Quaternion rot6 = rot * Quaternion.Euler(0, -twist, twist);
        Quaternion rot7 = rot * Quaternion.Euler(0, 0, twist);
        Quaternion rot8 = rot * Quaternion.Euler(0, 0, -twist);

        scale *= .75f;

        Grow16(num, meshes, pos, rot1, scale);

        if (num == 9 & branch1 > 2)
        {
            Grow16(num, meshes, pos, rot2, scale);
        }
        if (num == 8 & branch2 > 2)
        {
            Grow16(num, meshes, pos, rot2, scale);
        }
        if (num == 7 & branch3 > 2)
        {
            Grow16(num, meshes, pos, rot2, scale);
        }
        if (num == 6 & branch4 > 2)
        {
            Grow16(num, meshes, pos, rot2, scale);

        }


        if (num == 1)
        {

            scale *= 1.5f;

            Grow16(num, meshes, pos, rot8, scale);
            Grow16(num, meshes, pos, rot7, scale);
            Grow16(num, meshes, pos, rot3, scale);
            Grow16(num, meshes, pos, rot4, scale);
            Grow16(num, meshes, pos, rot5, scale);
            Grow16(num, meshes, pos, rot6, scale);
        }

    }
    private void Grow15(int num, List<CombineInstance> meshes, Vector3 pos, Quaternion rot, float scale)
    {
        int twist = 45;
        int twist2 = Random.Range(50, 30);
        int branch1 = Random.Range(0, 6);
        int branch2 = Random.Range(0, 6);
        int branch3 = Random.Range(0, 6);
        int branch4 = Random.Range(0, 6);
        int branch5 = Random.Range(-25, -15);
        int branch6 = Random.Range(5, 10);
        if (num <= 0) return;
        CombineInstance inst = new CombineInstance();
        inst.mesh = MakeCube();
        inst.transform = Matrix4x4.TRS(pos, rot, branchScaling * scale);
        meshes.Add(inst);
        num--;
        pos = inst.transform.MultiplyPoint(new Vector3(0, 1, 0));
        //Vector3 sidePos = inst.transform.MultiplyPoint(new Vector3(0, 2, 0));
        Quaternion rot1 = rot * Quaternion.Euler(branch5, -branch5, branch6);
        Quaternion rot2 = rot * Quaternion.Euler(0, twist2, -twist2);
        Quaternion rot3 = rot * Quaternion.Euler(0, twist, -twist);
        Quaternion rot4 = rot * Quaternion.Euler(0, -twist, -twist);
        Quaternion rot5 = rot * Quaternion.Euler(0, twist, twist);
        Quaternion rot6 = rot * Quaternion.Euler(0, -twist, twist);
        Quaternion rot7 = rot * Quaternion.Euler(0, 0, twist);
        Quaternion rot8 = rot * Quaternion.Euler(0, 0, -twist);

        scale *= .75f;

        Grow15(num, meshes, pos, rot1, scale);

        if (num == 9 & branch1 > 2)
        {
            Grow15(num, meshes, pos, rot2, scale);
        }
        if (num == 8 & branch2 > 2)
        {
            Grow15(num, meshes, pos, rot2, scale);
        }
        if (num == 7 & branch3 > 2)
        {
            Grow15(num, meshes, pos, rot2, scale);
        }
        if (num == 6 & branch4 > 2)
        {
            Grow15(num, meshes, pos, rot2, scale);

        }


        if (num == 1)
        {

            scale *= 1.5f;

            Grow15(num, meshes, pos, rot8, scale);
            Grow15(num, meshes, pos, rot7, scale);
            Grow15(num, meshes, pos, rot3, scale);
            Grow15(num, meshes, pos, rot4, scale);
            Grow15(num, meshes, pos, rot5, scale);
            Grow15(num, meshes, pos, rot6, scale);
        }

    }
    private void Grow14(int num, List<CombineInstance> meshes, Vector3 pos, Quaternion rot, float scale)
    {
        int twist = 45;
        int twist2 = Random.Range(20, 30);
        int branch1 = Random.Range(0, 6);
        int branch2 = Random.Range(0, 6);
        int branch3 = Random.Range(0, 6);
        int branch4 = Random.Range(0, 6);
        int branch5 = Random.Range(-18, 0);

        if (num <= 0) return;
        CombineInstance inst = new CombineInstance();
        inst.mesh = MakeCube();
        inst.transform = Matrix4x4.TRS(pos, rot, branchScaling * scale);
        meshes.Add(inst);
        num--;
        pos = inst.transform.MultiplyPoint(new Vector3(0, 1, 0));
        //Vector3 sidePos = inst.transform.MultiplyPoint(new Vector3(0, 2, 0));
        Quaternion rot1 = rot * Quaternion.Euler(0, 0, branch5);
        Quaternion rot2 = rot * Quaternion.Euler(0, twist2, -twist2);
        Quaternion rot3 = rot * Quaternion.Euler(0, twist, -twist);
        Quaternion rot4 = rot * Quaternion.Euler(0, -twist, -twist);
        Quaternion rot5 = rot * Quaternion.Euler(0, twist, twist);
        Quaternion rot6 = rot * Quaternion.Euler(0, -twist, twist);
        Quaternion rot7 = rot * Quaternion.Euler(0, 0, twist);
        Quaternion rot8 = rot * Quaternion.Euler(0, 0, -twist);

        scale *= .75f;

        Grow14(num, meshes, pos, rot1, scale);

        if (num == 9 & branch1 > 2)
        {
            Grow14(num, meshes, pos, rot2, scale);
        }
        if (num == 8 & branch2 > 2)
        {
            Grow14(num, meshes, pos, rot2, scale);
        }
        if (num == 7 & branch3 > 2)
        {
            Grow14(num, meshes, pos, rot2, scale);
        }
        if (num == 6 & branch4 > 2)
        {
            Grow14(num, meshes, pos, rot2, scale);

        }


        if (num == 1)
        {

            scale *= 1.5f;

            Grow14(num, meshes, pos, rot8, scale);
            Grow14(num, meshes, pos, rot7, scale);
            Grow14(num, meshes, pos, rot3, scale);
            Grow14(num, meshes, pos, rot4, scale);
            Grow14(num, meshes, pos, rot5, scale);
            Grow14(num, meshes, pos, rot6, scale);
        }

    }
    private void Grow13(int num, List<CombineInstance> meshes, Vector3 pos, Quaternion rot, float scale)
    {
        int twist = 45;
        int twist2 = Random.Range(20, 30);
        int branch1 = Random.Range(0, 6);
        int branch2 = Random.Range(0, 6);
        int branch3 = Random.Range(0, 6);
        int branch4 = Random.Range(0, 6);
        int branch5 = Random.Range(0, 6);
        int branch6 = Random.Range(0, 6);
        int branch7 = Random.Range(0, 6);
        int branch8 = Random.Range(5, 25);
        if (num <= 0) return;
        CombineInstance inst = new CombineInstance();
        inst.mesh = MakeCube();
        inst.transform = Matrix4x4.TRS(pos, rot, branchScaling * scale);
        meshes.Add(inst);
        num--;
        pos = inst.transform.MultiplyPoint(new Vector3(0, 1, 0));
        //Vector3 sidePos = inst.transform.MultiplyPoint(new Vector3(0, 2, 0));
        Quaternion rot1 = rot * Quaternion.Euler(0, 0, branch8);
        Quaternion rot2 = rot * Quaternion.Euler(0, twist2, -twist2);
        Quaternion rot3 = rot * Quaternion.Euler(0, twist, -twist);
        Quaternion rot4 = rot * Quaternion.Euler(0, -twist, -twist);
        Quaternion rot5 = rot * Quaternion.Euler(0, twist, twist);
        Quaternion rot6 = rot * Quaternion.Euler(0, -twist, twist);
        Quaternion rot7 = rot * Quaternion.Euler(0, 0, twist);
        Quaternion rot8 = rot * Quaternion.Euler(0, 0, -twist);

        scale *= .75f;

        Grow13(num, meshes, pos, rot1, scale);

        if (num == 9 & branch1 > 2)
        {
            Grow13(num, meshes, pos, rot2, scale);
        }
        if (num == 8 & branch2 > 2)
        {
            Grow13(num, meshes, pos, rot2, scale);
        }
        if (num == 7 & branch3 > 2)
        {
            Grow13(num, meshes, pos, rot2, scale);
        }
        if (num == 6 & branch4 > 2)
        {
            Grow13(num, meshes, pos, rot2, scale);

        }


        if (num == 1)
        {

            scale *= 1.5f;

            Grow13(num, meshes, pos, rot8, scale);
            Grow13(num, meshes, pos, rot7, scale);
            Grow13(num, meshes, pos, rot3, scale);
            Grow13(num, meshes, pos, rot4, scale);
            Grow13(num, meshes, pos, rot5, scale);
            Grow13(num, meshes, pos, rot6, scale);
        }

    }
    private void Grow12(int num, List<CombineInstance> meshes, Vector3 pos, Quaternion rot, float scale)
    {
        int twist = 45;
        int twist2 = Random.Range(40, 50);
        int branch1 = Random.Range(0, 6);
        int branch2 = Random.Range(0, 6);
        int branch3 = Random.Range(0, 6);
        int branch4 = Random.Range(0, 6);
        int branch5 = Random.Range(-15, -5);
        int branch6 = Random.Range(5, 10);
        if (num <= 0) return;
        CombineInstance inst = new CombineInstance();
        inst.mesh = MakeCube();
        inst.transform = Matrix4x4.TRS(pos, rot, branchScaling * scale);
        meshes.Add(inst);
        num--;
        pos = inst.transform.MultiplyPoint(new Vector3(0, 1, 0));
        //Vector3 sidePos = inst.transform.MultiplyPoint(new Vector3(0, 2, 0));
        Quaternion rot1 = rot * Quaternion.Euler(0, 0, 0);
        Quaternion rot2 = rot * Quaternion.Euler(0, twist2, -twist2);
        Quaternion rot3 = rot * Quaternion.Euler(0, twist, -twist);
        Quaternion rot4 = rot * Quaternion.Euler(0, -twist, -twist);
        Quaternion rot5 = rot * Quaternion.Euler(0, twist, twist);
        Quaternion rot6 = rot * Quaternion.Euler(0, -twist, twist);
        Quaternion rot7 = rot * Quaternion.Euler(0, 0, twist);
        Quaternion rot8 = rot * Quaternion.Euler(0, 0, -twist);

        scale *= .75f;

        Grow12(num, meshes, pos, rot1, scale);

        if (num == 9 & branch1 > 2)
        {
            Grow12(num, meshes, pos, rot2, scale);
        }
        if (num == 8 & branch2 > 2)
        {
            Grow12(num, meshes, pos, rot2, scale);
        }
        if (num == 7 & branch3 > 2)
        {
            Grow12(num, meshes, pos, rot2, scale);
        }
        if (num == 6 & branch4 > 2)
        {
            Grow12(num, meshes, pos, rot2, scale);

        }


        if (num == 1)
        {

            scale *= 1.5f;

            Grow12(num, meshes, pos, rot8, scale);
            Grow12(num, meshes, pos, rot7, scale);
            Grow12(num, meshes, pos, rot3, scale);
            Grow12(num, meshes, pos, rot4, scale);
            Grow12(num, meshes, pos, rot5, scale);
            Grow12(num, meshes, pos, rot6, scale);
        }

    }
    private void Grow11(int num, List<CombineInstance> meshes, Vector3 pos, Quaternion rot, float scale)
    {
        int twist = 45;
        int twist2 = Random.Range(60, 90);
        int branch1 = Random.Range(0, 6);
        int branch2 = Random.Range(0, 6);
        int branch3 = Random.Range(0, 6);
        int branch4 = Random.Range(0, 6);
        int branch5 = Random.Range(-15, -5);
        int branch6 = Random.Range(5, 10);
        if (num <= 0) return;
        CombineInstance inst = new CombineInstance();
        inst.mesh = MakeCube();
        inst.transform = Matrix4x4.TRS(pos, rot, branchScaling * scale);
        meshes.Add(inst);
        num--;
        pos = inst.transform.MultiplyPoint(new Vector3(0, 1, 0));
        //Vector3 sidePos = inst.transform.MultiplyPoint(new Vector3(0, 2, 0));
        Quaternion rot1 = rot * Quaternion.Euler(branch5, -branch5, branch6);
        Quaternion rot2 = rot * Quaternion.Euler(0, twist2, -twist2);
        Quaternion rot3 = rot * Quaternion.Euler(0, twist, -twist);
        Quaternion rot4 = rot * Quaternion.Euler(0, -twist, -twist);
        Quaternion rot5 = rot * Quaternion.Euler(0, twist, twist);
        Quaternion rot6 = rot * Quaternion.Euler(0, -twist, twist);
        Quaternion rot7 = rot * Quaternion.Euler(0, 0, twist);
        Quaternion rot8 = rot * Quaternion.Euler(0, 0, -twist);

        scale *= .75f;

        Grow11(num, meshes, pos, rot1, scale);

        if (num == 9 & branch1 > 2)
        {
            Grow11(num, meshes, pos, rot2, scale);
        }
        if (num == 8 & branch2 > 2)
        {
            Grow11(num, meshes, pos, rot2, scale);
        }
        if (num == 7 & branch3 > 2)
        {
            Grow11(num, meshes, pos, rot2, scale);
        }
        if (num == 6 & branch4 > 2)
        {
            Grow11(num, meshes, pos, rot2, scale);

        }


        if (num == 1)
        {

            scale *= 1.5f;

            Grow11(num, meshes, pos, rot8, scale);
            Grow11(num, meshes, pos, rot7, scale);
            Grow11(num, meshes, pos, rot3, scale);
            Grow11(num, meshes, pos, rot4, scale);
            Grow11(num, meshes, pos, rot5, scale);
            Grow11(num, meshes, pos, rot6, scale);
        }

    }
    private void Grow10(int num, List<CombineInstance> meshes, Vector3 pos, Quaternion rot, float scale)
    {
        int twist = 45;
        int twist2 = Random.Range(-60, -30);
        int branch1 = Random.Range(0, 6);
        int branch2 = Random.Range(0, 6);
        int branch3 = Random.Range(0, 6);
        int branch4 = Random.Range(0, 6);
        int branch5 = Random.Range(-10, -5);

        if (num <= 0) return;
        CombineInstance inst = new CombineInstance();
        inst.mesh = MakeCube();
        inst.transform = Matrix4x4.TRS(pos, rot, branchScaling * scale);
        meshes.Add(inst);
        num--;
        pos = inst.transform.MultiplyPoint(new Vector3(0, 1, 0));
        //Vector3 sidePos = inst.transform.MultiplyPoint(new Vector3(0, 2, 0));
        Quaternion rot1 = rot * Quaternion.Euler(0, 0, branch5);
        Quaternion rot2 = rot * Quaternion.Euler(0, twist2, -twist2);
        Quaternion rot3 = rot * Quaternion.Euler(0, twist, -twist);
        Quaternion rot4 = rot * Quaternion.Euler(0, -twist, -twist);
        Quaternion rot5 = rot * Quaternion.Euler(0, twist, twist);
        Quaternion rot6 = rot * Quaternion.Euler(0, -twist, twist);
        Quaternion rot7 = rot * Quaternion.Euler(0, 0, twist);
        Quaternion rot8 = rot * Quaternion.Euler(0, 0, -twist);

        scale *= .75f;

        Grow10(num, meshes, pos, rot1, scale);

        if (num == 9 & branch1 > 2)
        {
            Grow10(num, meshes, pos, rot2, scale);
        }
        if (num == 8 & branch2 > 2)
        {
            Grow10(num, meshes, pos, rot2, scale);
        }
        if (num == 7 & branch3 > 2)
        {
            Grow10(num, meshes, pos, rot2, scale);
        }
        if (num == 6 & branch4 > 2)
        {
            Grow10(num, meshes, pos, rot2, scale);

        }


        if (num == 1)
        {

            scale *= 1.5f;

            Grow10(num, meshes, pos, rot8, scale);
            Grow10(num, meshes, pos, rot7, scale);
            Grow10(num, meshes, pos, rot3, scale);
            Grow10(num, meshes, pos, rot4, scale);
            Grow10(num, meshes, pos, rot5, scale);
            Grow10(num, meshes, pos, rot6, scale);
        }

    }
    private void Grow9(int num, List<CombineInstance> meshes, Vector3 pos, Quaternion rot, float scale)
    {
        int twist = 45;
        int twist2 = Random.Range(20, 30);
        int branch1 = Random.Range(0, 6);
        int branch2 = Random.Range(0, 6);
        int branch3 = Random.Range(0, 6);
        int branch4 = Random.Range(0, 6);
        int branch5 = Random.Range(0, 6);
        int branch6 = Random.Range(0, 6);
        int branch7 = Random.Range(0, 6);
        int branch8 = Random.Range(5, 10);
        if (num <= 0) return;
        CombineInstance inst = new CombineInstance();
        inst.mesh = MakeCube();
        inst.transform = Matrix4x4.TRS(pos, rot, branchScaling * scale);
        meshes.Add(inst);
        num--;
        pos = inst.transform.MultiplyPoint(new Vector3(0, 1, 0));
        //Vector3 sidePos = inst.transform.MultiplyPoint(new Vector3(0, 2, 0));
        Quaternion rot1 = rot * Quaternion.Euler(0, 0, branch8);
        Quaternion rot2 = rot * Quaternion.Euler(0, twist2, -twist2);
        Quaternion rot3 = rot * Quaternion.Euler(0, twist, -twist);
        Quaternion rot4 = rot * Quaternion.Euler(0, -twist, -twist);
        Quaternion rot5 = rot * Quaternion.Euler(0, twist, twist);
        Quaternion rot6 = rot * Quaternion.Euler(0, -twist, twist);
        Quaternion rot7 = rot * Quaternion.Euler(0, 0, twist);
        Quaternion rot8 = rot * Quaternion.Euler(0, 0, -twist);

        scale *= .75f;

        Grow9(num, meshes, pos, rot1, scale);

        if (num == 9 & branch1 > 2)
        {
            Grow9(num, meshes, pos, rot2, scale);
        }
        if (num == 8 & branch2 > 2)
        {
            Grow9(num, meshes, pos, rot2, scale);
        }
        if (num == 7 & branch3 > 2)
        {
            Grow9(num, meshes, pos, rot2, scale);
        }
        if (num == 6 & branch4 > 2)
        {
            Grow9(num, meshes, pos, rot2, scale);

        }


        if (num == 1)
        {

            scale *= 1.5f;

            Grow9(num, meshes, pos, rot8, scale);
            Grow9(num, meshes, pos, rot7, scale);
            Grow9(num, meshes, pos, rot3, scale);
            Grow9(num, meshes, pos, rot4, scale);
            Grow9(num, meshes, pos, rot5, scale);
            Grow9(num, meshes, pos, rot6, scale);
        }

    }


    private void Grow8(int num, List<CombineInstance> meshes, Vector3 pos, Quaternion rot, float scale)
    {
        int twist = 45;
        int twist2 = Random.Range(20, 30);
        int branch1 = Random.Range(0, 6);
        int branch2 = Random.Range(0, 6);
        int branch3 = Random.Range(0, 6);
        int branch4 = Random.Range(0, 6);
        int branch5 = Random.Range(-15, -5);
        int branch6 = Random.Range(5, 10);
        if (num <= 0) return;
        CombineInstance inst = new CombineInstance();
        inst.mesh = MakeCube();
        inst.transform = Matrix4x4.TRS(pos, rot, branchScaling * scale);
        meshes.Add(inst);
        num--;
        pos = inst.transform.MultiplyPoint(new Vector3(0, 1, 0));
        //Vector3 sidePos = inst.transform.MultiplyPoint(new Vector3(0, 2, 0));
        Quaternion rot1 = rot * Quaternion.Euler(0, 0, 0);
        Quaternion rot2 = rot * Quaternion.Euler(0, twist2, -twist2);
        Quaternion rot3 = rot * Quaternion.Euler(0, twist, -twist);
        Quaternion rot4 = rot * Quaternion.Euler(0, -twist, -twist);
        Quaternion rot5 = rot * Quaternion.Euler(0, twist, twist);
        Quaternion rot6 = rot * Quaternion.Euler(0, -twist, twist);
        Quaternion rot7 = rot * Quaternion.Euler(0, 0, twist);
        Quaternion rot8 = rot * Quaternion.Euler(0, 0, -twist);

        scale *= .75f;

        Grow8(num, meshes, pos, rot1, scale);

        if (num == 9 & branch1 > 2)
        {
            Grow8(num, meshes, pos, rot2, scale);
        }
        if (num == 8 & branch2 > 2)
        {
            Grow8(num, meshes, pos, rot2, scale);
        }
        if (num == 7 & branch3 > 2)
        {
            Grow8(num, meshes, pos, rot2, scale);
        }
        if (num == 6 & branch4 > 2)
        {
            Grow8(num, meshes, pos, rot2, scale);

        }


        if (num == 1)
        {

            scale *= 1.5f;

            Grow8(num, meshes, pos, rot8, scale);
            Grow8(num, meshes, pos, rot7, scale);
            Grow8(num, meshes, pos, rot3, scale);
            Grow8(num, meshes, pos, rot4, scale);
            Grow8(num, meshes, pos, rot5, scale);
            Grow8(num, meshes, pos, rot6, scale);
        }

    }
    private void Grow7(int num, List<CombineInstance> meshes, Vector3 pos, Quaternion rot, float scale)
    {
        int twist = 45;
        int twist2 = Random.Range(20, 30);
        int branch1 = Random.Range(0, 6);
        int branch2 = Random.Range(0, 6);
        int branch3 = Random.Range(0, 6);
        int branch4 = Random.Range(0, 6);
        int branch5 = Random.Range(-15, -5);
        int branch6 = Random.Range(5, 10);
        if (num <= 0) return;
        CombineInstance inst = new CombineInstance();
        inst.mesh = MakeCube();
        inst.transform = Matrix4x4.TRS(pos, rot, branchScaling * scale);
        meshes.Add(inst);
        num--;
        pos = inst.transform.MultiplyPoint(new Vector3(0, 1, 0));
        //Vector3 sidePos = inst.transform.MultiplyPoint(new Vector3(0, 2, 0));
        Quaternion rot1 = rot * Quaternion.Euler(branch5, -branch5, branch6);
        Quaternion rot2 = rot * Quaternion.Euler(0, twist2, -twist2);
        Quaternion rot3 = rot * Quaternion.Euler(0, twist, -twist);
        Quaternion rot4 = rot * Quaternion.Euler(0, -twist, -twist);
        Quaternion rot5 = rot * Quaternion.Euler(0, twist, twist);
        Quaternion rot6 = rot * Quaternion.Euler(0, -twist, twist);
        Quaternion rot7 = rot * Quaternion.Euler(0, 0, twist);
        Quaternion rot8 = rot * Quaternion.Euler(0, 0, -twist);

        scale *= .75f;

        Grow7(num, meshes, pos, rot1, scale);

        if (num == 9 & branch1 > 2)
        {
            Grow7(num, meshes, pos, rot2, scale);
        }
        if (num == 8 & branch2 > 2)
        {
            Grow7(num, meshes, pos, rot2, scale);
        }
        if (num == 7 & branch3 > 2)
        {
            Grow7(num, meshes, pos, rot2, scale);
        }
        if (num == 6 & branch4 > 2)
        {
            Grow7(num, meshes, pos, rot2, scale);

        }


        if (num == 1)
        {

            scale *= 1.5f;

            Grow7(num, meshes, pos, rot8, scale);
            Grow7(num, meshes, pos, rot7, scale);
            Grow7(num, meshes, pos, rot3, scale);
            Grow7(num, meshes, pos, rot4, scale);
            Grow7(num, meshes, pos, rot5, scale);
            Grow7(num, meshes, pos, rot6, scale);
        }

    }
    private void Grow6(int num, List<CombineInstance> meshes, Vector3 pos, Quaternion rot, float scale)
    {
        int twist = 45;
        int twist2 = Random.Range(20, 30);
        int branch1 = Random.Range(0, 6);
        int branch2 = Random.Range(0, 6);
        int branch3 = Random.Range(0, 6);
        int branch4 = Random.Range(0, 6);
        int branch5 = Random.Range(-10, -5);

        if (num <= 0) return;
        CombineInstance inst = new CombineInstance();
        inst.mesh = MakeCube();
        inst.transform = Matrix4x4.TRS(pos, rot, branchScaling * scale);
        meshes.Add(inst);
        num--;
        pos = inst.transform.MultiplyPoint(new Vector3(0, 1, 0));
        //Vector3 sidePos = inst.transform.MultiplyPoint(new Vector3(0, 2, 0));
        Quaternion rot1 = rot * Quaternion.Euler(0, 0, branch5);
        Quaternion rot2 = rot * Quaternion.Euler(0, twist2, -twist2);
        Quaternion rot3 = rot * Quaternion.Euler(0, twist, -twist);
        Quaternion rot4 = rot * Quaternion.Euler(0, -twist, -twist);
        Quaternion rot5 = rot * Quaternion.Euler(0, twist, twist);
        Quaternion rot6 = rot * Quaternion.Euler(0, -twist, twist);
        Quaternion rot7 = rot * Quaternion.Euler(0, 0, twist);
        Quaternion rot8 = rot * Quaternion.Euler(0, 0, -twist);

        scale *= .75f;

        Grow6(num, meshes, pos, rot1, scale);

        if (num == 9 & branch1 > 2)
        {
            Grow6(num, meshes, pos, rot2, scale);
        }
        if (num == 8 & branch2 > 2)
        {
            Grow6(num, meshes, pos, rot2, scale);
        }
        if (num == 7 & branch3 > 2)
        {
            Grow6(num, meshes, pos, rot2, scale);
        }
        if (num == 6 & branch4 > 2)
        {
            Grow6(num, meshes, pos, rot2, scale);

        }


        if (num == 1)
        {

            scale *= 1.5f;

            Grow6(num, meshes, pos, rot8, scale);
            Grow6(num, meshes, pos, rot7, scale);
            Grow6(num, meshes, pos, rot3, scale);
            Grow6(num, meshes, pos, rot4, scale);
            Grow6(num, meshes, pos, rot5, scale);
            Grow6(num, meshes, pos, rot6, scale);
        }

    }
    private void Grow5(int num, List<CombineInstance> meshes, Vector3 pos, Quaternion rot, float scale)
    {
        int twist = 45;
        int twist2 = Random.Range(20, 30);
        int branch1 = Random.Range(0, 6);
        int branch2 = Random.Range(0, 6);
        int branch3 = Random.Range(0, 6);
        int branch4 = Random.Range(0, 6);
        int branch5 = Random.Range(0, 6);
        int branch6 = Random.Range(0, 6);
        int branch7 = Random.Range(0, 6);
        int branch8 = Random.Range(5, 10);
        if (num <= 0) return;
        CombineInstance inst = new CombineInstance();
        inst.mesh = MakeCube();
        inst.transform = Matrix4x4.TRS(pos, rot, branchScaling * scale);
        meshes.Add(inst);
        num--;
        pos = inst.transform.MultiplyPoint(new Vector3(0, 1, 0));
        //Vector3 sidePos = inst.transform.MultiplyPoint(new Vector3(0, 2, 0));
        Quaternion rot1 = rot * Quaternion.Euler(0, 0, branch8);
        Quaternion rot2 = rot * Quaternion.Euler(0, twist2, -twist2);
        Quaternion rot3 = rot * Quaternion.Euler(0, twist, -twist);
        Quaternion rot4 = rot * Quaternion.Euler(0, -twist, -twist);
        Quaternion rot5 = rot * Quaternion.Euler(0, twist, twist);
        Quaternion rot6 = rot * Quaternion.Euler(0, -twist, twist);
        Quaternion rot7 = rot * Quaternion.Euler(0, 0, twist);
        Quaternion rot8 = rot * Quaternion.Euler(0, 0, -twist);

        scale *= .75f;

        Grow5(num, meshes, pos, rot1, scale);

        if (num == 9 & branch1 > 2)
        {
            Grow5(num, meshes, pos, rot2, scale);
        }
        if (num == 8 & branch2 > 2)
        {
            Grow5(num, meshes, pos, rot2, scale);
        }
        if (num == 7 & branch3 > 2)
        {
            Grow5(num, meshes, pos, rot2, scale);
        }
        if (num == 6 & branch4 > 2)
        {
            Grow5(num, meshes, pos, rot2, scale);

        }


        if (num == 1)
        {

            scale *= 1.5f;

            Grow5(num, meshes, pos, rot8, scale);
            Grow5(num, meshes, pos, rot7, scale);
            Grow5(num, meshes, pos, rot3, scale);
            Grow5(num, meshes, pos, rot4, scale);
            Grow5(num, meshes, pos, rot5, scale);
            Grow5(num, meshes, pos, rot6, scale);
        }

    }
    private void Grow4(int num, List<CombineInstance> meshes, Vector3 pos, Quaternion rot, float scale)
    {
        int twist = 45;
        int twist2 = Random.Range(20, 30);
        int branch1 = Random.Range(0, 6);
        int branch2 = Random.Range(0, 6);
        int branch3 = Random.Range(0, 6);
        int branch4 = Random.Range(0, 6);
        int branch5 = Random.Range(-15, -5);
        int branch6 = Random.Range(5, 10);
        if (num <= 0) return;
        CombineInstance inst = new CombineInstance();
        inst.mesh = MakeCube();
        inst.transform = Matrix4x4.TRS(pos, rot, branchScaling * scale);
        meshes.Add(inst);
        num--;
        pos = inst.transform.MultiplyPoint(new Vector3(0, 1, 0));
        //Vector3 sidePos = inst.transform.MultiplyPoint(new Vector3(0, 2, 0));
        Quaternion rot1 = rot * Quaternion.Euler(0, 0, 0);
        Quaternion rot2 = rot * Quaternion.Euler(0, twist2, -twist2);
        Quaternion rot3 = rot * Quaternion.Euler(0, twist, -twist);
        Quaternion rot4 = rot * Quaternion.Euler(0, -twist, -twist);
        Quaternion rot5 = rot * Quaternion.Euler(0, twist, twist);
        Quaternion rot6 = rot * Quaternion.Euler(0, -twist, twist);
        Quaternion rot7 = rot * Quaternion.Euler(0, 0, twist);
        Quaternion rot8 = rot * Quaternion.Euler(0, 0, -twist);

        scale *= .75f;

        Grow4(num, meshes, pos, rot1, scale);

        if (num == 9 & branch1 > 2)
        {
            Grow4(num, meshes, pos, rot2, scale);
        }
        if (num == 8 & branch2 > 2)
        {
            Grow4(num, meshes, pos, rot2, scale);
        }
        if (num == 7 & branch3 > 2)
        {
            Grow4(num, meshes, pos, rot2, scale);
        }
        if (num == 6 & branch4 > 2)
        {
            Grow4(num, meshes, pos, rot2, scale);

        }


        if (num == 1)
        {

            scale *= 1.5f;

            Grow4(num, meshes, pos, rot8, scale);
            Grow4(num, meshes, pos, rot7, scale);
            Grow4(num, meshes, pos, rot3, scale);
            Grow4(num, meshes, pos, rot4, scale);
            Grow4(num, meshes, pos, rot5, scale);
            Grow4(num, meshes, pos, rot6, scale);
        }

    }
    private void Grow3(int num, List<CombineInstance> meshes, Vector3 pos, Quaternion rot, float scale)
    {
        int twist = 45;
        int twist2 = Random.Range(20, 30);
        int branch1 = Random.Range(0, 6);
        int branch2 = Random.Range(0, 6);
        int branch3 = Random.Range(0, 6);
        int branch4 = Random.Range(0, 6);
        int branch5 = Random.Range(-15, -5);
        int branch6 = Random.Range(5, 10);
        if (num <= 0) return;
        CombineInstance inst = new CombineInstance();
        inst.mesh = MakeCube();
        inst.transform = Matrix4x4.TRS(pos, rot, branchScaling * scale);
        meshes.Add(inst);
        num--;
        pos = inst.transform.MultiplyPoint(new Vector3(0, 1, 0));
        //Vector3 sidePos = inst.transform.MultiplyPoint(new Vector3(0, 2, 0));
        Quaternion rot1 = rot * Quaternion.Euler(branch5, -branch5, branch6);
        Quaternion rot2 = rot * Quaternion.Euler(0, twist2, -twist2);
        Quaternion rot3 = rot * Quaternion.Euler(0, twist, -twist);
        Quaternion rot4 = rot * Quaternion.Euler(0, -twist, -twist);
        Quaternion rot5 = rot * Quaternion.Euler(0, twist, twist);
        Quaternion rot6 = rot * Quaternion.Euler(0, -twist, twist);
        Quaternion rot7 = rot * Quaternion.Euler(0, 0, twist);
        Quaternion rot8 = rot * Quaternion.Euler(0, 0, -twist);

        scale *= .75f;

        Grow3(num, meshes, pos, rot1, scale);

        if (num == 9 & branch1 > 2)
        {
            Grow3(num, meshes, pos, rot2, scale);
        }
        if (num == 8 & branch2 > 2)
        {
            Grow3(num, meshes, pos, rot2, scale);
        }
        if (num == 7 & branch3 > 2)
        {
            Grow3(num, meshes, pos, rot2, scale);
        }
        if (num == 6 & branch4 > 2)
        {
            Grow3(num, meshes, pos, rot2, scale);

        }


        if (num == 1)
        {

            scale *= 1.5f;

            Grow3(num, meshes, pos, rot8, scale);
            Grow3(num, meshes, pos, rot7, scale);
            Grow3(num, meshes, pos, rot3, scale);
            Grow3(num, meshes, pos, rot4, scale);
            Grow3(num, meshes, pos, rot5, scale);
            Grow3(num, meshes, pos, rot6, scale);
        }

    }
    private void Grow2(int num, List<CombineInstance> meshes, Vector3 pos, Quaternion rot, float scale)
    {
        int twist = 45;
        int twist2 = Random.Range(20, 30);
        int branch1 = Random.Range(0, 6);
        int branch2 = Random.Range(0, 6);
        int branch3 = Random.Range(0, 6);
        int branch4 = Random.Range(0, 6);
        int branch5 = Random.Range(-10, -5);

        if (num <= 0) return;
        CombineInstance inst = new CombineInstance();
        inst.mesh = MakeCube();
        inst.transform = Matrix4x4.TRS(pos, rot, branchScaling * scale);
        meshes.Add(inst);
        num--;
        pos = inst.transform.MultiplyPoint(new Vector3(0, 1, 0));
        //Vector3 sidePos = inst.transform.MultiplyPoint(new Vector3(0, 2, 0));
        Quaternion rot1 = rot * Quaternion.Euler(0, 0, branch5);
        Quaternion rot2 = rot * Quaternion.Euler(0, twist2, -twist2);
        Quaternion rot3 = rot * Quaternion.Euler(0, twist, -twist);
        Quaternion rot4 = rot * Quaternion.Euler(0, -twist, -twist);
        Quaternion rot5 = rot * Quaternion.Euler(0, twist, twist);
        Quaternion rot6 = rot * Quaternion.Euler(0, -twist, twist);
        Quaternion rot7 = rot * Quaternion.Euler(0, 0, twist);
        Quaternion rot8 = rot * Quaternion.Euler(0, 0, -twist);

        scale *= .75f;

        Grow2(num, meshes, pos, rot1, scale);

        if (num == 9 & branch1 > 2)
        {
            Grow2(num, meshes, pos, rot2, scale);
        }
        if (num == 8 & branch2 > 2)
        {
            Grow2(num, meshes, pos, rot2, scale);
        }
        if (num == 7 & branch3 > 2)
        {
            Grow2(num, meshes, pos, rot2, scale);
        }
        if (num == 6 & branch4 > 2)
        {
            Grow2(num, meshes, pos, rot2, scale);

        }


        if (num == 1)
        {

            scale *= 1.5f;

            Grow2(num, meshes, pos, rot8, scale);
            Grow2(num, meshes, pos, rot7, scale);
            Grow2(num, meshes, pos, rot3, scale);
            Grow2(num, meshes, pos, rot4, scale);
            Grow2(num, meshes, pos, rot5, scale);
            Grow2(num, meshes, pos, rot6, scale);
        }

    }
    private void Grow(int num, List<CombineInstance> meshes, Vector3 pos, Quaternion rot, float scale)
    {
        int twist = 45;
        int twist2 = Random.Range(20, 30);
        int branch1 = Random.Range(0, 6);
        int branch2 = Random.Range(0, 6);
        int branch3 = Random.Range(0, 6);
        int branch4 = Random.Range(0, 6);
        int branch5 = Random.Range(0, 6);
        int branch6 = Random.Range(0, 6);
        int branch7 = Random.Range(0, 6);
        int branch8 = Random.Range(5, 10);
        if (num <= 0) return;
        CombineInstance inst = new CombineInstance();
        inst.mesh = MakeCube();
        inst.transform = Matrix4x4.TRS(pos, rot, branchScaling * scale);
        meshes.Add(inst);
        num--;
        pos = inst.transform.MultiplyPoint(new Vector3(0, 1, 0));
        //Vector3 sidePos = inst.transform.MultiplyPoint(new Vector3(0, 2, 0));
        Quaternion rot1 = rot * Quaternion.Euler(0, 0, branch8);
        Quaternion rot2 = rot * Quaternion.Euler(0, twist2, -twist2);
        Quaternion rot3 = rot * Quaternion.Euler(0, twist, -twist);
        Quaternion rot4 = rot * Quaternion.Euler(0, -twist, -twist);
        Quaternion rot5 = rot * Quaternion.Euler(0, twist, twist);
        Quaternion rot6 = rot * Quaternion.Euler(0, -twist, twist);
        Quaternion rot7 = rot * Quaternion.Euler(0, 0, twist);
        Quaternion rot8 = rot * Quaternion.Euler(0, 0, -twist);

        scale *= .75f;

        Grow(num, meshes, pos, rot1, scale);

        if (num == 9 & branch1 > 2)
        {
            Grow(num, meshes, pos, rot2, scale);
        }
        if (num == 8 & branch2 > 2)
        {
            Grow(num, meshes, pos, rot2, scale);
        }
        if (num == 7 & branch3 > 2)
        {
            Grow(num, meshes, pos, rot2, scale);
        }
        if (num == 6 & branch4 > 2)
        {
            Grow(num, meshes, pos, rot2, scale);

        }


        if (num == 1)
        {

            scale *= 1.5f;

            Grow(num, meshes, pos, rot8, scale);
            Grow(num, meshes, pos, rot7, scale);
            Grow(num, meshes, pos, rot3, scale);
            Grow(num, meshes, pos, rot4, scale);
            Grow(num, meshes, pos, rot5, scale);
            Grow(num, meshes, pos, rot6, scale);
        }

    }
    private Mesh MakeCube()
    {

        List<Vector3> verts = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<Vector3> normals = new List<Vector3>();
        List<int> tris = new List<int>();
        //front
        verts.Add(new Vector3(-0.5f, 0, -0.5f));
        verts.Add(new Vector3(-0.5f, 1, -0.5f));
        verts.Add(new Vector3(+0.5f, 1, -0.5f));
        verts.Add(new Vector3(+0.5f, 0, -0.5f));
        normals.Add(new Vector3(0, 0, -1));
        normals.Add(new Vector3(0, 0, -1));
        normals.Add(new Vector3(0, 0, -1));
        normals.Add(new Vector3(0, 0, -1));

        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
        uvs.Add(new Vector2(1, 0));


        tris.Add(0);
        tris.Add(1);
        tris.Add(2);
        tris.Add(2);
        tris.Add(3);
        tris.Add(0);
        //back
        verts.Add(new Vector3(-0.5f, 0, +0.5f));
        verts.Add(new Vector3(+0.5f, 0, +0.5f));
        verts.Add(new Vector3(+0.5f, 1, +0.5f));
        verts.Add(new Vector3(-0.5f, 1, +0.5f));
        normals.Add(new Vector3(0, 0, +1));
        normals.Add(new Vector3(0, 0, +1));
        normals.Add(new Vector3(0, 0, +1));
        normals.Add(new Vector3(0, 0, +1));

        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
        uvs.Add(new Vector2(1, 0));


        tris.Add(4);
        tris.Add(5);
        tris.Add(6);
        tris.Add(6);
        tris.Add(7);
        tris.Add(4);
        //left
        verts.Add(new Vector3(-0.5f, 0, -0.5f));
        verts.Add(new Vector3(-0.5f, 0, +0.5f));
        verts.Add(new Vector3(-0.5f, 1, +0.5f));
        verts.Add(new Vector3(-0.5f, 1, -0.5f));
        normals.Add(new Vector3(-1, 0, 0));
        normals.Add(new Vector3(-1, 0, 0));
        normals.Add(new Vector3(-1, 0, 0));
        normals.Add(new Vector3(-1, 0, 0));

        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
        uvs.Add(new Vector2(1, 0));


        tris.Add(8);
        tris.Add(9);
        tris.Add(10);
        tris.Add(10);
        tris.Add(11);
        tris.Add(8);
        //Right
        verts.Add(new Vector3(+0.5f, 0, -0.5f));
        verts.Add(new Vector3(+0.5f, 1, -0.5f));
        verts.Add(new Vector3(+0.5f, 1, +0.5f));
        verts.Add(new Vector3(+0.5f, 0, +0.5f));
        normals.Add(new Vector3(+1, 0, 0));
        normals.Add(new Vector3(+1, 0, 0));
        normals.Add(new Vector3(+1, 0, 0));
        normals.Add(new Vector3(+1, 0, 0));

        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
        uvs.Add(new Vector2(1, 0));


        tris.Add(12);
        tris.Add(13);
        tris.Add(14);
        tris.Add(14);
        tris.Add(15);
        tris.Add(12);
        //top
        verts.Add(new Vector3(-0.5f, 1, -0.5f));
        verts.Add(new Vector3(-0.5f, 1, +0.5f));
        verts.Add(new Vector3(+0.5f, 1, +0.5f));
        verts.Add(new Vector3(+0.5f, 1, -0.5f));
        normals.Add(new Vector3(0, +1, 0));
        normals.Add(new Vector3(0, +1, 0));
        normals.Add(new Vector3(0, +1, 0));
        normals.Add(new Vector3(0, +1, 0));

        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
        uvs.Add(new Vector2(1, 0));


        tris.Add(16);
        tris.Add(17);
        tris.Add(18);
        tris.Add(18);
        tris.Add(19);
        tris.Add(16);
        //bottom
        verts.Add(new Vector3(-0.5f, 0, -0.5f));
        verts.Add(new Vector3(+0.5f, 0, -0.5f));
        verts.Add(new Vector3(+0.5f, 0, +0.5f));
        verts.Add(new Vector3(-0.5f, 0, +0.5f));
        normals.Add(new Vector3(0, -1, 0));
        normals.Add(new Vector3(0, -1, 0));
        normals.Add(new Vector3(0, -1, 0));
        normals.Add(new Vector3(0, -1, 0));

        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
        uvs.Add(new Vector2(1, 0));


        tris.Add(20);
        tris.Add(21);
        tris.Add(22);
        tris.Add(22);
        tris.Add(23);
        tris.Add(20);

        Mesh mesh = new Mesh();
        mesh.SetVertices(verts);
        mesh.SetUVs(0, uvs);
        mesh.SetNormals(normals);

        mesh.SetTriangles(tris, 0);

        return mesh;
    }
}

[CustomEditor(typeof(CoralFingers))]
public class CoralMeshEditor : Editor
{
    public override void OnInspectorGUI()
    {

        base.OnInspectorGUI();
        if (GUILayout.Button("GROW!"))
        {
            CoralFingers b = (target as CoralFingers);

            b.Build();
        }
    }


}