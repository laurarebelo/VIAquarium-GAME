using System.Collections.Generic;
using UnityEngine;

public class ColorPickerTool : DrawingTool
{
    Color m_previewColor = new Color(1, 1, 1, 0.6f);
    public ColorPickerTool(DrawingRenderer drawingRenderer, ColorSelector colorSelector) : base(drawingRenderer, colorSelector)
    {
    }

    protected override Color GetDrawColor()
    => m_colorSelector.DrawColor;

    protected override Color GetPreviewColor()
    => m_colorSelector.DrawColor;

    public override void ApplyToDrawing(Vector2 start, Vector2 end)
    {
        List<Vector2Int> pixelPositions = GetPixelPositions(end, end);
        m_colorSelector.SetColor(_drawingRenderer.GetPixelColor(pixelPositions[0]));
    }

    public override void CreatePreview(Vector2 start, Vector2 end)
    {
        List<Vector2Int> pixelPositions = GetPixelPositions(end, end);
        //m_colorSelector.SetColor(_drawingRenderer.GetPixelColor(pixelPositions[0]));
        _drawingRenderer.DrawPreview(pixelPositions, m_previewColor);
    }
}
