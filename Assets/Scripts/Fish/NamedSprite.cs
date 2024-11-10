using UnityEngine;

[System.Serializable]
public class NamedSprite
{
    public string name; 
    public Sprite outlineSprite;
    public Sprite colorSprite;

    public NamedSprite(string name, Sprite outlineSprite, Sprite colorSprite)
    {
        this.name = name;
        this.outlineSprite = outlineSprite;
        this.colorSprite = colorSprite;
    }
}