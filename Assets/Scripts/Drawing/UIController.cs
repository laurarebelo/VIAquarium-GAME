using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// A single class that handles all the UI assignment to connect the UI Toolkit and allow other scripts listen to its events.
/// Definitely needs refactoring into smaller classes. Because we are using "event Action" to send information to other scripts
/// it can be refactored later without breaking the code.
/// </summary>
public class UIController : MonoBehaviour
{
    private UIDocument m_uiDocument;
    private VisualElement m_canvas, m_canvasBackground, m_previewCanvas, m_outlineCanvas;

    private VisualElement
        m_brushButton,
        m_lineButton,
        m_squareButton,
        m_colorPickerButton,
        m_bucketButton;

    public UnityEngine.UI.Button undoButton;
    public UnityEngine.UI.Button redoButton;
    public UnityEngine.UI.Button clearButton;

    private Slider m_saturationSlider, m_valueSlider, m_alphaSlider;
    private IntegerField m_hueField, m_saturationField, m_valueField, m_alphaField;
    private VisualElement m_colorSquare, m_selectedColor, m_hueStrip;

    public event Action<Vector2> OnPointerDown, OnPointerMoved, OnPointerEntered, OnPointerReleased;

    public event Action OnClearButtonClicked,
        OnRectangleClicked,
        OnLineClicked,
        OnBrushClicked,
        OnPointerOut,
        OnColorPickerClicked,
        OnBucketClicked;

    public event Action OnUndoButtonClicked, OnRedoButtonClicked;

    public event Action<Vector2> OnColorSelected, OnHueSelected;
    public event Action<int> OnHueChanged, OnAlphaChanged, OnSaturationChanged, OnValueChanged;

    private bool m_pointerColorSquareHeld = false;
    private bool m_pointerHueStripHeld = false;

    /// <summary>
    /// Simple solution to ensure that the selected tool is highlighted by being able to loop through all the buttons and disable the highlight
    /// </summary>
    private List<VisualElement> m_toolButtons = new();

    private void Awake()
    {
        m_uiDocument = GetComponent<UIDocument>();
        //We need to type the same name as we gave in Ui Builder
        m_canvas = m_uiDocument.rootVisualElement.Q<VisualElement>("Canvas");
        m_canvasBackground = m_uiDocument.rootVisualElement.Q<VisualElement>("CanvasBackground");
        m_previewCanvas = m_uiDocument.rootVisualElement.Q<VisualElement>("PreviewCanvas");
        m_outlineCanvas = m_uiDocument.rootVisualElement.Q<VisualElement>("Outline");

        m_brushButton = m_uiDocument.rootVisualElement.Q<VisualElement>("BrushButtonBackground");
        m_lineButton = m_uiDocument.rootVisualElement.Q<VisualElement>("LineButtonBackground");
        m_squareButton = m_uiDocument.rootVisualElement.Q<VisualElement>("SquareButtonBackground");
        m_colorPickerButton = m_uiDocument.rootVisualElement.Q<VisualElement>("ColorPickerButtonBackground");
        m_bucketButton = m_uiDocument.rootVisualElement.Q<VisualElement>("BucketButtonBackground");

        m_hueStrip = m_uiDocument.rootVisualElement.Q<VisualElement>("HueStrip");

        m_saturationSlider = m_uiDocument.rootVisualElement.Q<Slider>("SaturationSlider");
        m_valueSlider = m_uiDocument.rootVisualElement.Q<Slider>("ValueSlider");
        m_alphaSlider = m_uiDocument.rootVisualElement.Q<Slider>("AlphaSlider");

        m_colorSquare = m_uiDocument.rootVisualElement.Q<VisualElement>("ColorSelector");
        m_selectedColor = m_uiDocument.rootVisualElement.Q<VisualElement>("SelectedColor");
        m_alphaField = m_uiDocument.rootVisualElement.Q<IntegerField>("AlphaField");
        m_hueField = m_uiDocument.rootVisualElement.Q<IntegerField>("HueField");
        m_saturationField = m_uiDocument.rootVisualElement.Q<IntegerField>("SaturationField");
        m_valueField = m_uiDocument.rootVisualElement.Q<IntegerField>("ValueField");
    }

    private void Start()
    {
        m_toolButtons.Add(m_brushButton);
        m_toolButtons.Add(m_lineButton);
        m_toolButtons.Add(m_squareButton);
        m_toolButtons.Add(m_colorPickerButton);
        m_toolButtons.Add(m_bucketButton);

        //Make the Canvas square
        m_canvas.RegisterCallback<GeometryChangedEvent>(HandleCanvasGeometryChanged);

        //canvas callbacks
        //Trickle down https://docs.unity.cn/2023.2/Documentation/Manual/UIE-Events-Dispatching.html
        m_canvas.RegisterCallback<PointerDownEvent>(HandlePointerDown, TrickleDown.TrickleDown);
        m_canvas.RegisterCallback<PointerMoveEvent>(HandlePointerMove, TrickleDown.TrickleDown);
        m_canvas.RegisterCallback<PointerUpEvent>(HandlePointerUp, TrickleDown.TrickleDown);
        m_canvas.RegisterCallback<PointerOutEvent>(HandlePointerOut, TrickleDown.TrickleDown);
        m_canvas.RegisterCallback<PointerEnterEvent>(HandlePointerIn, TrickleDown.TrickleDown);

        //Button callbacks
        clearButton.onClick.AddListener(() => OnClearButtonClicked?.Invoke());
        m_brushButton.RegisterCallback<ClickEvent>((arg) => OnBrushClicked?.Invoke());
        m_brushButton.RegisterCallback<ClickEvent>((evt) => SetButtonChecked(evt, m_brushButton));
        m_lineButton.RegisterCallback<ClickEvent>((arg) => OnLineClicked?.Invoke());
        m_lineButton.RegisterCallback<ClickEvent>((evt) => SetButtonChecked(evt, m_lineButton));
        m_squareButton.RegisterCallback<ClickEvent>((arg) => OnRectangleClicked?.Invoke());
        m_squareButton.RegisterCallback<ClickEvent>((evt) => SetButtonChecked(evt, m_squareButton));
        m_colorPickerButton.RegisterCallback<ClickEvent>((arg) => OnColorPickerClicked?.Invoke());
        m_colorPickerButton.RegisterCallback<ClickEvent>((evt) => SetButtonChecked(evt, m_colorPickerButton));
        m_bucketButton.RegisterCallback<ClickEvent>((arg) => OnBucketClicked?.Invoke());
        m_bucketButton.RegisterCallback<ClickEvent>((evt) => SetButtonChecked(evt, m_bucketButton));
        undoButton.onClick.AddListener(() => OnUndoButtonClicked?.Invoke());
        redoButton.onClick.AddListener(() => OnRedoButtonClicked?.Invoke());

        //Color selector callbacks
        m_colorSquare.RegisterCallback<PointerDownEvent>(HandleColorSquareClicked);
        m_colorSquare.RegisterCallback<PointerMoveEvent>(HandleColorSquareHeld, TrickleDown.TrickleDown);
        m_colorSquare.RegisterCallback<PointerUpEvent>((arg) => m_pointerColorSquareHeld = false,
            TrickleDown.TrickleDown);
        m_colorSquare.RegisterCallback<PointerLeaveEvent>((arg) => m_pointerColorSquareHeld = false,
            TrickleDown.TrickleDown);

        m_hueStrip.RegisterCallback<PointerDownEvent>(HandleHueStripClicked);
        m_hueStrip.RegisterCallback<PointerMoveEvent>(HandleHueStripHeld, TrickleDown.TrickleDown);
        m_hueStrip.RegisterCallback<PointerUpEvent>((arg) => m_pointerHueStripHeld = false,
            TrickleDown.TrickleDown);
        m_hueStrip.RegisterCallback<PointerLeaveEvent>((arg) => m_pointerHueStripHeld = false,
            TrickleDown.TrickleDown);


        m_hueField.RegisterValueChangedCallback(ChangeHue);
        m_alphaSlider.RegisterValueChangedCallback(ChangeAlpha);
        m_alphaField.RegisterValueChangedCallback(ChangeAlpha);
        m_saturationSlider.RegisterValueChangedCallback(SaturationChanged);
        m_saturationField.RegisterValueChangedCallback(SaturationChanged);
        m_valueSlider.RegisterValueChangedCallback(ValueChanged);
        m_valueField.RegisterValueChangedCallback(ValueChanged);
    }

    /// <summary>
    /// Applies highlight to the selected tool (button)
    /// </summary>
    /// <param name="evt"></param>
    /// <param name="element"></param>
    private void SetButtonChecked(ClickEvent evt, VisualElement element)
    {
        foreach (var item in m_toolButtons)
        {
            item.style.unityBackgroundImageTintColor = Color.white;
        }

        element.style.unityBackgroundImageTintColor = Color.yellow;
    }

    /// <summary>
    /// Allows us do click and drag to select a color
    /// </summary>
    /// <param name="evt"></param>
    private void HandleColorSquareHeld(PointerMoveEvent evt)
    {
        if (m_pointerColorSquareHeld == false)
            return;
        Vector2 normalizedPosition = ProcessPosition(evt.localPosition, m_colorSquare);
        OnColorSelected?.Invoke(normalizedPosition);
    }

    private void HandleHueStripHeld(PointerMoveEvent evt)
    {
        if (m_pointerHueStripHeld == false)
            return;
        Vector2 normalizedPosition = ProcessPosition(evt.localPosition, m_hueStrip);
        OnHueSelected?.Invoke(normalizedPosition);
    }

    // Helper methods that allows me to control the input value of the field (to clamp it) and to inform about this event
    // the DrawingManager
    private void ValueChanged(ChangeEvent<int> evt)
    {
        SetValue(evt.newValue);
        OnValueChanged?.Invoke(m_valueField.value);
    }

    private void ValueChanged(ChangeEvent<float> evt)
    {
        SetValue(Mathf.RoundToInt(evt.newValue));
        OnValueChanged?.Invoke(m_valueField.value);
    }

    private void SaturationChanged(ChangeEvent<int> evt)
    {
        SetSaturation(evt.newValue);
        OnSaturationChanged?.Invoke(m_saturationField.value);
    }

    private void SaturationChanged(ChangeEvent<float> evt)
    {
        SetSaturation(Mathf.RoundToInt(evt.newValue));
        OnSaturationChanged?.Invoke(m_saturationField.value);
    }

    private void ChangeAlpha(ChangeEvent<int> evt)
    {
        SetAlpha(evt.newValue);
        OnAlphaChanged?.Invoke(m_alphaField.value);
    }

    private void ChangeAlpha(ChangeEvent<float> evt)
    {
        SetAlpha(Mathf.RoundToInt(evt.newValue));
        OnAlphaChanged?.Invoke(m_alphaField.value);
    }

    private void ChangeHue(ChangeEvent<int> evt)
    {
        SetHue(evt.newValue);
        OnHueChanged?.Invoke(m_hueField.value);
    }

    /// <summary>
    /// Sets the color when we click on the color square
    /// </summary>
    /// <param name="evt"></param>
    private void HandleColorSquareClicked(PointerDownEvent evt)
    {
        m_pointerColorSquareHeld = true;
        Vector2 normalizedPosition = ProcessPosition(evt.localPosition, m_colorSquare);
        OnColorSelected?.Invoke(normalizedPosition);
    }

    private void HandleHueStripClicked(PointerDownEvent evt)
    {
        m_pointerHueStripHeld = true;
        Vector2 normalizedPosition = ProcessPosition(evt.localPosition, m_hueStrip);
        OnHueSelected?.Invoke(normalizedPosition);
    }

    /// <summary>
    /// Sets the selected color to the VisualElement representing it
    /// </summary>
    /// <param name="colorRGB"></param>
    public void SetColor(Color colorRGB)
    {
        m_selectedColor.style.backgroundColor = colorRGB;
    }

    /// <summary>
    /// Updates color values in the UI. ColorSquare is updated automatically by the ColorSelector
    /// since the UI only shows the RenderTexture asset.
    /// </summary>
    /// <param name="colorRGB"></param>
    public void UpdateColorValues(Color colorRGB)
    {
        Color.RGBToHSV(colorRGB, out float hue, out float saturation, out float value);
        m_hueField.SetValueWithoutNotify(Mathf.RoundToInt(hue * 360f));
        // TODO HUE SLIDER m_hueSlider.SetValueWithoutNotify(Mathf.RoundToInt(hue * 360f));
        m_saturationField.SetValueWithoutNotify(Mathf.RoundToInt(saturation * 100));
        m_saturationSlider.SetValueWithoutNotify(Mathf.RoundToInt(saturation * 100));
        m_valueField.SetValueWithoutNotify(Mathf.RoundToInt(value * 100));
        m_valueSlider.SetValueWithoutNotify(Mathf.RoundToInt(value * 100));

        m_alphaSlider.SetValueWithoutNotify(Mathf.RoundToInt(colorRGB.a * 100));
    }

    // We use SetValueWithoutNotify because otherwise we would get an infinite loop of events
    // There might be better way to make it work that i am not aware of

    public void SetHue(int hue)
    {
        m_hueField.SetValueWithoutNotify(Mathf.Clamp(hue, 0, 360));

        // TODO HUE SLIDER m_hueSlider.SetValueWithoutNotify(m_hueField.value);
    }

    public void SetSaturation(int saturation)
    {
        m_saturationField.SetValueWithoutNotify(Mathf.Clamp(saturation, 0, 100));
        m_saturationSlider.SetValueWithoutNotify(m_saturationField.value);
    }

    public void SetValue(int value)
    {
        m_valueField.SetValueWithoutNotify(Mathf.Clamp(value, 0, 100));
        m_valueSlider.SetValueWithoutNotify(m_valueField.value);
    }

    public void SetAlpha(int alpha)
    {
        m_alphaField.SetValueWithoutNotify(Mathf.Clamp(alpha, 0, 100));
        m_alphaSlider.SetValueWithoutNotify(m_alphaField.value);
    }

    /// <summary>
    /// Clear canvas pointer events. This way we can ensure that if we select a new tool we don't get the old tool events.
    /// </summary>
    public void ClearCanvasPointerEvents()
    {
        OnPointerDown = null;
        OnPointerMoved = null;
        OnPointerEntered = null;
        OnPointerReleased = null;
        OnPointerOut = null;
    }

    /// <summary>
    /// Handles pointer movement inside the canvas
    /// </summary>
    /// <param name="evt"></param>
    private void HandlePointerIn(PointerEnterEvent evt)
    {
        Vector2 normalizedPosition = ProcessPosition(evt.localPosition, m_canvas);
        OnPointerEntered?.Invoke(normalizedPosition);
    }

    /// <summary>
    /// We use this method to set make the canvas into square.
    /// </summary>
    /// <param name="evt"></param>
    private void HandleCanvasGeometryChanged(GeometryChangedEvent evt)
    {
        float desiredSize = m_canvasBackground.resolvedStyle.height -
                            m_canvasBackground.resolvedStyle.paddingBottom * 2;
        m_canvasBackground.style.width = desiredSize;
        m_canvas.style.height = desiredSize;
        m_canvas.style.width = desiredSize;
        m_previewCanvas.style.height = desiredSize;
        m_previewCanvas.style.width = desiredSize;
    }

    //Canvas related methods that handles different events

    private void HandlePointerOut(PointerOutEvent evt)
    {
        OnPointerOut?.Invoke();
    }

    private void HandlePointerUp(PointerUpEvent evt)
    {
        //Debug.Log($"Pointer Up {evt.localPosition}");
        Vector2 normalizedPosition = ProcessPosition(evt.localPosition, m_canvas);
        OnPointerReleased?.Invoke(normalizedPosition);
    }

    private void HandlePointerMove(PointerMoveEvent evt)
    {
        Vector2 normalizedPosition = ProcessPosition(evt.localPosition, m_canvas);
        //Debug.Log($"Pointer Moved {normalizedPosition}");
        OnPointerMoved?.Invoke(normalizedPosition);
    }

    private void HandlePointerDown(PointerDownEvent evt)
    {
        Vector2 normalizedPosition = ProcessPosition(evt.localPosition, m_canvas);
        //Debug.Log($"Pointer Down {normalizedPosition}");
        OnPointerDown?.Invoke(normalizedPosition);
    }


    //Flip Y axis to be pointing UP. Otherwise Ui Toolkit draws the UI with Y axis pointing down. By default unity draws with Y axis pointing up.
    private Vector2 ProcessPosition(Vector2 localMousePosition, VisualElement element)
    {
        //Debug.Log(localMousePosition);
        Vector2 normalizedPosition = NormalizePixelPosition(localMousePosition, element.layout);
        normalizedPosition.y = 1 - normalizedPosition.y;
        return normalizedPosition;
    }

    //normalized pixel position to the 0-1 range based on Ui element width and height
    //This allows us to easily detect where on the RenderTexture we want to draw
    private Vector2 NormalizePixelPosition(Vector2 pixelPosition, Rect layout)
    {
        float normalizedX = Mathf.InverseLerp(0, layout.width, pixelPosition.x);
        float normalizedY = Mathf.InverseLerp(0, layout.height, pixelPosition.y);
        return new(normalizedX, normalizedY);
    }
}