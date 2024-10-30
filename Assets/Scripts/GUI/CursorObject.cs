using UnityEngine;
using UnityEngine.EventSystems;

public class CursorObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private PettingManager pettingManager;

    private void Start()
    {
        // Find the PettingManager in the scene, assuming it's attached to a GameObject tagged as "PettingManager"
        pettingManager = FindObjectOfType<PettingManager>();
        
        if (pettingManager == null)
        {
            Debug.LogError("PettingManager not found in the scene.");
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (pettingManager != null)
        {
            // Change the cursor to the Petting type
            pettingManager.SetActiveCursorType(PettingManager.CursorType.Petting);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (pettingManager != null)
        {
            // Revert the cursor back to the DefaultHand type
            pettingManager.SetActiveCursorType(PettingManager.CursorType.DefaultHand);
        }
    }
}