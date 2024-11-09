using UnityEngine;

/// <summary>
/// Renders a checker pattern under the Canvas ot make it easier to see the pixel positions.
/// </summary>
public class GridRenderer : MonoBehaviour
{
    /// <summary>
    /// Colors to use to draw the checker pattern.
    /// </summary>
    [SerializeField]
    private Color gridColor_1, gridColor_2;
    /// <summary>
    /// We will render the checker pattern to this RenderTexture
    /// </summary>
    [SerializeField]
    private RenderTexture m_gridRenderTexture;

    Texture2D m_gridTexture;
    TextureRendererHelper m_textureRendererHelper;

    private void Awake()
    {
        m_textureRendererHelper = new TextureRendererHelper();

    }

    /// <summary>
    /// Generates a checker pattern and renders it to the RenderTexture.
    /// </summary>
    /// <param name="textureSize"></param>
    public void RenderGrid(Vector2Int textureSize)
    {
        m_gridTexture = new Texture2D(m_gridRenderTexture.width, m_gridRenderTexture.height, TextureFormat.RGBA32, false);
        Color[] colors = new Color[textureSize.x * textureSize.y];
        //create check pattern
        for (int y = 0; y < textureSize.y; y++)
        {
            for (int x = 0; x < textureSize.x; x++)
            {
                if ((x + y) % 2 == 0)
                {
                    colors[y * textureSize.x + x] = gridColor_1;
                }
                else
                {
                    colors[y * textureSize.x + x] = gridColor_2;
                }
            }
        }
        m_gridTexture.SetPixels(colors);
        m_gridTexture.Apply();
        m_textureRendererHelper.UpdateRenderTexture(m_gridRenderTexture, m_gridTexture);

    }
}