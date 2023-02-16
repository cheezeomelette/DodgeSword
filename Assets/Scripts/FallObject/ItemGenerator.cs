using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] itemPrefabs;
    [SerializeField] float[] probs = new float[2] { 10f, 90f };    // life 10, coin 90

    // 칼 생성 주기
    float currentTime;
    bool isPlaying = false;

    void Update()
    {
        if (isPlaying == false)
            return;
        currentTime += ObjectTime.deltaTime;
        // 쿨타임이 돌면
        if (GameManager.Instance.ItemDropCool < currentTime)
        {
            // 아이템 생성
            currentTime = 0f;
            GenerateRandomItem();
        }
    }

    public void StartGenerating()
    {
        currentTime = 0f;
        isPlaying = true;
    }
    public void StopGenerating()
    {
        isPlaying = false;
    }
    public void ClearItems()
	{
        foreach (Transform child in transform)
            Destroy(child.gameObject);
	}
    private void GenerateRandomItem()
	{
        float randomPoint = Random.Range(1f, 29f);
        GameObject newItem = GetRandomItem();
        Instantiate(newItem, new Vector2(randomPoint, 20), Quaternion.identity, transform);
	}
    private GameObject GetRandomItem()
	{
        float total = 0;
        foreach (float elem in probs)
            total += elem;

        float randomPoint = Random.value * total ;

        for (int i = 0; i < probs.Length; i++)
        {
            if (randomPoint < probs[i])
            {
                return itemPrefabs[ i];
            }
            else
            {
                randomPoint -= probs[i];
            }
        }

        return itemPrefabs[probs.Length - 1];

	}
}
