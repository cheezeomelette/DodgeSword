using GooglePlayGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // 메뉴화면 오브젝트
    [SerializeField] UIManager uiManager;
    [SerializeField] Text ScoreText;
    // 칼 생성 transform
    [SerializeField] Transform swordsTransform;
	[SerializeField] SwordGenerator swordGenerator;
    [SerializeField] ItemGenerator itemGenerator;

    // 플레이어 시작 transform
    [SerializeField] Transform startTransform;
    [SerializeField] Player player;

    [SerializeField] CharacterManager characterManager;

    // 싱글톤
    private static GameManager instance;
    public static GameManager Instance => instance;

    int coin = 0;
    private int score;
    // 칼 생성 주기와 스프라이트를 바꿀 기준
    private int level;
    public int Level => level;

    // 칼 생성 주기
    private float swordDropCoolTime = 0.1f;
    public float SwordDropCoolTime => swordDropCoolTime;

    // 아이템 생성 주기
    private float itemDropCool = 5f;
    public float ItemDropCool => itemDropCool;

    const float COOLTIME = 0.1f;
    float time;
    bool isPlaying = false;
    int bestScore;

    private void Awake()
    {
        instance = this;
        // 30개의 생성위치 리스트
    }
    void Start()
    {
        // 프레임 설정
        Application.targetFrameRate = 60;

        SetLobby();
        UpdateUI();
    }

	private void Update()
	{
        if (!isPlaying)
            return;
        time += ObjectTime.deltaTime;
        uiManager.UpdateTime(time);
	}
	// 점수 획득 함수
	public void GetScore()
    {
        if (player.IsDead)
            return;
        score += 10 + level;
        bestScore = Mathf.Max(bestScore, score);
        // 난이도 조절
        ChangeLevel();
    }

    public void GetCoin()
	{
        coin += 1;

	}
    // 칼 생성 속도를 앞당긴다
    public void GenerateSwordFaster()
    {
        if (swordDropCoolTime < 0.03)
            return;
        swordDropCoolTime -= 0.0001f;
    }


    // 점수 업데이트
    public void UpdateUI()
    {
        ScoreText.text = string.Format($"Best : {bestScore}\nScore : {score}");
    }


    // 죽었을 때
    public void Dead()
    {
        isPlaying = false;
        swordGenerator.StopGenerating();
        itemGenerator.StopGenerating();
		Invoke(nameof(ShowResult), 1.5f);
        
        // 최고점수 기록
        if (bestScore == score)
		{

            Social.ReportScore(score, "CgkI6YaVy7wNEAIQBQ", (bool success) => {
                
            });
        }
    }

    // 게임시작시
    public void StartGame()
    {
        // 초기세팅
        player.gameObject.SetActive(true);
        player.Respawn(startTransform.position);
        isPlaying = true;
        swordDropCoolTime = COOLTIME;
        swordGenerator.StartGenerating();
        itemGenerator.StartGenerating();
        characterManager.ClearLobbyCharacter();
        time = 0f;
        level = 0;
        score = 0;
        coin = 0;

        //UI Setting
        uiManager.SetIngame();
        swordGenerator.ClearSwords();
        itemGenerator.ClearItems();
        UpdateUI();

        // 칼을 떨어뜨리기 위한
        ObjectTime.timeScale = 1f;
    }

    public void PauseGame()
	{
        isPlaying = false;
        uiManager.PauseGame();
        ObjectTime.timeScale = 0f;
	}

    public void ContinueGame()
	{
        isPlaying = true;
        uiManager.ContinueGame();
        ObjectTime.timeScale = 1f;
    }

    public void SetLobby()
	{
        isPlaying = false;
        ObjectTime.timeScale = 1f;
        player.gameObject.SetActive(false);
        uiManager.SetLobby();
        characterManager.SetLobbyCharacter();
        swordGenerator.ClearSwords();
        itemGenerator.ClearItems();
    }

    public void SetOption()
	{
        uiManager.SetOption();
	}
    // 게임종료
    public void Quit()
    {
        Application.Quit();
    }

    public void SetBestScore()
	{
        ILeaderboard lb = PlayGamesPlatform.Instance.CreateLeaderboard();
        lb.id = GPGSIds.leaderboard;

        lb.LoadScores(scores =>
        {
            bestScore = (int)lb.localUserScore.value;
            UpdateUI();
        });
    }

    // 난이도 조절 함수
    private void ChangeLevel()
    {
        // 점수에 따라 레벨을 바꾼다.
        for (; level < 19; level++)
        {
            if (score < 350 * (level * 1.7 + 1))
                return;
        }
    }

    private void ShowResult()
	{
        Reward[] rewards= new Reward[] { new Reward(ItemSlotType.Coin, "Coin", coin) };
        player.StopAnim();
        ObjectTime.timeScale = 0f;
        uiManager.SetResult(score, bestScore, rewards);
	}
}
