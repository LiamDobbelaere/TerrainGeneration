using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{
    public GameObject proceduralTerrain;
    public int seed = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    void Awake()
    {
        for (int y = 0; y < 16; y++)
        {
            for (int x = 0; x < 16; x++)
            {
                GenerateChunk(x, y);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateChunk(int x, int y)
    {
        var terrainObject = Instantiate(proceduralTerrain, new Vector3(x * 64f, 0f, y * 64f), proceduralTerrain.transform.rotation);
        var terrain= terrainObject.GetComponent<ProceduralTerrain>();

        terrain.chunkX = x;
        terrain.chunkY = y;
        terrain.seed = seed;
        terrain.xSize = 64;
        terrain.ySize = 64;

        terrain.Generate();
    }
}
