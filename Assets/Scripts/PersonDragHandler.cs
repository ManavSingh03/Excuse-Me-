using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class PersonDragHandler : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public float snapSpeed = 10f;

    Vector3 offset;
    Vector3 lastValidPosition;
    Seat lastSeat;
    Camera cam;
    Coroutine snapRoutine;

    void Awake()
    {
        cam = Camera.main;
        if (!cam) Debug.LogError("[PersonDragHandler] No camera tagged MainCamera found!");
    }

    void Start()
    {
        lastValidPosition = transform.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (CursorController.Instance) CursorController.Instance.SetCursor(CursorState.Dragging);
        Vector3 wp = cam.ScreenToWorldPoint(eventData.position);
        offset = transform.position - new Vector3(wp.x, wp.y, transform.position.z);
        Debug.Log("[PersonDragHandler] PointerDown");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("[PersonDragHandler] BeginDrag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 wp = cam.ScreenToWorldPoint(eventData.position);
        Vector3 target = new Vector3(wp.x, wp.y, transform.position.z) + offset;
        transform.position = target;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (CursorController.Instance) CursorController.Instance.SetCursor(CursorState.Normal);

        Seat nearestSeat = SeatManager.Instance.GetNearestSeat(transform.position);
        if (nearestSeat != null && SeatManager.Instance.TryOccupySeat(nearestSeat, GetComponent<Person>()))
        {
            if (lastSeat != null) SeatManager.Instance.FreeSeat(lastSeat);

            lastSeat = nearestSeat;
            lastValidPosition = nearestSeat.transform.position;
            StartSnap(nearestSeat.transform.position);
            Debug.Log("[PersonDragHandler] EndDrag -> snapped to new seat");
        }
        else
        {
            StartSnap(lastValidPosition);
            Debug.Log("[PersonDragHandler] EndDrag -> reverted to last valid");
        }
    }

    void StartSnap(Vector3 target)
    {
        if (snapRoutine != null) StopCoroutine(snapRoutine);
        snapRoutine = StartCoroutine(SnapTo(target));
    }

    IEnumerator SnapTo(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * snapSpeed);
            yield return null;
        }
        transform.position = target;
    }
}
