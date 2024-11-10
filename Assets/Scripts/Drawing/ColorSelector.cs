using System;
using UnityEngine;

/// <summary>
/// Draws a color square (color wheel) texture and allows to select a color from it.
/// I will want to separate it into 2 classes: ColorSelector and ColorSquareRenderer.
/// </summary>
public class ColorSelector : MonoBehaviour
{
    [SerializeField]
    private RenderTexture m_colorSquareTexture;

    private Texture2D m_colorsTexture;
    private Color[] m_colors;

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
        m_colorsTexture = new Texture2D(m_colorSquareTexture.width, m_colorSquareTexture.height);
        m_colors = new Color[m_colorSquareTexture.width * m_colorSquareTexture.height];
    }

    /// <summary>
    /// Updates the ColorSquare texture with the new hue value.
    /// </summary>
    /// <param name="hue"></param>
    private void UpdateColorSquare(float hue)
    {
        GenerateColorSquare(hue, new Vector2Int(m_colorSquareTexture.width, m_colorSquareTexture.height), m_colors);
        m_colorsTexture.SetPixels(m_colors);
        m_colorsTexture.Apply();
        m_textureRendererHelper.UpdateRenderTexture(m_colorSquareTexture, m_colorsTexture);
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
        Vector2Int texturePosition = TextureRendererHelper.CalculateTexturePosition(vector, m_colorSquareTexture);
        Color newColor = m_colors[texturePosition.y * m_colorSquareTexture.width + texturePosition.x];
        newColor.a = DrawColor.a; // Preserve original alpha
        DrawColor = newColor;
        OnColorChanged?.Invoke(DrawColor);
    }


    /// <summary>
    /// Helps to set the Hue value correctly. 
    /// We call events specific to the parameter changed to avoid locking slider (since if Value == 0 Hue and saturation are converted to 0 by RGBToHSV method)
    /// </summary>
    /// <param name="hue"></param>
    public void SetHue(int hue)
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
