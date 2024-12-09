using UnityEngine;
using UnityEngine.UI;

public class SetActiveOnClickButton : MonoBehaviour
{
    public GameObject targetObject;
    public bool active;
    private Button xButton;

    private void Awake()
    {
        if (xButton == null)
        {
            xButton = GetComponent<Button>();
        }

        if (xButton != null)
        {
            xButton.onClick.AddListener(SetActiveTargetObject);
        }
    }

    private void SetActiveTargetObject()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(active);
        }
        else
        {
            Debug.LogWarning("Target object is not assigned.");
        }
    }
}