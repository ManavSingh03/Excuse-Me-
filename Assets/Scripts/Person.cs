using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Person : MonoBehaviour
{
    private Vector3 offset;
    private bool isDragging = false;

    private Vector3 lastValidPosition;
    private Vector3 seatTarget;

    private float snapSpeed = 10f;

    private void Start()
    {
        lastValidPosition = transform.position;
    }

    private void OnMouseDown()
    {
        isDragging = true;
        offset = transform.position - GetMouseWorldPos();
        CursorController.Instance.SetCursor(CursorState.Dragging);

        // Free seat since we’re moving
        SeatManager.Instance.FreeSeat(lastValidPosition);
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
        CursorController.Instance.SetCursor(CursorState.Normal);

        // Find nearest seat (grid tile with Seat tag)
        Collider2D nearestSeat = FindNearestSeat();
        if (nearestSeat != null)
        {
            Vector3 seatPos = nearestSeat.transform.position;

            if (!SeatManager.Instance.IsSeatOccupied(seatPos))
            {
                // Snap to seat with animation
                seatTarget = seatPos;
                lastValidPosition = seatPos;
                StartCoroutine(SnapToSeat(seatPos));

                SeatManager.Instance.TryOccupySeat(seatPos, this);
                return;
            }
            else
            {
                CursorController.Instance.SetCursor(CursorState.Blocked);
            }
        }

        // If no valid seat → revert
        StartCoroutine(SnapToSeat(lastValidPosition));
        SeatManager.Instance.TryOccupySeat(lastValidPosition, this);
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mouse = Input.mousePosition;
        mouse.z = 10f; // distance from camera
        return Camera.main.ScreenToWorldPoint(mouse);
    }

    private System.Collections.IEnumerator SnapToSeat(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * snapSpeed);
            yield return null;
        }
        transform.position = target;
    }

    private Collider2D FindNearestSeat()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1f);
        Collider2D nearest = null;
        float minDist = float.MaxValue;

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Seat"))
            {
                float dist = Vector3.Distance(transform.position, hit.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = hit;
                }
            }
        }

        return nearest;
    }
}
