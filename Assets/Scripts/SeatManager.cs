using UnityEngine;
using System.Collections.Generic;

public class SeatManager : MonoBehaviour
{
    public static SeatManager Instance;

    private Dictionary<Vector3, Person> occupiedSeats = new Dictionary<Vector3, Person>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public bool TryOccupySeat(Vector3 seatPos, Person person)
    {
        if (occupiedSeats.ContainsKey(seatPos)) return false;

        occupiedSeats[seatPos] = person;
        return true;
    }

    public void FreeSeat(Vector3 seatPos)
    {
        if (occupiedSeats.ContainsKey(seatPos))
            occupiedSeats.Remove(seatPos);
    }

    public bool IsSeatOccupied(Vector3 seatPos)
    {
        return occupiedSeats.ContainsKey(seatPos);
    }
}
