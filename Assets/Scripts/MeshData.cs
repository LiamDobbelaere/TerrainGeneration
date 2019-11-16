using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Triangle
{
    public Vector3 x;
    public Vector3 y;
    public Vector3 z;
    public Color color;

    public Triangle(Vector3 x, Vector3 y, Vector3 z, Color color)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.color = color;
    }
}

public class MeshData
{
    private List<Triangle> triangles;

    public MeshData()
    {
        triangles = new List<Triangle>();
    }

    public void AddTriangle(Vector3 x, Vector3 y, Vector3 z, Color color)
    {
        triangles.Add(new Triangle(x, y, z, color));
    }

    public Mesh ToMesh()
    {
        Mesh mesh = new Mesh();

        Vector3[] verts = new Vector3[this.triangles.Count * 3];
        Color[] cols = new Color[this.triangles.Count * 3];
        int[] tris = new int[this.triangles.Count * 3];

        for (int arrayIndex = 0, i = 0; i < this.triangles.Count; i++, arrayIndex += 3)
        {
            Triangle triangle = this.triangles[i];

            verts[arrayIndex] = triangle.x;
            verts[arrayIndex + 1] = triangle.y;
            verts[arrayIndex + 2] = triangle.z;

            cols[arrayIndex] = triangle.color;
            cols[arrayIndex + 1] = triangle.color;
            cols[arrayIndex + 2] = triangle.color;

            tris[arrayIndex] = arrayIndex;
            tris[arrayIndex + 1] = arrayIndex + 1;
            tris[arrayIndex + 2] = arrayIndex + 2;
        }

        mesh.vertices = verts;
        mesh.colors = cols;
        mesh.triangles = tris;

        mesh.RecalculateNormals();

        return mesh;
    }
}

