using System.Collections.Generic;
using UnityEngine;
using Color = UnityEngine.Color;

public class DrawingRenderer : MonoBehaviour
{
    // Texture that we render to
    [SerializeField] private RenderTexture m_renderTexture;

    // Preview texture that we render to
    [SerializeField] private RenderTexture m_previewRenderTexture;
    [SerializeField] private RenderTexture m_outlineRenderTexture;

    [SerializeField] private Vector2Int m_textureSize = new(32, 32);

    //Texture2D that we modify and next use to apply changes to the RenderTexture.
    //We could skip RenderTexture but it separates nicely our code into separate units
    //(this script knows nothing about UI. It only knows about the RenderTexture)
    private Texture2D m_drawingTexture, m_previewTexture, m_outlineTexture;

    //Color array 
    Color[] m_drawingPixelColors, m_previewPixelColors, m_outlinePixelColors;

    TextureRendererHelper m_textureRendererHelper;

    private bool[] m_drawableMask;
    
    private Stack<Color[]> undoStack = new Stack<Color[]>();
    private Stack<Color[]> redoStack = new Stack<Color[]>();

    public Vector2Int TextureSize => new(m_renderTexture.width, m_renderTexture.height);

    //Brush size. I will want to move it to a separate class
    public int BrushSize { get; private set; } = 1;

    private void Awake()
    {
        //Set size of the texture
        m_textureSize = ValidateSize(m_textureSize);
        m_textureRendererHelper = new TextureRendererHelper();
    }

    void Start()
    {
        //Prepare the drawing texture
        m_drawingTexture =
            new(m_renderTexture.width, m_renderTexture.height, TextureFormat.RGBA32, false);
        m_previewTexture =
            new(m_previewRenderTexture.width, m_previewRenderTexture.height, TextureFormat.RGBA32, false);
        m_outlineTexture =
            new(m_outlineRenderTexture.width, m_outlineRenderTexture.height, TextureFormat.RGBA32, false);
        m_outlinePixelColors = new Color[m_outlineRenderTexture.width * m_outlineRenderTexture.height];
        m_drawingPixelColors = new Color[m_renderTexture.width * m_renderTexture.height];
        m_previewPixelColors = new Color[m_previewRenderTexture.width * m_previewRenderTexture.height];
        SetFishBase();
    }
    
    public void SaveUndoState()
    {
        undoStack.Push((Color[])m_drawingPixelColors.Clone());
        redoStack.Clear();
    }
    
    public void Undo()
    {
        if (undoStack.Count > 0)
        {
            redoStack.Push((Color[])m_drawingPixelColors.Clone());
            m_drawingPixelColors = undoStack.Pop();
            UpdateRenderTexture(m_renderTexture, m_drawingTexture, m_drawingPixelColors);
        }
    }
    
    public void Redo()
    {
        if (redoStack.Count > 0)
        {
            undoStack.Push((Color[])m_drawingPixelColors.Clone());
            m_drawingPixelColors = redoStack.Pop();
            UpdateRenderTexture(m_renderTexture, m_drawingTexture, m_drawingPixelColors);
        }
    }


    private Vector2Int ValidateSize(Vector2Int m_textureSize)
    {
        if (m_textureSize.x < 1 || m_textureSize.y < 1)
        {
            Debug.LogError("Texture size must be at least 1x1. Setting to 16x16.");
            return new(16, 16);
        }

        return m_textureSize;
    }

    private void UpdateRenderTexture(RenderTexture renderTexture, Texture2D textureToUpdate, Color[] pixelsToApply)
    {
        textureToUpdate.SetPixels(pixelsToApply);
        textureToUpdate.Apply();
        m_textureRendererHelper.UpdateRenderTexture(renderTexture, textureToUpdate);
    }

    /// <summary>
    /// Draws new pixels on the canvas
    /// </summary>
    /// <param name="pixelPositions"></param>
    /// <param name="color"></param>
    /// <param name="blending"></param>
    public void DrawOnCanvas(List<Vector2Int> pixelPositions, Color color, bool blending = true)
    {
        List<Vector2Int> allowedPositions = new List<Vector2Int>();

        foreach (var pos in pixelPositions)
        {
            if (CanDrawAt(pos))
            {
                allowedPositions.Add(pos);
            }
        }

        // Call the actual drawing method only on allowed positions
        DrawOnTexture(color, allowedPositions, m_renderTexture, m_drawingTexture, m_drawingPixelColors, blending);
    }

    /// <summary>
    /// Draws new pixels on the preview Texture
    /// </summary>
    /// <param name="pixelPositions"></param>
    /// <param name="color"></param>
    /// <param name="blending"></param>
    public void DrawPreview(List<Vector2Int> pixelPositions, Color color, bool blending = true)
    {
        for (int i = 0; i < m_previewPixelColors.Length; i++)
        {
            m_previewPixelColors[i] = Color.clear;
        }

        DrawOnTexture(color, pixelPositions, m_previewRenderTexture, m_previewTexture, m_previewPixelColors, blending);
    }

    /// <summary>
    /// Allows us to either blend the new color with the existing one or override it. Eraser removed the existing color while brush and other tools blend 
    /// colors (a full alpha color will completely override the existing color).
    /// We pass so many parameters because I reuse this for preview and canvas drawing.
    /// </summary>
    /// <param name="color">Color to paint</param>
    /// <param name="pixelPositionsInTextureSpace"> Position in RenderTexture space (ex 32x32 mean x can be 0-31)</param>
    /// <param name="renderTexture">RenderTexture to update</param>
    /// <param name="textureToUpdate">Intermediary Texture2D to update</param>
    /// <param name="colors">Colors array to update</param>
    /// <param name="blending"></param>
    private void DrawOnTexture(Color color, List<Vector2Int> pixelPositionsInTextureSpace, RenderTexture renderTexture,
        Texture2D textureToUpdate, Color[] colors, bool blending)
    {
        //apply brush size
        foreach (Vector2Int pos in pixelPositionsInTextureSpace)
        {
            if (blending)
                BlendPixel(pos, color, renderTexture.width, colors);
            else
                DrawPixel(pos, color, renderTexture.width, colors);
        }

        UpdateRenderTexture(renderTexture, textureToUpdate, colors);
    }

    /// <summary>
    /// Blends pixel color present on the texture with the newly painted color
    /// </summary>
    /// <param name="position">Position in RenderTexture space (ex 32x32 mean x can be 0-31)</param>
    /// <param name="newColor"></param>
    /// <param name="width">Texture width</param>
    /// <param name="pixelColors">Color array to affect</param>
    private void BlendPixel(Vector2Int position, Color newColor, int width, Color[] pixelColors)
    {
        // Calculate 1D array index (row-major order)
        int pixelIndex = position.y * width + position.x;

        // Get the existing color
        Color existingColor = pixelColors[pixelIndex];

        // Blend the new color with the existing color
        Color blendedColor = BlendColors(existingColor, newColor);

        // Update color array with the blended color
        pixelColors[pixelIndex] = blendedColor;
    }

    /// <summary>
    /// Blends 2 colors together
    /// </summary>
    /// <param name="background"></param>
    /// <param name="foreground"></param>
    /// <returns></returns>
    private Color BlendColors(Color background, Color foreground)
    {
        float alpha = foreground.a;
        float inverseAlpha = 1 - alpha;

        return new Color(
            foreground.r * alpha + background.r * inverseAlpha,
            foreground.g * alpha + background.g * inverseAlpha,
            foreground.b * alpha + background.b * inverseAlpha,
            Mathf.Clamp01(background.a + foreground.a)
        );
    }

    /// <summary>
    /// Draws a single pixel on the canvas - overriding the existing color
    /// </summary>
    /// <param name="position">Position in RenderTexture space (ex 32x32 mean x can be 0-31)</param>
    /// <param name="newColor"></param>
    /// <param name="width">Texture width</param>
    /// <param name="pixelColors">Color array to affect</param>
    private void DrawPixel(Vector2Int position, Color color, int width, Color[] pixelColors)
    {
        // Calculate 1D array index (row-major order)
        int pixelIndex = position.y * m_renderTexture.width + position.x;
        //Update color array and the texture
        pixelColors[pixelIndex] = color;
    }

    /// <summary>
    /// Clears the RenderTextures that represents the canvas
    /// </summary>
    public void ClearCanvas()
    {
        SetFishBase();
    }

    public void SetFishBase()
    {
        // Set outline
        Texture2D outlineTexture = FishTemplateProvider.Instance.selectedTemplate.namedSprite.outlineSprite.texture;
        Color[] fishOutlineColorArray = outlineTexture.GetPixels();
        m_outlinePixelColors = fishOutlineColorArray;
        UpdateRenderTexture(m_outlineRenderTexture, m_outlineTexture, fishOutlineColorArray);

        // Set base
        Texture2D baseTexture = FishTemplateProvider.Instance.selectedTemplate.namedSprite.colorSprite.texture;
        Color[] fishBaseColorArray = baseTexture.GetPixels();

        m_drawingPixelColors = fishBaseColorArray;
        UpdateRenderTexture(m_renderTexture, m_drawingTexture, fishBaseColorArray);

        SetDrawableMask(baseTexture);
    }

    /// <summary>
    /// Clears the RenderTextures that represents the preview
    /// </summary>
    public void ClearPreview()
    {
        ClearTexture(m_previewPixelColors, m_previewRenderTexture, m_previewTexture);
    }

    private void ClearTexture(Color[] colors, RenderTexture renderTexture, Texture2D updateTexture)
    {
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = Color.clear;
        }

        UpdateRenderTexture(renderTexture, updateTexture, colors);
    }

    public void SetBrushSize(int v)
    {
        BrushSize = Mathf.Clamp(v, 1, Mathf.Max(m_renderTexture.width, m_renderTexture.height));
    }

    /// <summary>
    /// Uses a normalized position 0-1 (sent by the UI) to calculate the position on the RenderTexture.
    /// 
    /// </summary>
    /// <param name="normalizedPixelPosition"></param>
    /// <returns></returns>
    public Vector2Int CalculateTexturePosition(Vector2 normalizedPixelPosition)
    {
        return TextureRendererHelper.CalculateTexturePosition(normalizedPixelPosition, m_renderTexture);
    }

    internal Color GetPixelColor(Vector2Int pixelPosition)
    {
        int pixelIndex = pixelPosition.y * m_renderTexture.width + pixelPosition.x;
        return m_drawingPixelColors[pixelIndex];
    }

    private void SetDrawableMask(Texture2D fishBaseTexture)
    {
        m_drawableMask = new bool[fishBaseTexture.width * fishBaseTexture.height];
        for (int y = 0; y < fishBaseTexture.height; y++)
        {
            for (int x = 0; x < fishBaseTexture.width; x++)
            {
                int index = y * fishBaseTexture.width + x;
                Color pixelColor = fishBaseTexture.GetPixel(x, y);
                m_drawableMask[index] = pixelColor.a > 0.1f;
            }
        }
    }

    public bool CanDrawAt(Vector2Int position)
    {
        int index = position.y * m_renderTexture.width + position.x;
        return m_drawableMask != null && m_drawableMask[index];
    }
}