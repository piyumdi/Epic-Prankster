using System.Collections;
using UnityEngine;

public class RoomCameraController : MonoBehaviour
{
    public Transform cameraTransform;  // Assign the camera Transform here
    public float transitionSpeed = 5f; // Speed of camera movement
    public float stayDuration = 5f;    // Time to stay in each room

    public Transform[] roomMarkers;     // Assign the room markers in the Inspector
    private int currentRoomIndex = 0;   // Keeps track of the current room
    private Vector3 targetPosition;      // The target position for the camera
    private bool isMoving = false;

    public Vector3 cameraOffset = new Vector3(0, 5f, -10f); // Offset for adjusting camera position (modify as needed)

    private void Start()
    {
        // Start moving the camera between rooms
        StartCoroutine(SwitchRoomsAutomatically());
    }

    private void Update()
    {
        if (isMoving)
        {
            // Smoothly move the camera to the target position with the offset
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPosition + cameraOffset, Time.deltaTime * transitionSpeed);

            // If close enough to the target, stop moving
            if (Vector3.Distance(cameraTransform.position, targetPosition + cameraOffset) < 0.1f)
            {
                cameraTransform.position = targetPosition + cameraOffset;
                isMoving = false;
            }
        }
    }

    private IEnumerator SwitchRoomsAutomatically()
    {
        while (true)
        {
            // Move the camera to the position of the current room marker
            MoveToRoom(roomMarkers[currentRoomIndex].position);

            // Wait for the transition and stay duration
            yield return new WaitForSeconds(stayDuration + (1f / transitionSpeed));

            // Move to the next room (looping back to the first room if needed)
            currentRoomIndex = (currentRoomIndex + 1) % roomMarkers.Length;
        }
    }

    public void MoveToRoom(Vector3 newRoomPosition)
    {
        targetPosition = newRoomPosition;
        isMoving = true;
    }
}
