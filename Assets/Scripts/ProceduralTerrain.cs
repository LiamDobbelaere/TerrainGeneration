using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralTerrain : MonoBehaviour
{
    public float noiseScale = 10f;
    public float heightScale = 20f;
    public Texture2D heightmap;
    public int xSize, ySize;
    private Vector3[] vertices;
    private Color[] colors;
    private int seed = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    void Awake()
    {
        heightmap = new Texture2D(xSize, ySize);

        GetComponent<MeshFilter>().sharedMesh = new Mesh();
        GetComponent<MeshFilter>().mesh.name = "Procedural Terrain";

        seed = Random.Range(0, 1000000);
        Generate();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            seed = Random.Range(0, 1000000);
            Generate();
        }
    }

    void Generate()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        vertices = new Vector3[(xSize + 1) * (ySize + 1)];
        colors = new Color[(xSize + 1) * (ySize + 1)];
        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                float height = Mathf.PerlinNoise(seed + ((float) x / xSize * noiseScale), seed + ((float) y / ySize * noiseScale));

                vertices[i] = new Vector3(x, height * heightScale, y);

                Debug.Log(height);
                heightmap.SetPixel(x, y, new Color(height, height, height, 1f));
                
                if (height < 0.2f)
                {
                    colors[i] = new Color(0, 0.7f, 0.7f);
                }
                else if (height < 0.3f)
                {
                    colors[i] = new Color(0.761f, 0.698f, 0.502f);
                }
                else if (height < 0.6f)
                {
                    colors[i] = new Color(0, 0.2f, 0);
                }
                else if (height < 0.7f)
                {
                    colors[i] = new Color(0.3f, 0.3f, 0.3f);
                }
                else
                {
                    colors[i] = Color.white;
                }
            }
        }
        mesh.vertices = vertices;
        mesh.colors = colors;

        int[] triangles = new int[xSize * ySize * 6];
        for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++)
        {
            for (int x = 0; x < xSize; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                triangles[ti + 5] = vi + xSize + 2;
            }
        }
        mesh.triangles = triangles;

        mesh.RecalculateNormals();

        heightmap.Apply();
    }

    void FlatShading()
    {

    }
}
