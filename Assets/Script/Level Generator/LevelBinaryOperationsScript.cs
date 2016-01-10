using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class LevelGeneratorHelper : MonoBehaviour
{
    /// <summary>
    /// Generates WxH map rectangle.
    /// </summary>
    /// <param name="rektW"></param>
    /// <param name="rektH"></param>
    /// <returns></returns>
    public static int[,] GetRekt(int rektW, int rektH)
    {
        int[,] result = new int[rektW, rektH];

        for (int i = 0; i < rektW; ++i)
        {
            for (int j = 0; j < rektH; ++j)
            {
                result[i, j] = 0;

                if (i == 0 || j == 0 || i == rektW - 1 || j == rektH - 1)
                {
                    result[i, j] = 1;
                }
            }
        }

        return result;
    }

    public static List<Vector2> GetPathInMap(int[,] map, int startX = 0, int startY = 0)
    {
        Debug.Assert(startX >= 0 && startX < map.GetLength(0) && startY >= 0 && startY < map.GetLength(1));

        List<Vector2> result = new List<Vector2>();

        HashSet<Vector2> visited = new HashSet<Vector2>();        
        Vector2 position = new Vector2(startX, startY);
        visited.Add(position);
        result.Add(position);

        while (true)
        {
            // get next non-visited position
            Vector2 right = position;
            right.x += 1;

            if (right.x < map.GetLength(0) && map[(int)right.x, (int)right.y] == 1 && visited.Contains(right) == false)
            {
                position = right;
                visited.Add(position);
                result.Add(position);
                continue;
            }

            Vector2 down = position;
            down.y += 1;

            if (down.y < map.GetLength(1) && map[(int)down.x, (int)down.y] == 1 && visited.Contains(down) == false)
            {
                position = down;
                visited.Add(position);
                result.Add(position);
                continue;
            }

            Vector2 left = position;
            left.x -= 1;
            
            if (left.x >= 0 && map[(int)left.x, (int)left.y] == 1 && visited.Contains(left) == false)
            {
                position = left;
                visited.Add(position);
                result.Add(position);
                continue;
            }

            Vector2 up = position;
            up.y -= 1;

            if (up.y >= 0 && map[(int)up.x, (int)up.y] == 1 && visited.Contains(up) == false)
            {
                position = up;
                visited.Add(position);
                result.Add(position);
                continue;
            }

            // if nothing found, discontinue loop
            break;
        }

        return result;
    }

    public static int[,] SubtractFromBottomRight(int[,] map, int rectW, int rectH)
    {
        int[,] result = new int[map.GetLength(0), map.GetLength(1)];

        Debug.Assert(rectW >= 2 && rectW < map.GetLength(0) - 1 && rectH >= 2 && rectH < map.GetLength(1) - 1);

        for (int i = map.GetLength(0) - rectW; i < rectW; ++i)
        {
            for (int j = map.GetLength(1) - rectH; j < rectH; ++i)
            {
                result[i, j] = 0;

                if (i == 0 || j == 0)
                {
                    result[i, j] = 1;
                }
            }
        }

        return result;
    }

    static string GetColoredElement(int value, string originalString)
    {
        string result = "";

        if (value == 1)
        {
            string colorStart = "<color=";
            string colorEnd = "</color>";

            colorStart += "green>";
            result = colorStart + originalString + colorEnd;
        }
        else
        {
            result = originalString;
        }

        return result;
    }

    /// <summary>
    /// Prints out map array to unity ui text element.
    /// </summary>
    /// <param name="map"></param>
    /// <param name="mapW"></param>
    /// <param name="mapH"></param>
    /// <param name="debugPrint"></param>
    public static void PrintMap(int[,] map, int mapW, int mapH, Text debugPrint)
    {
        string text = "";
        for (int i = 0; i < mapW; ++i)
        {
            for (int j = 0; j < mapH; ++j)
            {
                text += GetColoredElement(map[i, j], string.Format("{0}", map[i, j]));
            }
            text += "\n";
        }
        debugPrint.text = text;
    }
}
