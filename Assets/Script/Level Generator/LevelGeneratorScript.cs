using UnityEngine;
using System.Collections.Generic;

public class LevelGeneratorScript : MonoBehaviour
{
    public float MapScale = 4.0f;
    public int MapSize = 3;
    public int TileSize = 10;
    public RoadElementScript RoadElementGameObject;
    public bool DrawGizmos = false;

    [Range(0.0f, 1.0f)]
    public float GizmoAlpha = 0.2f;

    Dictionary<Vector2, RoadElementScript> MapGraph = new Dictionary<Vector2, RoadElementScript>();
    LinkedList<RoadElementScript> MapList = new LinkedList<RoadElementScript>();

    void Start()
    {
        // in-game instantiation
        GenerateMap();

        // scale up
        transform.localScale = new Vector3(MapScale, 1.0f, MapScale);
    }

    public void GenerateMap()
    {
        ClearMap();

        for (int i = 0; i < MapSize; ++i)
        {
            GenerateAt(i, 0, TileSize);
        }

        for (int i = 0; i < MapSize; ++i)
        {
            GenerateAt(MapSize - 1, i, TileSize);
        }

        for (int i = MapSize - 1; i >= 0; --i)
        {
            GenerateAt(i, MapSize - 1, TileSize);
        }

        for (int i = MapSize - 1; i >= 0; --i)
        {
            GenerateAt(0, i, TileSize);
        }

        UpdateRoadElements(MapList);
    }

    void ClearMap()
    {
        foreach (var element in MapList)
        {
            DestroyImmediate(element.gameObject);
        }

        MapList.Clear();
        MapGraph.Clear();
    }

    void GenerateAt(int mapX, int mapY, int size)
    {
        if (ContainsObjectAtPosition(mapX, mapY) == false)
        {
            RoadElementScript element = GenerateRoadElement(mapX, mapY, size);
            MapList.AddFirst(element);
        }
    }

    bool ContainsObjectAtPosition(int mapX, int mapY)
    {
        bool result = false;

        Vector2 mapPosition = new Vector2((float)mapX, (float)mapY);
        result = MapGraph.ContainsKey(mapPosition);

        return result;
    }

    void UpdateRoadElements(LinkedList<RoadElementScript> list)
    {
        LinkedListNode<RoadElementScript> iter = null;
        for (iter = list.First; iter != null; iter = iter.Next)
        {
            RoadElementScript prevElement = null;
            if (iter == list.First)
            {
                prevElement = list.Last.Value;
            }
            else
            {
                prevElement = iter.Previous.Value;
            }

            RoadElementScript nextElement = null;
            if (iter == list.Last)
            {
                nextElement = list.First.Value;
            }
            else
            {
                nextElement = iter.Next.Value;
            }

            RoadElementScript currentElement = iter.Value;
            currentElement.Previous = prevElement;
            currentElement.Next = nextElement;

            currentElement.UpdateShape();
        }
    }

    RoadElementScript GenerateRoadElement(int mapX, int mapY, int size)
    {
        RoadElementScript result = null;

        Vector2 mapPosition = new Vector2((float)mapX, (float)mapY);

        Vector3 tilePositionVector3 = new Vector3((float)mapX * size, -1.0f, (float)mapY * size);
        result = Instantiate(RoadElementGameObject);
        result.transform.position = tilePositionVector3;
        result.transform.parent = transform;

        MapGraph[mapPosition] = result;

        return result;
    }

    void OnDrawGizmos()
    {
        if (DrawGizmos == true)
        {
            if (MapList.Count > 0)
            {
                foreach (RoadElementScript element in MapList)
                {
                    Vector3 pos = element.transform.position;
                    Color gizmoColor = Color.blue;

                    Vector3 difference = element.Next.transform.position - element.Previous.transform.position;
                    Vector3 forwardVector3 = element.transform.position - element.Previous.transform.position;

                    float angle = Vector3.Angle(difference, forwardVector3);
                    if (Mathf.Abs(angle) > 0.0f)
                    {
                        gizmoColor = Color.green;
                    }
                    gizmoColor.a = GizmoAlpha;

                    Gizmos.color = gizmoColor;

                    Vector3 size = new Vector3(TileSize, TileSize, TileSize);
                    size *= 4;
                    Gizmos.DrawCube(pos, size);
                }
            }
        }
    }
}
