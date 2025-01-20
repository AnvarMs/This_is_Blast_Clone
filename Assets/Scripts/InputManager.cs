
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // Check if there is at least one touch
        if (Input.touchCount > 0)
        {
            // Get the first touch
            Touch touch = Input.GetTouch(0);

            // Check if the touch phase is "Began" (equivalent to a mouse button down event)
            if (touch.phase == TouchPhase.Began)
            {
                // Create a ray from the touch position
                Ray ray = Camera.main.ScreenPointToRay(touch.position);

                // Perform a raycast
                if (Physics.Raycast(ray, out RaycastHit hitInfo))
                {
                    // Check if the object hit has a "Shooter" component
                    Shooter shooter = hitInfo.transform.GetComponent<Shooter>();

                    if (shooter != null)
                    {
                        // Trigger the fight logic
                        shooter.StartFight();
                    }
                }
            }
        }
    }
}
