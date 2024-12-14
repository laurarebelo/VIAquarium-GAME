using UnityEngine;

[ExecuteAlways] // Ensures validation runs in the editor and during play
public class CameraBounds : MonoBehaviour
{
    [SerializeField]
    private float minX = -3;

    [SerializeField]
    private float maxX = 10;
    public float MinX
    {
        get => minX;
        set
        {
            minX = Mathf.Min(value, maxX); 
        }
    }

    public float MaxX
    {
        get => maxX;
        set
        {
            maxX = Mathf.Max(value, 0); 
        }
    }

    public float ClampPosition(float x)
    {
        return Mathf.Clamp(x, minX, maxX);
    }

    private void OnValidate()
    {
        if (minX > maxX)
        {
            Debug.LogWarning("minX cannot exceed maxX. Adjusting values.");
            maxX = minX;
        }
    }
}