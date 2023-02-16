using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordGenerator : MonoBehaviour
{
    [SerializeField] Transform swordsTransform;
    // 칼 생성 주기
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
        // 쿨타임이 돌면
        if (GameManager.Instance.SwordDropCoolTime < currentTime)
        {
            // 칼 생성
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
        // 이전에 남아있던 칼은 회수한다
        ReturnSwords();
        count = locationList.Count;
    }
    // 초기 생성위치 리스트 초기화
    private void ListInit()
    {
        // 1단위로 균일하게 칼 생성위치 리스트를 만든다.
        for (int i = 0; i < locationList.Capacity; i++)
            locationList.Add(i);
    }

    // 랜덤하게 칼을 생성하는 함수
    private float GetRandomLocation()
    {
        // 리스트에 값이 없다면 다시 채운다
        if (count <= 0)
        {
            count = locationList.Count;
        }
        // 랜덤한 리스트의 인덱스 생성
        int randIndex = Random.Range(0, count);
        // 생성위치에서 약간씩 틀어지게 하기위한 offset
        float offset = Random.Range(-0.5f, 0.6f);
        // 랜덤한 위치
        int randLocation = locationList[randIndex];
        // 같은 위치에서 연속해서 나오지않고 균일하게 하기 위해 나온 위치는 리스트의 제일뒤로 보내고 
        // 제일뒤의 위치인 count를 한칸 당겨준다
        Swap(locationList, randIndex, count - 1);
        count--;

        // 랜덤한 위치에 offset을 더한다
        return randLocation + offset;
    }

    // 칼 생성
    private void GenerateSword()
    {
        // 랜덤한 위치를 가져온다
        float location = GetRandomLocation();
        // 오브젝트 풀에서 칼 오브젝트를 가져온다
        Sword sword = SwordPool.Instance.GetPool();
        // 칼 초기세팅
        sword.Setup();
        // 칼 위치 세팅
        sword.transform.position = new Vector3(location, 20);
    }

    // 리스트 스왑함수
    private void Swap(List<int> list, int from, int to)
    {
        int tmp = list[from];
        list[from] = list[to];
        list[to] = tmp;
    }

    // 칼 리턴 함수
    private void ReturnSwords()
    {
        // 칼이 남아 있는 동안
        while (swordsTransform.childCount > 0)
        {
            // 칼이 있는 transform의 첫번째를 오브젝트 풀에 리턴시킨다
            Sword sword = swordsTransform.GetChild(0).GetComponent<Sword>();
            SwordPool.Instance.ReturnPool(sword);
        }
    }
}
