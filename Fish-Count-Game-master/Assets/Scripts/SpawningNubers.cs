using UnityEngine;

public class SpawningNubers : MonoBehaviour
{
    public GameObject[] numbersPrefab;
    public GameObject[] fruitsPrefabs;
    public Transform spawnPoint1;
    public Transform spawnPoint2;
    public float spawnInterval = 2f;
    public float minY;
    public float maxY;
    public float minY1;
    public float maxY1;
    void Start()
    {
        InvokeRepeating(nameof(SpawnNumbers), 0f, spawnInterval);
    }

    public void SpawnNumbers()
    {
        int index = Random.Range(0, numbersPrefab.Length);
        int fruits = Random.Range(0, fruitsPrefabs.Length);

        float spawnPos = Random.Range(minY, maxY);
        float spawnPosyy = Random.Range(minY1, maxY1);

        Vector2 spawnPosY = new Vector2(spawnPoint1.position.x, spawnPos);
        Vector2 spawnPosy = new Vector2(spawnPoint2.position.x, spawnPosyy);

        Instantiate(numbersPrefab[index], spawnPosY,Quaternion.identity);
        Instantiate(fruitsPrefabs[index], spawnPosy, Quaternion.identity);

    }
}
