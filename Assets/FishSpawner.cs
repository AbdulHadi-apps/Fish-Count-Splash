using UnityEngine;
using System.Collections.Generic;

public class FishSpawner : MonoBehaviour
{
    public GameObject[] fishPrefabs; // Assign 18 fish prefabs
    public Transform fishParent; // Parent for cleanup
    public int currentFishCount;

    public void SpawnFish(int fishCount)
    {
        ClearOldFish(); // Clean before spawning

        currentFishCount = Mathf.Clamp(fishCount, 1, fishPrefabs.Length);
        List<GameObject> available = new List<GameObject>(fishPrefabs);

        for (int i = 0; i < currentFishCount; i++)
        {
            int index = Random.Range(0, available.Count);
            GameObject fish = Instantiate(available[index], GetRandomPos(), Quaternion.identity, fishParent);
            fish.AddComponent<FishMover>();
            available.RemoveAt(index); // No duplicates
        }
    }

    void ClearOldFish()
    {
        foreach (Transform child in fishParent)
            Destroy(child.gameObject);
    }

    Vector3 GetRandomPos()
    {
        float z = 10f;
        Vector3 view = new Vector3(Random.Range(0.1f, 0.9f), Random.Range(0.3f, 0.9f), z);
        return Camera.main.ViewportToWorldPoint(view);
    }
}
