using System.Collections.Generic;
using UnityEngine;

public class SeatManager : MonoBehaviour
{
    public static SeatManager Instance;

    [Header("Seat Setup")]
    public List<Seat> seats = new List<Seat>();

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

    // Find nearest free *valid* seat
    public Seat GetNearestSeat(Vector3 position)
    {
        Seat nearest = null;
        float minDist = Mathf.Infinity;

        foreach (var seat in seats)
        {
            if (!seat.IsAvailable()) continue;

            float dist = Vector3.Distance(position, seat.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = seat;
            }
        }

        return nearest;
    }

    // Try to occupy a seat
    public bool TryOccupySeat(Seat seat, Person person)
    {
        if (seat == null || person == null) return false;

        if (seat.IsAvailable())
        {
            seat.SetOccupied(true, person);
            return true;
        }

        return false;
    }

    // Free a seat
    public void FreeSeat(Seat seat)
    {
        if (seat == null) return;
        seat.SetOccupied(false);
    }
}
