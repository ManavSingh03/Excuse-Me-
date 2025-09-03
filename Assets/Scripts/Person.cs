using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class Person : MonoBehaviour
{
    private Vector3 offset;
    private bool isDragging = false;

    private Vector3 lastValidPosition;
    private Seat lastSeat;
    private float snapSpeed = 10f;

    private Coroutine snapRoutine;

    private void Start()
    {
        lastValidPosition = transform.position;
    }

    private void OnMouseDown()
    {
        isDragging = true;
        offset = transform.position - GetMouseWorldPos();
        if (CursorController.Instance != null)
            CursorController.Instance.SetCursor(CursorState.Dragging);
      
        Debug.Log("[Person] CLICKED on " + gameObject.name);
    }

    private void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 newPos = GetMouseWorldPos() + offset;
            transform.position = newPos;
        }
    }

    private void OnMouseUp()
    {
        isDragging = false;
        if (CursorController.Instance != null)
            CursorController.Instance.SetCursor(CursorState.Normal);

        Seat nearestSeat = SeatManager.Instance.GetNearestSeat(transform.position);
        if (nearestSeat != null && SeatManager.Instance.TryOccupySeat(nearestSeat, this))
        {
            if (lastSeat != null) SeatManager.Instance.FreeSeat(lastSeat);

            lastSeat = nearestSeat;
            lastValidPosition = nearestSeat.transform.position;
            StartSnap(nearestSeat.transform.position);
        }
        else
        {
            StartSnap(lastValidPosition);
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mouse = Input.mousePosition;
        mouse.z = 10f;
        return Camera.main.ScreenToWorldPoint(mouse);
    }

    private IEnumerator SnapToSeat(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * snapSpeed);
            yield return null;
        }
        transform.position = target;
    }

    private void StartSnap(Vector3 target)
    {
        if (snapRoutine != null) StopCoroutine(snapRoutine);
        snapRoutine = StartCoroutine(SnapToSeat(target));
    }
}
