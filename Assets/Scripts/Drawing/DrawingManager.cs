using UnityEngine;

/// <summary>
/// Manages the app. Connects UI with DrawingRenderer and DrawingTools. Needs some refactoring.
/// </summary>
public class DrawingManager : MonoBehaviour
{
    //Get references to UIController and DrawingRenderer
    [SerializeField]
    private UIController m_uiController;
    [SerializeField]
    private DrawingRenderer m_drawingRenderer;
    [SerializeField]
    private GridRenderer m_gridRenderer;

    private DrawingTool m_currentTool;

    private Vector2 m_drawingStartPointInNormalizedCoordinates;

    private Color m_selectedColor = Color.yellow;

    private bool m_isDrawing = false;

    [SerializeField]
    private ColorSelector m_colorSelector;

    //Connect events
    private void Start()
    {
        m_gridRenderer.RenderGrid(m_drawingRenderer.TextureSize);
        m_uiController.OnClearButtonClicked += ClearDrawing;

        m_uiController.OnBrushClicked += SetBrush;
        // m_uiController.OnEraserClicked += SetEraser;
        m_uiController.OnLineClicked += SetLine;
        m_uiController.OnRectangleClicked += SetUpRectangle;
        // m_uiController.OnSaveButtonClicked += () => m_textureSaver.SaveTextureAsPNG();
        m_uiController.OnColorPickerClicked += SetColorPicker;

        m_uiController.OnBrushSizePlusClicked += IncreaseBrushSize;
        m_uiController.OnBrushSizeMinusClicked += DecreaseBrushSize;

        m_uiController.OnColorSelected += m_colorSelector.SetColor;
        m_uiController.OnHueChanged += m_colorSelector.SetHue;
        m_uiController.OnSaturationChanged += m_colorSelector.SetSaturation;
        m_uiController.OnValueChanged += m_colorSelector.SetValue;
        m_uiController.OnAlphaChanged += m_colorSelector.SetAlpha;

        m_colorSelector.OnColorChanged += m_uiController.SetColor;
        m_colorSelector.OnColorChanged += m_uiController.UpdateColorValues;
        m_colorSelector.OnHueChanged += m_uiController.SetHue;
        m_colorSelector.OnHueChanged += (val) => m_uiController.SetColor(m_colorSelector.DrawColor);
        m_colorSelector.OnSaturationChanged += m_uiController.SetSaturation;
        m_colorSelector.OnSaturationChanged += (val) => m_uiController.SetColor(m_colorSelector.DrawColor);
        m_colorSelector.OnValueChanged += m_uiController.SetValue;
        m_colorSelector.OnValueChanged += (val) => m_uiController.SetColor(m_colorSelector.DrawColor);
        m_colorSelector.OnAlphaChanged += m_uiController.SetAlpha;
        m_colorSelector.OnAlphaChanged += (val) => m_uiController.SetColor(m_colorSelector.DrawColor);
        m_colorSelector.SetColor(m_selectedColor);

    }

    /// <summary>
    /// Toggles Rectangle tool
    /// </summary>
    private void SetUpRectangle()
    {
        m_currentTool = new RectangleTool(m_drawingRenderer, m_colorSelector);
        m_uiController.ClearCanvasPointerEvents();
        SetUpDrawOnReleaseEvents();
    }

    /// <summary>
    /// Modified brush size.
    /// </summary>
    private void DecreaseBrushSize()
    {
        m_drawingRenderer.SetBrushSize(m_drawingRenderer.BrushSize - 1);
        m_uiController.SetBrushSize(m_drawingRenderer.BrushSize);
    }

    /// <summary>
    /// Modifies brush size.
    /// </summary>
    private void IncreaseBrushSize()
    {
        m_drawingRenderer.SetBrushSize(m_drawingRenderer.BrushSize + 1);
        m_uiController.SetBrushSize(m_drawingRenderer.BrushSize);
    }

    /// <summary>
    /// Toggles the Eraser tool
    /// </summary>
    private void SetEraser()
    {
        m_currentTool = new EraserTool(m_drawingRenderer);
        m_uiController.ClearCanvasPointerEvents();
        SetUpDrawOnClickEvents();
    }

    private void SetColorPicker()
    {
        m_currentTool = new ColorPickerTool(m_drawingRenderer, m_colorSelector);
        m_uiController.ClearCanvasPointerEvents();
        SetUpDrawOnClickEvents();
    }

    /// <summary>
    /// Toggles the Line tool
    /// </summary>
    private void SetLine()
    {
        m_currentTool = new LineTool(m_drawingRenderer, m_colorSelector);
        m_uiController.ClearCanvasPointerEvents();
        SetUpDrawOnReleaseEvents();

    }

    /// <summary>
    /// Connects Line and Rectangle tools with the UIController to correctly draw only when user lets go of the mouse button
    /// </summary>
    private void SetUpDrawOnReleaseEvents()
    {
        m_uiController.OnPointerDown += StartDrawing;
        m_uiController.OnPointerDown += SetDrawingStartPosition;
        m_uiController.OnPointerReleased += Draw;
        m_uiController.OnPointerReleased += (position) => StopDrawing();
        m_uiController.OnPointerOut += StopDrawing;
        m_uiController.OnPointerOut += ClearPreview;
        m_uiController.OnPointerOut += StopDrawingPreview;


        m_uiController.OnPointerMoved += (pos) =>
        {
            if (m_isDrawing == false)
            {
                m_drawingStartPointInNormalizedCoordinates = pos;
            }
        };

        m_uiController.OnPointerEntered += (pos) => StartDrawingPreview();
    }


    /// <summary>
    /// Connects Brush and Eraser tools with the UIController to correctly draw when user clicks and drags the mouse
    /// </summary>
    private void SetUpDrawOnClickEvents()
    {
        m_uiController.OnPointerDown += SetDrawingStartPosition;
        m_uiController.OnPointerDown += StartDrawing;
        m_uiController.OnPointerDown += Draw;
        m_uiController.OnPointerMoved += (pos) =>
        {
            if (m_isDrawing)
            {
                Draw(pos);
            }
        };
        m_uiController.OnPointerReleased += (position) => StopDrawing();
        m_uiController.OnPointerOut += StopDrawing;
        m_uiController.OnPointerEntered += (pos) => StartDrawingPreview();
        m_uiController.OnPointerOut += ClearPreview;
        m_uiController.OnPointerOut += StopDrawingPreview;
        m_uiController.OnPointerOut += StopDrawing;
    }

    /// <summary>
    /// Helper method to start drawing. Something that I will want to refactor inside the DrawingTool class.
    /// </summary>
    /// <param name="vector"></param>
    private void StartDrawing(Vector2 vector)
    {
        m_isDrawing = true;
    }

    /// <summary>
    /// Sets the brush size
    /// </summary>
    private void SetBrush()
    {
        m_currentTool = new BrushTool(m_drawingRenderer, m_colorSelector);
        m_uiController.ClearCanvasPointerEvents();
        SetUpDrawOnClickEvents();
    }

    /// <summary>
    /// Clears preview canvas (separate RenderTexture)
    /// </summary>
    private void ClearPreview()
    {
        m_drawingRenderer.ClearPreview();
    }

    /// <summary>
    /// Enables drawing preview
    /// </summary>
    /// <param name="normalizedPixelPosition"></param>
    private void DrawPreview(Vector2 normalizedPixelPosition)
    {
        m_currentTool.CreatePreview(m_drawingStartPointInNormalizedCoordinates, normalizedPixelPosition);
    }

    /// <summary>
    /// Connects Preview Drawing to the UIController
    /// </summary>
    private void StartDrawingPreview()
    {
        m_uiController.OnPointerMoved += DrawPreview;
    }
    /// <summary>
    /// Disconnects Preview Drawing to the UIController
    /// </summary>
    private void StopDrawingPreview()
    {
        m_uiController.OnPointerMoved -= DrawPreview;
    }

    /// <summary>
    /// Clears canvas (main RenderTexture)
    /// </summary>
    private void ClearDrawing()
    {
        m_drawingRenderer.ClearCanvas();
    }

    //Manage drawing process
    private void StopDrawing()
    {
        m_isDrawing = false;
        m_uiController.OnPointerMoved -= Draw;
    }

    /// <summary>
    /// Handles drawing input
    /// </summary>
    /// <param name="normalizedPixelPosition"></param>
    private void Draw(Vector2 normalizedPixelPosition)
    {
        if (m_isDrawing == false)
        {
            return;
        }
        m_currentTool.ApplyToDrawing(m_drawingStartPointInNormalizedCoordinates, normalizedPixelPosition);
    }

    /// <summary>
    /// Helper method to set the drawing start position. Useful for Rectangle and line tools. Probably will refactor it to be inside those classes.
    /// </summary>
    /// <param name="normalizedPixelPosition"></param>
    private void SetDrawingStartPosition(Vector2 normalizedPixelPosition)
    {
        m_drawingStartPointInNormalizedCoordinates = normalizedPixelPosition;
    }
}
