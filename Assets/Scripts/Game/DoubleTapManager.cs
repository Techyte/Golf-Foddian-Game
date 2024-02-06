namespace Game
{
    using UnityEngine;

    public class DoubleTapManager : MonoBehaviour
    {
        // Time threshold for double tap (in seconds)
        public float doubleTapTimeThreshold = 0.5f;

        // Variable to store the time of the last tap
        private float lastTapTime;

        // Update is called once per frame
        void Update()
        {
            // Check for touch input
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                // Check for the beginning of a touch
                if (touch.phase == TouchPhase.Began)
                {
                    // Calculate the time since the last tap
                    float timeSinceLastTap = Time.time - lastTapTime;

                    // Check if it's a double tap
                    if (timeSinceLastTap < doubleTapTimeThreshold)
                    {
                        // Double tap detected
                        Debug.Log("Double Tap Detected!");

                        PauseMenu.Instance.Toggle();
                    }

                    // Update the last tap time
                    lastTapTime = Time.time;
                }
            }
        }
    }   
}