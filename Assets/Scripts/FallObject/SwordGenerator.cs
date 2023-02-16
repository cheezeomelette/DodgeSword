using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordGenerator : MonoBehaviour
{
    [SerializeField] Transform swordsTransform;
    // Į ���� �ֱ�
    float currentTime;
    int count;
    bool isPlaying = false;
    List<int> locationList;

    // Start is called before the first frame update
    void Start()
    {
        locationList = new List<int>(30);
        ListInit();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying == false)
            return;
        currentTime += ObjectTime.deltaTime;
        // ��Ÿ���� ����
        if (GameManager.Instance.SwordDropCoolTime < currentTime)
        {
            // Į ����
            currentTime = 0f;
            GenerateSword();
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
    public void ClearSwords()
    {
        // ������ �����ִ� Į�� ȸ���Ѵ�
        ReturnSwords();
        count = locationList.Count;
    }
    // �ʱ� ������ġ ����Ʈ �ʱ�ȭ
    private void ListInit()
    {
        // 1������ �����ϰ� Į ������ġ ����Ʈ�� �����.
        for (int i = 0; i < locationList.Capacity; i++)
            locationList.Add(i);
    }

    // �����ϰ� Į�� �����ϴ� �Լ�
    private float GetRandomLocation()
    {
        // ����Ʈ�� ���� ���ٸ� �ٽ� ä���
        if (count <= 0)
        {
            count = locationList.Count;
        }
        // ������ ����Ʈ�� �ε��� ����
        int randIndex = Random.Range(0, count);
        // ������ġ���� �ణ�� Ʋ������ �ϱ����� offset
        float offset = Random.Range(-0.5f, 0.6f);
        // ������ ��ġ
        int randLocation = locationList[randIndex];
        // ���� ��ġ���� �����ؼ� �������ʰ� �����ϰ� �ϱ� ���� ���� ��ġ�� ����Ʈ�� ���ϵڷ� ������ 
        // ���ϵ��� ��ġ�� count�� ��ĭ ����ش�
        Swap(locationList, randIndex, count - 1);
        count--;

        // ������ ��ġ�� offset�� ���Ѵ�
        return randLocation + offset;
    }

    // Į ����
    private void GenerateSword()
    {
        // ������ ��ġ�� �����´�
        float location = GetRandomLocation();
        // ������Ʈ Ǯ���� Į ������Ʈ�� �����´�
        Sword sword = SwordPool.Instance.GetPool();
        // Į �ʱ⼼��
        sword.Setup();
        // Į ��ġ ����
        sword.transform.position = new Vector3(location, 20);
    }

    // ����Ʈ �����Լ�
    private void Swap(List<int> list, int from, int to)
    {
        int tmp = list[from];
        list[from] = list[to];
        list[to] = tmp;
    }

    // Į ���� �Լ�
    private void ReturnSwords()
    {
        // Į�� ���� �ִ� ����
        while (swordsTransform.childCount > 0)
        {
            // Į�� �ִ� transform�� ù��°�� ������Ʈ Ǯ�� ���Ͻ�Ų��
            Sword sword = swordsTransform.GetChild(0).GetComponent<Sword>();
            SwordPool.Instance.ReturnPool(sword);
        }
    }
}
