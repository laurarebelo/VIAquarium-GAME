using System.Collections.Generic;
using UnityEngine;

namespace Drawing.DrawingTools
{
    public class BucketTool : DrawingTool
    {
        public BucketTool(DrawingRenderer drawingRenderer, ColorSelector colorSelector) : base(drawingRenderer, colorSelector)
        {
        }

        protected override Color GetDrawColor()
            => m_colorSelector.DrawColor;

        protected override Color GetPreviewColor()
            => m_colorSelector.DrawColor;

        public override void ApplyToDrawing(Vector2 start, Vector2 end)
        {
            Vector2Int startPixel = _drawingRenderer.CalculateTexturePosition(start);
            if (!_drawingRenderer.CanDrawAt(startPixel))
                return;
            Color targetColor = _drawingRenderer.GetPixelColor(startPixel);
            if (AreColorsEqual(targetColor, GetDrawColor()))
                return;
            FloodFill(startPixel, targetColor);
        }

        private void FloodFill(Vector2Int startPixel, Color targetColor)
        {
            Queue<Vector2Int> pixelQueue = new();
            HashSet<Vector2Int> visited = new();
            pixelQueue.Enqueue(startPixel);
            visited.Add(startPixel);

            while (pixelQueue.Count > 0)
            {
                Vector2Int currentPixel = pixelQueue.Dequeue();
                _drawingRenderer.DrawOnCanvas(new List<Vector2Int> { currentPixel }, GetDrawColor());
                foreach (Vector2Int neighbor in GetNeighbors(currentPixel))
                {
                    if (visited.Contains(neighbor) || !_drawingRenderer.CanDrawAt(neighbor))
                        continue;
                    if (AreColorsEqual(_drawingRenderer.GetPixelColor(neighbor), targetColor))
                    {
                        pixelQueue.Enqueue(neighbor);
                        visited.Add(neighbor);
                    }
                }
            }
        }

        private IEnumerable<Vector2Int> GetNeighbors(Vector2Int pixel)
        {
            return new List<Vector2Int>
            {
                new(pixel.x + 1, pixel.y),
                new(pixel.x - 1, pixel.y),
                new(pixel.x, pixel.y + 1),
                new(pixel.x, pixel.y - 1)
            };
        }

        private bool AreColorsEqual(Color color1, Color color2)
        {
            return Mathf.Approximately(color1.r, color2.r) &&
                   Mathf.Approximately(color1.g, color2.g) &&
                   Mathf.Approximately(color1.b, color2.b) &&
                   Mathf.Approximately(color1.a, color2.a);
        }
    }
}