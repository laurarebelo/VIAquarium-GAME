using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Draws a rectangle when we hold and drag our cursor between two points.
/// </summary>
public class RectangleTool : DrawingTool
{
    public RectangleTool(DrawingRenderer drawingRenderer, ColorSelector colorSelector) : base(drawingRenderer, colorSelector)
    {
        m_colorSelector = colorSelector;
    }

    protected override Color GetDrawColor()
    => m_colorSelector.DrawColor;

    protected override Color GetPreviewColor()
    => m_colorSelector.DrawColor;

    /// <summary>
    /// Draws 4 lines to create a rectangle. We use Hashset to avoid duplicates.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="textureSize"></param>
    /// <param name="brushSize"></param>
    /// <returns></returns>
    protected override List<Vector2Int> GetDrawPositions(Vector2Int start, Vector2Int end, Vector2Int textureSize, int brushSize)
    {
        HashSet<Vector2Int> uniquePositions = new HashSet<Vector2Int>();

        // Calculate the corners of the rectangle
        Vector2Int topLeft = new Vector2Int(Mathf.Min(start.x, end.x), Mathf.Max(start.y, end.y));
        Vector2Int bottomRight = new Vector2Int(Mathf.Max(start.x, end.x), Mathf.Min(start.y, end.y));

        // Draw the four sides of the rectangle
        uniquePositions.UnionWith(DrawLine(topLeft, new Vector2Int(bottomRight.x, topLeft.y), textureSize, brushSize));
        uniquePositions.UnionWith(DrawLine(new Vector2Int(bottomRight.x, topLeft.y), bottomRight, textureSize, brushSize));
        uniquePositions.UnionWith(DrawLine(bottomRight, new Vector2Int(topLeft.x, bottomRight.y), textureSize, brushSize));
        uniquePositions.UnionWith(DrawLine(new Vector2Int(topLeft.x, bottomRight.y), topLeft, textureSize, brushSize));

        return new List<Vector2Int>(uniquePositions);
    }

    /// <summary>
    /// Generates a line. We use Hashset to avoid duplicates.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="textureSize"></param>
    /// <param name="brushSize"></param>
    /// <returns></returns>
    private List<Vector2Int> DrawLine(Vector2Int start, Vector2Int end, Vector2Int textureSize, int brushSize)
    {
        HashSet<Vector2Int> uniquePositions = new HashSet<Vector2Int>();
        foreach (Vector2Int linePosition in DrawingAlgorithms.BresenhamLine(start, end))
        {
            uniquePositions.UnionWith(ApplyBrush(linePosition, textureSize.x, textureSize.y, brushSize));
        }
        return new List<Vector2Int>(uniquePositions);
    }
}
