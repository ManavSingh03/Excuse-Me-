using UnityEngine;

public enum SeatType { Valid, Invalid, VIP, Blocked }

public class Seat : MonoBehaviour
{
    public bool IsOccupied { get; private set; }
    public Person CurrentPerson { get; private set; }

    [Header("Seat Settings")]
    public SeatType seatType = SeatType.Valid;

    // optional grid coordinates for puzzle rules
    public int gridX;
    public int gridY;

    public bool IsAvailable()
    {
        return !IsOccupied && seatType == SeatType.Valid;
    }

    public void SetOccupied(bool occupied, Person person = null)
    {
        IsOccupied = occupied;

        if (occupied)
        {
            CurrentPerson = person;
        }
        else
        {
            CurrentPerson = null;
        }
    }
}
