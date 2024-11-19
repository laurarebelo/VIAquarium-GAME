using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Draws line when we hold and drag our cursor.
/// </summary>
public class LineTool : DrawingTool
{
    public LineTool(DrawingRenderer drawingRenderer, ColorSelector colorSelector) : base(drawingRenderer, colorSelector)
    {
    }

    protected override Color GetDrawColor()
    => m_colorSelector.DrawColor;

    protected override Color GetPreviewColor()
    => m_colorSelector.DrawColor;

    /// <summary>
    /// This method calculates the positions of the pixels between start and end position to create a line.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="textureSize"></param>
    /// <param name="brushSize"></param>
    /// <returns></returns>
    protected override List<Vector2Int> GetDrawPositions(Vector2Int start, Vector2Int end, Vector2Int textureSize, int brushSize)
    {
        HashSet<Vector2Int> uniquePositions = new();
        foreach (Vector2Int linePosition in DrawingAlgorithms.BresenhamLine(start, end))
        {
            uniquePositions.UnionWith(ApplyBrush(linePosition, textureSize.x, textureSize.y, brushSize));
        }
        return new List<Vector2Int>(uniquePositions);
    }
}
