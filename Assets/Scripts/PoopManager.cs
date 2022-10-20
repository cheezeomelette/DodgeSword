using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoopManager : MonoBehaviour
{
    static PoopManager instance;
    public static PoopManager Instance => instance;

    [SerializeField] Poop prefab;
    [SerializeField] int createCount;

    Transform storageParent;
    Stack<Poop> storageStack;

    private void Awake()
    {
        instance = this;

        // 풀링 오브젝트의 저장 위치.
        storageParent = new GameObject("storage").transform;
        storageParent.SetParent(transform);
        storageParent.gameObject.SetActive(false);

        // 스택 객체 생성.
        storageStack = new Stack<Poop>();
    }
    private void Start()
    {
        for (int i = 0; i < createCount; i++)
            CreatePool();
    }

    private void CreatePool()
    {
        // storageParent오브젝트의 하위에 prefab을 clone으로 생성한다.
        Poop newPoop = Instantiate(prefab, storageParent);
        storageStack.Push(newPoop);
    }
    public Poop GetPool()
    {
        // 만약 스택의 개수가 0이하라면
        if (storageStack.Count <= 0)
            CreatePool();

        // 스택에서 SE를 꺼내고 부모 오브젝트를 나로 변경한 뒤에 반환.
        Poop poop = storageStack.Pop();
        poop.transform.SetParent(transform);
        return poop;
    }
    public void ReturnPool(Poop poop)
    {
        // 다시 비활성화 오브젝트 하위에 두고 스택에 넣는다.
        poop.transform.SetParent(storageParent);
        storageStack.Push(poop);
    }
}
