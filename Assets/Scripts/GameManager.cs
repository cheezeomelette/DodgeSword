using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // �޴�ȭ�� ������Ʈ
    [SerializeField] GameObject menu;
    [SerializeField] GameObject swordPrefab;
    // Į ���� transform
    [SerializeField] Transform swordsTransform;
    // �÷��̾� ���� transform
    [SerializeField] Transform startTransform;
    [SerializeField] Text ScoreText;
    [SerializeField] Player player;

    // �̱���
    private static GameManager instance;
    public static GameManager Instance => instance;
    public bool isGameOver;
    public bool isDead;
    public int score;
    // Į ���� �ֱ�� ��������Ʈ�� �ٲ� ����
    public int level;
    // Į ���� �ֱ�
    public float coolTime;

    // Į ���� ��ġ ����Ʈ
    List<int> locationList;
    const float COOLTIME = 0.1f;
    float currentTime;
    int count;
    int bestScore;

    private void Awake()
	{
        instance = this;
        // 30���� ������ġ ����Ʈ
        locationList = new List<int>(30);
	}
	void Start()
    {
        // ���� �� Į�� �������� �ʰ� �������� 0���� ����
        Time.timeScale = 0f;
        // �޴�ȭ�� �ѱ�
        menu.SetActive(true);
        // PlayerPrefs�� �����ص� �ְ����� ������
        bestScore = PlayerPrefs.GetInt("BestScore");
        // ����Ʈ ����
        ListInit();
        UpdateUI();
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        // ��Ÿ���� ����
        if(coolTime < currentTime)
		{
            // Į ����
            currentTime = 0f;
            GenerateSword();
		}
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
        Swap(locationList, randIndex, count-1);
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
        Sword sword = SwordManager.Instance.GetPool();
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

    // ���� ȹ�� �Լ�
	public void GetScore()
	{
        score += 10 + level;
        // ���̵� ����
        ChangeLevel();
    }

    // ���̵� ���� �Լ�
    private void ChangeLevel()
    {
        // ������ ���� ������ �ٲ۴�.
        for (; level < 19; level++)
        {
            if (GameManager.Instance.score < 350 * (level * 1.7 + 1))
            {
                return;
            }
        }
    }

    // ���� ������Ʈ
    public void UpdateUI()
	{
        ScoreText.text = string.Format($"Best : {bestScore}\nScore : {score}");
	}

    // Į ���� �ӵ��� �մ���
    public void LevelDesign()
	{
        if (coolTime < 0.03)
            return;
        coolTime -= 0.0001f;
	}

    // �׾��� ��
    public void Dead()
	{
		isDead = true;
        // �ְ����� ���
        if(PlayerPrefs.GetInt("BestScore") < score)
            PlayerPrefs.SetInt("BestScore", score);
        // �÷��̾� ���ó��
        player.IsDead();
	}

    // �޴� ȭ�� �Ѵ� �Լ�
    public void SetMenu()
	{
        menu.SetActive(true);
    }

    // ���ӽ��۽�
    public void StartGame()
	{
        // ������ �����ִ� Į�� ȸ���Ѵ�
        ReturnSwords();
        // �޴�ȭ���� ����
        menu.SetActive(false);

        // �ʱ⼼��
        isGameOver = false;
        isDead = false;
        currentTime = 0f;
        level = 0;
        score = 0;
        coolTime = COOLTIME;
        count = locationList.Count;

        // �÷��̾� �ʱ� ��ġ ����
        player.transform.position = startTransform.position;
        // Į�� ����߸��� ����
        Time.timeScale = 1f;
    }

    // Į ���� �Լ�
    private void ReturnSwords()
	{
        // Į�� ���� �ִ� ����
		while (swordsTransform.childCount > 0)
		{
            // Į�� �ִ� transform�� ù��°�� ������Ʈ Ǯ�� ���Ͻ�Ų��
            Sword sword = swordsTransform.GetChild(0).GetComponent<Sword>();
            SwordManager.Instance.ReturnPool(sword);
		}
	}

    // ��������
	public void Quit()
	{
        Application.Quit();
	}

    // ���� üũ
	public bool Getinvincibility()
	{
        return player.invincibility;
	}
}
