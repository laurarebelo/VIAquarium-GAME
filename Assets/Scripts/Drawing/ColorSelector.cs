using System;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Draws a color square (color wheel) texture and allows to select a color from it.
/// I will want to separate it into 2 classes: ColorSelector and ColorSquareRenderer.
/// </summary>
public class ColorSelector : MonoBehaviour
{
    public RenderTexture m_colorSquareRenderTexture;
    private Texture2D m_colorSquareTexture;
    private Color[] m_colorSquareColors;
    
    public RenderTexture m_hueStripRenderTexture;
    private Texture2D m_hueStripTexture;
    private Color[] m_hueStripColors;

    private TextureRendererHelper m_textureRendererHelper;

    public Color DrawColor { get; private set; }
    //public Color PreviewColor { get; private set; }

    /// <summary>
    /// Informs UI that it should update the sliders / value in the color selector (color wheel)
    /// </summary>
    public event Action<Color> OnColorChanged;
    /// <summary>
    /// Informs about Hue value change.
    /// </summary>
    public event Action<int> OnHueChanged;
    /// <summary>
    /// Informs about Saturation value change.
    /// </summary>
    public event Action<int> OnSaturationChanged;
    /// <summary>
    /// Informs about Value value change.
    /// </summary>
    public event Action<int> OnValueChanged;
    /// <summary>
    /// Informs about Alpha value change.
    /// </summary>
    public event Action<int> OnAlphaChanged;


    private void Awake()
    {
        m_textureRendererHelper = new TextureRendererHelper();
        
        m_colorSquareTexture = new Texture2D(m_colorSquareRenderTexture.width, m_colorSquareRenderTexture.height);
        m_colorSquareColors = new Color[m_colorSquareRenderTexture.width * m_colorSquareRenderTexture.height];
        
        m_hueStripTexture = new Texture2D(m_hueStripRenderTexture.width, m_hueStripRenderTexture.height);
        m_hueStripColors = new Color[m_hueStripRenderTexture.width * m_hueStripRenderTexture.height];
    }

    /// <summary>
    /// Updates the ColorSquare texture with the new hue value.
    /// </summary>
    /// <param name="hue"></param>
    private void UpdateColorSquare(float hue)
    {
        GenerateColorSquare(hue, new Vector2Int(m_colorSquareRenderTexture.width, m_colorSquareRenderTexture.height), m_colorSquareColors);
        m_colorSquareTexture.SetPixels(m_colorSquareColors);
        m_colorSquareTexture.Apply();
        m_textureRendererHelper.UpdateRenderTexture(m_colorSquareRenderTexture, m_colorSquareTexture);
    }

    public void UpdateHueStrip()
    {
        GenerateHueStrip(new Vector2Int(m_hueStripRenderTexture.width, m_hueStripRenderTexture.height), m_hueStripColors);
        m_hueStripTexture.SetPixels(m_hueStripColors);
        m_hueStripTexture.Apply();
        m_textureRendererHelper.UpdateRenderTexture(m_hueStripRenderTexture, m_hueStripTexture);
    }

    /// <summary>
    /// Generates a color square texture with the given hue value.
    /// </summary>
    /// <param name="hue"></param>
    /// <param name="textureSize"></param>
    /// <param name="colors"></param>
    public void GenerateColorSquare(float hue, Vector2Int textureSize, Color[] colors)
    {
        for (int y = 0; y < textureSize.y; y++)
        {
            for (int x = 0; x < textureSize.x; x++)
            {
                float saturation = (float)x / textureSize.x;  // X axis for saturation (S)
                float value = (float)y / textureSize.y;       // Y axis for value (V)
                colors[y * textureSize.x + x] = ColorFromHSV(hue, saturation, value);
            }
        }
    }
    
    /// <summary>
    /// Generates a horizontal hue strip texture.
    /// </summary>
    /// <param name="textureSize">The size of the texture.</param>
    /// <param name="colors">The array to hold the color data.</param>
    public void GenerateHueStrip(Vector2Int textureSize, Color[] colors)
    {
        for (int y = 0; y < textureSize.y; y++)
        {
            for (int x = 0; x < textureSize.x; x++)
            {
                float hue = (float)x / textureSize.x;
                colors[y * textureSize.x + x] = ColorFromHSV(hue, 1f, 1f);
            }
        }
    }

    // Converts HSV to RGB color
    public Color ColorFromHSV(float h, float s, float v)
    {
        float r = 0, g = 0, b = 0;

        int i = Mathf.FloorToInt(h * 6);
        float f = h * 6 - i;
        float p = v * (1 - s);
        float q = v * (1 - f * s);
        float t = v * (1 - (1 - f) * s);

        switch (i % 6)
        {
            case 0: r = v; g = t; b = p; break;
            case 1: r = q; g = v; b = p; break;
            case 2: r = p; g = v; b = t; break;
            case 3: r = p; g = q; b = v; break;
            case 4: r = t; g = p; b = v; break;
            case 5: r = v; g = p; b = q; break;
        }

        return new Color(r, g, b);
    }

    /// <summary>
    /// Sets the new color and informs about the change our UI
    /// </summary>
    /// <param name="c"></param>
    public void SetColor(Color c)
    {
        DrawColor = c;
        Color.RGBToHSV(c, out float h, out float s, out float v);
        UpdateColorSquare(h);
        OnColorChanged?.Invoke(DrawColor);
    }

    /// <summary>
    /// Used by the UI color selector to select a color based on the position of the cursor.
    /// </summary>
    /// <param name="vector"></param>
    public void SetColor(Vector2 vector)
    {
        Vector2Int texturePosition = TextureRendererHelper.CalculateTexturePosition(vector, m_colorSquareRenderTexture);
        Color newColor = m_colorSquareColors[texturePosition.y * m_colorSquareRenderTexture.width + texturePosition.x];
        newColor.a = DrawColor.a; // Preserve original alpha
        DrawColor = newColor;
        OnColorChanged?.Invoke(DrawColor);
    }

    public void SetHueByVector(Vector2 vector)
    {
        Vector2Int texturePosition = TextureRendererHelper.CalculateTexturePosition(vector, m_hueStripRenderTexture);
        Color newColor = m_hueStripColors[texturePosition.y * m_hueStripRenderTexture.width + texturePosition.x];
        // change the color square to be in that hue
        Color.RGBToHSV(newColor,out float h,out float s, out float v);
        OnHueChanged?.Invoke(Mathf.RoundToInt(h * 360));
        
        // change the selected color to be that hue
        Color.RGBToHSV(DrawColor,out float wtv,out s, out v);
        Color newDrawColor = Color.HSVToRGB(h, s, v);
        SetColor(newDrawColor);
    }


    /// <summary>
    /// Helps to set the Hue value correctly. 
    /// We call events specific to the parameter changed to avoid locking slider (since if Value == 0 Hue and saturation are converted to 0 by RGBToHSV method)
    /// </summary>
    /// <param name="hue"></param>
    public void SetHueByNumber(int hue)
    {
        float h, s, v;
        Color.RGBToHSV(DrawColor, out h, out s, out v);
        h = hue / 360f; // Convert 0-360 to 0-1 range
        Color newColor = Color.HSVToRGB(h, s, v);
        newColor.a = DrawColor.a; // Preserve original alpha
        DrawColor = newColor;
        UpdateColorSquare(h);
        OnHueChanged?.Invoke(hue);
    }

    /// <summary>
    /// Helps to set the Saturation value.
    /// We call events specific to the parameter changed to avoid locking slider (since if Value == 0 Hue and saturation are converted to 0 by RGBToHSV method)
    /// </summary>
    /// <param name="saturation"></param>
    public void SetSaturation(int saturation)
    {
        float h, s, v;
        Color.RGBToHSV(DrawColor, out h, out s, out v);
        s = Mathf.Clamp01(saturation / 100f); // Convert 0-100 to 0-1 range
        Color newColor = Color.HSVToRGB(h, s, v);
        newColor.a = DrawColor.a; // Preserve original alpha
        DrawColor = newColor;
        OnSaturationChanged?.Invoke(saturation);
    }

    /// <summary>
    /// Helps to set the Value value.
    /// We call events specific to the parameter changed to avoid locking slider (since if Value == 0 Hue and saturation are converted to 0 by RGBToHSV method)
    /// </summary>
    /// <param name="value"></param>
    public void SetValue(int value)
    {
        float h, s, v;
        Color.RGBToHSV(DrawColor, out h, out s, out v);
        v = Mathf.Clamp01(value / 100f); // Convert 0-100 to 0-1 range
        Color newColor = Color.HSVToRGB(h, s, v);
        newColor.a = DrawColor.a; // Preserve original alpha
        DrawColor = newColor;
        OnValueChanged?.Invoke(value);
    }

    /// <summary>
    /// Helps to set the Alpha value.
    /// We call events specific to the parameter changed to avoid locking slider (since if Value == 0 Hue and saturation are converted to 0 by RGBToHSV method)
    /// </summary>
    /// <param name="alpha"></param>
    public void SetAlpha(int alpha)
    {
        Color newColor = DrawColor;
        newColor.a = Mathf.Clamp01(alpha / 100f); // Convert 0-255 to 0-1 range
        DrawColor = newColor;
        OnAlphaChanged?.Invoke(alpha);
    }
}
