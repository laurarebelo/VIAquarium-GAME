using UnityEngine;

/// <summary>
/// Brush tool implemenation. At the moment it is just sets the color to the DrawColor from the ColorSelector. 
/// </summary>
public class BrushTool : DrawingTool
{
    ColorSelector m_colorSelector;

    public BrushTool(DrawingRenderer drawingRenderer, ColorSelector colorSelector) : base(drawingRenderer)
    {
        m_colorSelector = colorSelector;
    }

    protected override Color GetDrawColor()
    => m_colorSelector.DrawColor;

    protected override Color GetPreviewColor()
    => m_colorSelector.DrawColor;
}
