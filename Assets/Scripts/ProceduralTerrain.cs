using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralTerrain : MonoBehaviour
{
    public Texture2D heightmap;
    public bool useHeightmap = false;
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
        MeshData terrain = new MeshData();

        for (int y = 0; y < ySize - 1; y++)
        {
            for (int x = 0; x < xSize - 1; x++)
            {
                float amplitude = 20f;
                float noiseScale = 3f;
                bool flip = x % 2 == 1;
                if (y % 2 == 1)
                {
                    flip = !flip;
                }

                float z = Mathf.PerlinNoise(
                    seed + ((float)x / xSize * 2f) * noiseScale, 
                    seed + ((float)y / ySize * 2f) * noiseScale);
                float zNextX = Mathf.PerlinNoise(
                    seed + ((float)(x + 1) / xSize * 2f) * noiseScale, 
                    seed + ((float)y / ySize * 2f) * noiseScale);
                float zNextY = Mathf.PerlinNoise(
                    seed + ((float)x / xSize * 2f) * noiseScale, 
                    seed + ((float)(y + 1) / ySize * 2f) * noiseScale);
                float zNextXY = Mathf.PerlinNoise(
                    seed + ((float)(x + 1) / xSize * 2f) * noiseScale, 
                    seed + ((float)(y + 1) / ySize * 2f) * noiseScale);

                if (useHeightmap)
                {
                    z = heightmap.GetPixel(x, y).r;
                    zNextX = heightmap.GetPixel(x + 1, y).r;
                    zNextY = heightmap.GetPixel(x, y + 1).r;
                    zNextXY = heightmap.GetPixel(x + 1, y + 1).r;
                }

                Color c1 = Color.Lerp(GetColorForHeight(z), Color.white, Random.Range(0.0f, 0.12f));
                Color c2 = Color.Lerp(GetColorForHeight(z), Color.white, Random.Range(0.0f, 0.12f));

                if (flip)
                {
                    terrain.AddTriangle(
                        new Vector3(x, z * amplitude, y),
                        new Vector3(x, zNextY * amplitude, y + 1),
                        new Vector3(x + 1, zNextXY * amplitude, y + 1),
                        c1);
                    terrain.AddTriangle(
                        new Vector3(x, z * amplitude, y),
                        new Vector3(x + 1, zNextXY * amplitude, y + 1),
                        new Vector3(x + 1, zNextX * amplitude, y),
                        c2);
                }
                else
                {
                    terrain.AddTriangle(
                        new Vector3(x, z * amplitude, y),
                        new Vector3(x, zNextY * amplitude, y + 1),
                        new Vector3(x + 1, zNextX * amplitude, y),
                        c1);
                    terrain.AddTriangle(
                        new Vector3(x + 1, zNextX * amplitude, y),
                        new Vector3(x, zNextY * amplitude, y + 1),
                        new Vector3(x + 1, zNextXY * amplitude, y + 1),
                        c2);
                }
            }
        }

        GetComponent<MeshFilter>().mesh = terrain.ToMesh();
    }

    private Color GetColorForHeight(float height)
    {
        if (height < 0.2f)
        {
            return new Color(0, 0.7f, 0.7f);
        }
        else if (height < 0.3f)
        {
            return new Color(0.761f, 0.698f, 0.502f);
        }
        else if (height < 0.6f)
        {
            return new Color(0, 0.2f, 0);
        }
        else if (height < 0.7f)
        {
            return new Color(0.3f, 0.3f, 0.3f);
        }
        else
        {
            return Color.white;
        }
    }
}
