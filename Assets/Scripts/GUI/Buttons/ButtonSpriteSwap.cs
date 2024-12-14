using UnityEngine;
using UnityEngine.UI;

public class ButtonSpriteSwap : MonoBehaviour
{
    public Sprite sprite1;
    public Sprite sprite2;

    private Image buttonImage;
    private bool isSprite1Active = true;
    public bool trueForMusicFalseForSound = false;

    void Start()
    {
        buttonImage = GetComponent<Image>();
        if (buttonImage != null)
        {
            if (trueForMusicFalseForSound && AudioState.Instance.isMusicMuted || !trueForMusicFalseForSound && AudioState.Instance.areSoundsMuted)
            {
                buttonImage.sprite = sprite2;
                isSprite1Active = false;
            }
        }
    }

    public void SwapSprite()
    {
        if (buttonImage != null)
        {
            buttonImage.sprite = isSprite1Active ? sprite2 : sprite1;
            isSprite1Active = !isSprite1Active;
        }
    }
}