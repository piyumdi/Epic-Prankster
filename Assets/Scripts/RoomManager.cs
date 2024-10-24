/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public List<GameObject> rooms; // List of room prefabs
    public Transform roomPosition; // The fixed position where rooms will appear
    private GameObject currentRoom;
    private int roomIndex = 0;

    private void Start()
    {
        if (rooms.Count > 0)
        {
            SpawnRoom(roomIndex); // Start with the first room
            StartCoroutine(SwitchRoom()); // Begin the room-switching process
        }
    }

    private void SpawnRoom(int index)
    {
        if (currentRoom != null)
        {
            Destroy(currentRoom); // Destroy the current room
        }

        // Instantiate the new room at the fixed position with 180 degrees Y rotation
        currentRoom = Instantiate(rooms[index], roomPosition.position, Quaternion.Euler(0, 180, 0));
    }

    private IEnumerator SwitchRoom()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f); // Wait for 5 seconds

            roomIndex = (roomIndex + 1) % rooms.Count; // Loop through the rooms
            SpawnRoom(roomIndex); // Spawn the next room
        }
    }
}
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Make sure to include this for UI handling

public class RoomManager : MonoBehaviour
{
    public static RoomManager instance;
    public List<GameObject> rooms; // List of room prefabs
    public Transform roomPosition; // The fixed position where rooms will appear
    public Button nextLevelButton; // Reference to the "Next Level" button
    private GameObject currentRoom;
    private int roomIndex = 0;

    private void Start()
    {
        // Get the current level from PlayerPrefs, default to 0 if not set
        roomIndex = GetLevel();

        // Spawn the first room or the current saved level
        if (rooms.Count > 0)
        {
            SpawnRoom(roomIndex);
        }

        // Add listener to the button to trigger room change
        nextLevelButton.onClick.AddListener(ChangeRoom);
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Optional: If you want to persist the RoomManager across scenes
        }
        else
        {
            Destroy(gameObject); // Ensure there's only one instance
        }
    }

    private void SpawnRoom(int index)
    {
        if (currentRoom != null)
        {
            Destroy(currentRoom); // Destroy the current room
        }

        // Instantiate the new room at the fixed position with 180 degrees Y rotation
        currentRoom = Instantiate(rooms[index], roomPosition.position, Quaternion.Euler(0, 180, 0));
    }

    private void ChangeRoom()
    {
        roomIndex = (roomIndex + 1) % rooms.Count; // Loop through the rooms
        SpawnRoom(roomIndex); // Spawn the next room
        SaveLevel(roomIndex); // Save the current room as the current level
        

    }

    private void SaveLevel(int currentLevel)
    {
        
        //PlayerPrefs.SetInt("Level", currentLevel);
        PlayerPrefs.Save();
    }
 
    public int GetLevel()
    {
        
        return PlayerPrefs.GetInt("Level", 0);
    }
}
