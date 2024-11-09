using UnityEngine;

/// <summary>
/// Helper script that makes it easier to render textures to RenderTextures. 
/// </summary>
public class TextureRendererHelper
{
    public void UpdateRenderTexture(RenderTexture textureToRenderOn, Texture2D contentToRender)
    {
        //Save the previous active texture
        RenderTexture currentActiveRT = RenderTexture.active;
        //Set our RenderTexture as active
        RenderTexture.active = textureToRenderOn;

        //Draw Texture2D to RenderTexture
        Graphics.Blit(contentToRender, textureToRenderOn);
        //Reset active RenderTexture
        RenderTexture.active = currentActiveRT;
    }

    public static Vector2Int CalculateTexturePosition(Vector2 normalizedPixelPosition, RenderTexture renderTexture)
    {
        return new((int)(normalizedPixelPosition.x * (renderTexture.width)),
                    (int)(normalizedPixelPosition.y * (renderTexture.height)));
    }
}
