using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 칼 오브젝트 풀링
public class SwordPool : MonoBehaviour
{
    // 싱글톤패턴
    static SwordPool instance;
    public static SwordPool Instance => instance;

    // 비활성화해둔 저장위치
    [SerializeField] Transform storage;
    [SerializeField] Sword prefab;
    [SerializeField] int createCount;

    // 사용한 오브젝트 스택
    Stack<Sword> storageStack;

    private void Awake()
    {
        instance = this;

        // 비활성화 해둔다.
        storage.gameObject.SetActive(false);

        // 스택 객체 생성.
        storageStack = new Stack<Sword>();
    }

    private void Start()
    {
        // 시작시 미리 풀을 만들어둔다
        for (int i = 0; i < createCount; i++)
            CreatePool();
    }

    private void CreatePool()
    {
        // storage오브젝트의 하위에 prefab을 clone으로 생성한다.
        Sword newSword = Instantiate(prefab, storage);
        storageStack.Push(newSword);
    }
    public Sword GetPool()
    {
        // 만약 스택의 개수가 0이하라면
        if (storageStack.Count <= 0)
            CreatePool();              // 추가한다

        // 스택에서 가져온다
        Sword sword = storageStack.Pop();
        sword.transform.SetParent(transform);
        return sword;
    }
    public void ReturnPool(Sword sword)
    {
        // 다시 비활성화 오브젝트 하위에 두고 스택에 넣는다.
        sword.transform.SetParent(storage);
        storageStack.Push(sword);
    }
}
