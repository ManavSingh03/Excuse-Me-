using UnityEngine;

public class ClickRayProbe : MonoBehaviour
{
    private int personMask;

    private void Awake()
    {
        // Make sure we have a MainCamera
        if (Camera.main == null)
        {
            Debug.LogError("[ClickRayProbe] No camera tagged MainCamera found!");
            return;
        }

        // Prepare the mask to only hit the "Person" layer
        personMask = 1 << LayerMask.NameToLayer("Person");
        Debug.Log("[ClickRayProbe] Using LayerMask for Person objects.");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero, Mathf.Infinity, personMask);

            if (hit.collider != null)
            {
                Debug.Log("[ClickRayProbe] Clicked PERSON: " + hit.collider.name);
            }
            else
            {
                Debug.Log("[ClickRayProbe] No person at click.");
            }
        }
    }
}
