using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // 메뉴화면 오브젝트
    [SerializeField] GameObject menu;
    [SerializeField] GameObject swordPrefab;
    // 칼 생성 transform
    [SerializeField] Transform swordsTransform;
    // 플레이어 시작 transform
    [SerializeField] Transform startTransform;
    [SerializeField] Text ScoreText;
    [SerializeField] Player player;

    // 싱글톤
    private static GameManager instance;
    public static GameManager Instance => instance;
    public bool isGameOver;
    public bool isDead;
    public int score;
    // 칼 생성 주기와 스프라이트를 바꿀 기준
    public int level;
    // 칼 생성 주기
    public float coolTime;

    // 칼 생성 위치 리스트
    List<int> locationList;
    const float COOLTIME = 0.1f;
    float currentTime;
    int count;
    int bestScore;

    private void Awake()
	{
        instance = this;
        // 30개의 생성위치 리스트
        locationList = new List<int>(30);
	}
	void Start()
    {
        // 시작 시 칼이 떨어지지 않게 스케일을 0으로 맞춤
        Time.timeScale = 0f;
        // 메뉴화면 켜기
        menu.SetActive(true);
        // PlayerPrefs에 저장해둔 최고점수 가져옴
        bestScore = PlayerPrefs.GetInt("BestScore");
        // 리스트 세팅
        ListInit();
        UpdateUI();
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        // 쿨타임이 돌면
        if(coolTime < currentTime)
		{
            // 칼 생성
            currentTime = 0f;
            GenerateSword();
		}
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
        Swap(locationList, randIndex, count-1);
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
        Sword sword = SwordManager.Instance.GetPool();
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

    // 점수 획득 함수
	public void GetScore()
	{
        score += 10 + level;
        // 난이도 조절
        ChangeLevel();
    }

    // 난이도 조절 함수
    private void ChangeLevel()
    {
        // 점수에 따라 레벨을 바꾼다.
        for (; level < 19; level++)
        {
            if (GameManager.Instance.score < 350 * (level * 1.7 + 1))
            {
                return;
            }
        }
    }

    // 점수 업데이트
    public void UpdateUI()
	{
        ScoreText.text = string.Format($"Best : {bestScore}\nScore : {score}");
	}

    // 칼 생성 속도를 앞당긴다
    public void LevelDesign()
	{
        if (coolTime < 0.03)
            return;
        coolTime -= 0.0001f;
	}

    // 죽었을 때
    public void Dead()
	{
		isDead = true;
        // 최고점수 기록
        if(PlayerPrefs.GetInt("BestScore") < score)
            PlayerPrefs.SetInt("BestScore", score);
        // 플레이어 사망처리
        player.IsDead();
	}

    // 메뉴 화면 켜는 함수
    public void SetMenu()
	{
        menu.SetActive(true);
    }

    // 게임시작시
    public void StartGame()
	{
        // 이전에 남아있던 칼은 회수한다
        ReturnSwords();
        // 메뉴화면을 끈다
        menu.SetActive(false);

        // 초기세팅
        isGameOver = false;
        isDead = false;
        currentTime = 0f;
        level = 0;
        score = 0;
        coolTime = COOLTIME;
        count = locationList.Count;

        // 플레이어 초기 위치 설정
        player.transform.position = startTransform.position;
        // 칼을 떨어뜨리기 위한
        Time.timeScale = 1f;
    }

    // 칼 리턴 함수
    private void ReturnSwords()
	{
        // 칼이 남아 있는 동안
		while (swordsTransform.childCount > 0)
		{
            // 칼이 있는 transform의 첫번째를 오브젝트 풀에 리턴시킨다
            Sword sword = swordsTransform.GetChild(0).GetComponent<Sword>();
            SwordManager.Instance.ReturnPool(sword);
		}
	}

    // 게임종료
	public void Quit()
	{
        Application.Quit();
	}

    // 무적 체크
	public bool Getinvincibility()
	{
        return player.invincibility;
	}
}
