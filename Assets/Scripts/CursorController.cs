using UnityEngine;

public enum CursorState { Normal, Dragging, Blocked }

public class CursorController : MonoBehaviour
{
    public static CursorController Instance;

    [Header("Cursor Textures")]
    public Texture2D normalCursor;
    public Texture2D draggingCursor;
    public Texture2D blockedCursor;

    [Header("Hotspots")]
    public Vector2 normalHotspot = Vector2.zero;
    public Vector2 draggingHotspot = Vector2.zero;
    public Vector2 blockedHotspot = Vector2.zero;

    private CursorState currentState = CursorState.Normal;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // optional
    }

    private void Start()
    {
        SetCursor(CursorState.Normal); // apply at game start
    }

    public void SetCursor(CursorState state)
    {
        if (state == currentState) return;
        currentState = state;

        Texture2D tex = normalCursor;
        Vector2 hotspot = normalHotspot;

        switch (state)
        {
            case CursorState.Dragging:
                tex = draggingCursor != null ? draggingCursor : normalCursor;
                hotspot = draggingHotspot;
                break;
            case CursorState.Blocked:
                tex = blockedCursor != null ? blockedCursor : normalCursor;
                hotspot = blockedHotspot;
                break;
        }

        if (tex != null)
            Cursor.SetCursor(tex, hotspot, CursorMode.Auto);
    }
}
