using System.Collections.Generic;
using UnityEngine;

public static class DrawingAlgorithms
{
    /// <summary>
    /// Helps us draw a pixel line between two points.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    //https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm
    public static List<Vector2Int> BresenhamLine(Vector2Int start, Vector2Int end)
    {
        List<Vector2Int> points = new List<Vector2Int>();
        int x = start.x;
        int y = start.y;
        int dx = Mathf.Abs(end.x - start.x);
        int dy = Mathf.Abs(end.y - start.y);
        int sx = start.x < end.x ? 1 : -1;
        int sy = start.y < end.y ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
            points.Add(new Vector2Int(x, y));
            if (x == end.x && y == end.y) break;
            int e2 = 2 * err;
            if (e2 > -dy)
            {
                err -= dy;
                x += sx;
            }
            if (e2 < dx)
            {
                err += dx;
                y += sy;
            }
        }
        return points;
    }
}
