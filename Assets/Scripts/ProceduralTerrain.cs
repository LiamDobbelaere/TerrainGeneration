using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralTerrain : MonoBehaviour
{
    public Texture2D heightmap;
    public bool useHeightmap = false;
    public int xSize, ySize;
    public float chunkX, chunkY;
    public float maxHeight = 50f;
    public int seed = -1;

    // Start is called before the first frame update
    void Start()
    {

    }

    void Awake()
    {
        if (!useHeightmap)
        {
            this.heightmap = new Texture2D(xSize - 1, ySize - 1);
        }

        GetComponent<MeshFilter>().sharedMesh = new Mesh();
        GetComponent<MeshFilter>().mesh.name = "Procedural Terrain";

        if (seed == -1)
        {
            seed = Random.Range(0, 100000);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Generate()
    {
        int increment = 2;

        MeshData terrain = new MeshData();
        float[] heights = new float[xSize * ySize];
        float[,] noiseMap = this.GenerateNoiseMap(seed, chunkX, chunkY, xSize + 1, ySize + 1, 50f, 4, 0.5f, 2f);

        for (int y = 0; y < ySize; y += increment)
        {
            for (int x = 0; x < xSize; x += increment)
            {
                bool flip = x % 2 == 1;
                if (y % 2 == 1)
                {
                    flip = !flip;
                }

                float z = noiseMap[x, y];
                float zNextX = noiseMap[x + increment, y];
                float zNextY = noiseMap[x, y + increment];
                float zNextXY = noiseMap[x + increment, y + increment];

                heights[x + y * (ySize - 1)] = z;

                if (useHeightmap)
                {
                    z = heightmap.GetPixel(x, y).r;
                    zNextX = heightmap.GetPixel(x + increment, y).r;
                    zNextY = heightmap.GetPixel(x, y + increment).r;
                    zNextXY = heightmap.GetPixel(x + increment, y + increment).r;
                }

                Color c1 = Color.Lerp(GetColorForHeight(z), Color.white, Random.Range(0.0f, 0.12f));
                Color c2 = Color.Lerp(GetColorForHeight(z), Color.white, Random.Range(0.0f, 0.12f));

                if (flip)
                {
                    terrain.AddTriangle(
                        new Vector3(x, z * maxHeight, y),
                        new Vector3(x, zNextY * maxHeight, y + increment),
                        new Vector3(x + increment, zNextXY * maxHeight, y + increment),
                        c1);
                    terrain.AddTriangle(
                        new Vector3(x, z * maxHeight, y),
                        new Vector3(x + increment, zNextXY * maxHeight, y + increment),
                        new Vector3(x + increment, zNextX * maxHeight, y),
                        c2);
                }
                else
                {
                    terrain.AddTriangle(
                        new Vector3(x, z * maxHeight, y),
                        new Vector3(x, zNextY * maxHeight, y + increment),
                        new Vector3(x + increment, zNextX * maxHeight, y),
                        c1);
                    terrain.AddTriangle(
                        new Vector3(x + increment, zNextX * maxHeight, y),
                        new Vector3(x, zNextY * maxHeight, y + increment),
                        new Vector3(x + increment, zNextXY * maxHeight, y + increment),
                        c2);
                }
            }
        }

        GetComponent<MeshFilter>().mesh = terrain.ToMesh();

        this.heightmap.SetPixels(heights.Select(height => new Color(height, height, height, 1f)).ToArray());
        this.heightmap.Apply();
    }

    private Color GetColorForHeight(float height)
    {
        if (height < 0.3f)
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

    private float[,] GenerateNoiseMap(
        int seed, float chunkX, float chunkY, int width, int height, 
        float scale, int octaves, float persistence, float lacunarity)
    {
        float[,] noiseMap = new float[width, height];

        float maxNoiseHeight = 1f;
        float minNoiseHeight = -1f;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float amplitude = 1f;
                float frequency = 1f;
                float noiseHeight = 0f;

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x + chunkX * xSize) / scale * frequency;
                    float sampleY = (y + chunkY * ySize) / scale * frequency;

                    float perlin = Mathf.PerlinNoise(seed + sampleX, seed + sampleY) * 2f - 1f;
                    noiseHeight += perlin * amplitude;

                    amplitude *= persistence;
                    frequency *= lacunarity;
                }

                noiseMap[x, y] = noiseHeight;
            }
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }

        return noiseMap;
    }
}
