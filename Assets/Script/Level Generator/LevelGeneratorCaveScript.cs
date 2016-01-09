using UnityEngine;
using System.Collections;
using System;

public class LevelGeneratorCaveScript : MonoBehaviour
{
    public bool DrawGizmos = false;
    public int Width;
    public int Height;

    public string Seed;
    public bool UseRandomSeed;

    [Range(0, 100)]
    public int RandomFillPercent;

    [Range(0, 10)]
    public int SmoothIterations;

    int[,] Map;

    void Start()
    {
        GenerateMap();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GenerateMap();
        }
    }

    void GenerateMap()
    {
        Map = new int[Width, Height];
        RandomFillMap();

        for (int i = 0; i < SmoothIterations; i++)
        {
            SmoothMap();
        }

        MeshGeneratorScript meshGen = GetComponent<MeshGeneratorScript>();
        meshGen.GenerateMesh(Map, 1.0f);
    }
    
    void RandomFillMap()
    {
        if (UseRandomSeed)
        {
            Seed = Time.time.ToString();
        }

        System.Random pseudoRandom = new System.Random(Seed.GetHashCode());

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (x == 0 || x == Width - 1 || y == 0 || y == Height - 1)
                {
                    Map[x, y] = 1;
                }
                else {
                    Map[x, y] = (pseudoRandom.Next(0, 100) < RandomFillPercent) ? 1 : 0;
                }
            }
        }
    }

    void SmoothMap()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y);

                if (neighbourWallTiles > 4)
                {
                    Map[x, y] = 1;
                }
                else if (neighbourWallTiles < 4)
                {
                    Map[x, y] = 0;
                }
            }
        }
    }

    int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < Width && neighbourY >= 0 && neighbourY < Height)
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        wallCount += Map[neighbourX, neighbourY];
                    }
                }
                else {
                    wallCount++;
                }
            }
        }

        return wallCount;
    }

    void OnDrawGizmos()
    {
        if (DrawGizmos == true)
        {
            if (Map != null)
            {
                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        Gizmos.color = (Map[x, y] == 1) ? Color.black : Color.white;
                        Vector3 pos = new Vector3(-Width / 2 + x + .5f, 0, -Height / 2 + y + .5f);
                        Gizmos.DrawCube(pos, Vector3.one);
                    }
                }
            }
        }
    }
}