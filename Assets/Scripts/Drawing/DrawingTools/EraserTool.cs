using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Eraser tool implementation. It sets the color to clear (transparent) end ensures that we override the canvas color rather than blending new color
/// with what is already there (like the BrushTool does).
/// </summary>
public class EraserTool : DrawingTool
{
    Color m_previewColor = new Color(1, 1, 1, 0.6f);


    public EraserTool(DrawingRenderer drawingRenderer) : base(drawingRenderer)
    {
    }

    protected override Color GetDrawColor()
    => Color.clear;

    protected override Color GetPreviewColor()
    => m_previewColor;

    public override void ApplyToDrawing(Vector2 start, Vector2 end)
    {
        List<Vector2Int> pixelPositions = GetPixelPositions(start, end);
        //Notice the "fasle" paremeter at the end.
        _drawingRenderer.DrawOnCanvas(pixelPositions, GetDrawColor(), false);
    }
}
