using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstraction of a Drawing tool. It is responsible for allowing different tools to draw on the canvas.
/// </summary>
public abstract class DrawingTool
{
    //Reference to the DrawingRenderer so that we can apply changes to the canvas
    protected DrawingRenderer _drawingRenderer;

    /// <summary>
    /// Constructor allows us to assign the DrawingRenderer
    /// </summary>
    /// <param name="drawingRenderer"></param>
    public DrawingTool(DrawingRenderer drawingRenderer)
    {
        _drawingRenderer = drawingRenderer;
    }

    /// <summary>
    /// Applies changes to the canvas based on the start and end positions
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    public virtual void ApplyToDrawing(Vector2 start, Vector2 end)
    {
        List<Vector2Int> pixelPositions = GetPixelPositions(start, end);
        _drawingRenderer.DrawOnCanvas(pixelPositions, GetDrawColor());
    }

    /// <summary>
    /// Allows us to create a preview of the drawing tool
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    public virtual void CreatePreview(Vector2 start, Vector2 end)
    {
        List<Vector2Int> pixelPositions = GetPixelPositions(start, end);
        _drawingRenderer.DrawPreview(pixelPositions, GetPreviewColor());
    }

    /// <summary>
    /// Returns a list of pixel positions based on the start and end positions
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    protected virtual List<Vector2Int> GetPixelPositions(Vector2 start, Vector2 end)
    {
        Vector2Int startTexturePosition = _drawingRenderer.CalculateTexturePosition(start);
        Vector2Int endTexturePosition = _drawingRenderer.CalculateTexturePosition(end);
        return GetDrawPositions(startTexturePosition, endTexturePosition, _drawingRenderer.TextureSize, _drawingRenderer.BrushSize);
    }

    /// <summary>
    /// Returns a list of pixel positions based on the start and end positions. We will see it overrided on the Line and Rectangle tools
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="textureSize"></param>
    /// <param name="brushSize"></param>
    /// <returns></returns>
    protected virtual List<Vector2Int> GetDrawPositions(Vector2Int start, Vector2Int end, Vector2Int textureSize, int brushSize)
    {
        return ApplyBrush(end, textureSize.x, textureSize.y, brushSize);
    }

    /// <summary>
    /// Color to be used when drawing on the canvas. Eraser tool modifies this. I will want to refactor this in some way as it is here only because of 1 tool.
    /// </summary>
    /// <returns></returns>
    protected abstract Color GetDrawColor();

    protected abstract Color GetPreviewColor();

    /// <summary>
    /// Applies the brush to the canvas. I will want to refactor this to a separate class as DrawingTool abstraction "knows to much" at the moment.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="textureWidth"></param>
    /// <param name="textureHeight"></param>
    /// <param name="brushSize"></param>
    /// <returns></returns>
    protected List<Vector2Int> ApplyBrush(Vector2Int position, int textureWidth, int textureHeight, int brushSize)
    {
        List<Vector2Int> pixels = new List<Vector2Int>();
        int startX = position.x - brushSize / 2;
        int startY = position.y - brushSize / 2;
        int endX = startX + brushSize;
        int endY = startY + brushSize;

        for (int x = startX; x < endX; x++)
        {
            for (int y = startY; y < endY; y++)
            {
                Vector2Int pixelPos = new Vector2Int(x, y);
                if (pixelPos.x >= 0 && pixelPos.x < textureWidth &&
                    pixelPos.y >= 0 && pixelPos.y < textureHeight)
                {
                    pixels.Add(pixelPos);
                }
            }
        }
        return pixels;
    }
}
