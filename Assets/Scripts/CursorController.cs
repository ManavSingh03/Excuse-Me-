using UnityEngine;

public enum CursorState { Normal, Dragging, Blocked }

public class CursorController : MonoBehaviour
{
    public static CursorController Instance;

    [Header("Cursor Textures")]
    public Texture2D normalCursor;
    public Texture2D draggingCursor;
    public Texture2D blockedCursor;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void SetCursor(CursorState state)
    {
        Texture2D tex = normalCursor;
        Vector2 hotspot = Vector2.zero;

        switch (state)
        {
            case CursorState.Dragging:
                tex = draggingCursor;
                hotspot = new Vector2(tex.width / 2, tex.height / 2); // center
                break;
            case CursorState.Blocked:
                tex = blockedCursor;
                break;
        }

        Cursor.SetCursor(tex, hotspot, CursorMode.Auto);
    }
}
