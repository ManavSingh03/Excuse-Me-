using UnityEngine;
    [RequireComponent(typeof(Collider2D))]
public class PersonClickDebugger : MonoBehaviour
{
    void OnMouseEnter() { Debug.Log($"[PersonClickDebugger] OnMouseEnter: {name}"); }
    void OnMouseExit()  { Debug.Log($"[PersonClickDebugger] OnMouseExit: {name}"); }

    void OnMouseDown()  { Debug.Log($"[PersonClickDebugger] OnMouseDown: {name}"); }
    void OnMouseUp()    { Debug.Log($"[PersonClickDebugger] OnMouseUp: {name}"); }
    void OnMouseDrag()  { Debug.Log($"[PersonClickDebugger] OnMouseDrag: {name}"); }
}