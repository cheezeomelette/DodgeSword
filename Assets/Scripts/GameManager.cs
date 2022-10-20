using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject poop;
    [SerializeField] Transform poops;
    [SerializeField] Text ScoreText;
    [SerializeField] Player player;
	public GameObject GameOverPanel;

    private static GameManager instance;
    public static GameManager Instance => instance;
    public bool isGameOver;
    public bool isDead;
    public int score;
    public int level;
    public float coolTime;

    List<int> locationList;
    const float COOLTIME = 0.1f;
    float currentTime;
    int count;
    int bestScore;

    private void Awake()
	{
        instance = this;
        locationList = new List<int>(30);
	}
	void Start()
    {
        bestScore = PlayerPrefs.GetInt("BestScore");
        ListInit();
        ReStart();
        UpdateUI();
        level = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if(isGameOver)
		{
            if (Input.GetKeyDown(KeyCode.R))
                SceneManager.LoadScene(0);
        }
        currentTime += Time.deltaTime;
        if(coolTime < currentTime)
		{
            currentTime = 0f;
            GeneratePoop();
		}
    }

    private void ListInit()
	{
        for (int i = 0; i < locationList.Capacity; i++)
            locationList.Add(i);
	}

    private float GetRandomLocation()
	{
        if (count <= 0)
		{
            count = locationList.Count;
		}
        int randIndex = Random.Range(0, count);
        float offset = Random.Range(-0.5f, 0.6f);
        int randLocation = locationList[randIndex];
        Swap(locationList, randIndex, count-1);
        count--;

        return randLocation + offset;
	}

    private void GeneratePoop()
	{
        float location = GetRandomLocation();
        Poop poop = PoopManager.Instance.GetPool();
        poop.Setup();
        poop.transform.position = new Vector3(location, 20);
        //Instantiate(poop, new Vector3(location, 20),Quaternion.identity, poops);
	}

    private void Swap(List<int> list, int from, int to)
    {
        int tmp = list[from];
        list[from] = list[to];
        list[to] = tmp;
    }

	public void GetScore()
	{
        score += 10 + level;
        ChangeLevel();
    }

    private void ChangeLevel()
    {
        for (; level < 19; level++)
        {
            if (GameManager.Instance.score < 350 * (level * 1.7 + 1))
            {
                return;
            }
        }
    }

    public void UpdateUI()
	{
        ScoreText.text = string.Format($"Best : {bestScore}\nScore : {score}");
	}

    public void LevelDesign()
	{
        if (coolTime < 0.03)
            return;
        coolTime -= 0.0001f;
	}

    public void IsDead()
	{
        isDead = true;
        if(PlayerPrefs.GetInt("BestScore") < score)
            PlayerPrefs.SetInt("BestScore", score);
        player.IsDead();
	}

    private void ReStart()
	{
        score = 0;
        currentTime = 0f;
        coolTime = COOLTIME;
        count = locationList.Count;
        GameOverPanel.SetActive(false);
        Time.timeScale = 1f;
        isGameOver = false;
        isDead = false;
    }

    public bool Getinvincibility()
	{
        return player.invincibility;
	}
}
