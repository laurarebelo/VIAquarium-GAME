using UnityEngine;

[System.Serializable]
public class NamedSprite
{
    public string name; 
    public Sprite outlineSprite;
    public Sprite colorSprite;
    public Sprite outlineDeadSprite;

    public NamedSprite(string name, Sprite outlineSprite, Sprite colorSprite, Sprite outlineDeadSprite)
    {
        this.name = name;
        this.outlineSprite = outlineSprite;
        this.colorSprite = colorSprite;
        this.outlineDeadSprite = outlineDeadSprite;
    }
}