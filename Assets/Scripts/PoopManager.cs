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

        // Ǯ�� ������Ʈ�� ���� ��ġ.
        storageParent = new GameObject("storage").transform;
        storageParent.SetParent(transform);
        storageParent.gameObject.SetActive(false);

        // ���� ��ü ����.
        storageStack = new Stack<Poop>();
    }
    private void Start()
    {
        for (int i = 0; i < createCount; i++)
            CreatePool();
    }

    private void CreatePool()
    {
        // storageParent������Ʈ�� ������ prefab�� clone���� �����Ѵ�.
        Poop newPoop = Instantiate(prefab, storageParent);
        storageStack.Push(newPoop);
    }
    public Poop GetPool()
    {
        // ���� ������ ������ 0���϶��
        if (storageStack.Count <= 0)
            CreatePool();

        // ���ÿ��� SE�� ������ �θ� ������Ʈ�� ���� ������ �ڿ� ��ȯ.
        Poop poop = storageStack.Pop();
        poop.transform.SetParent(transform);
        return poop;
    }
    public void ReturnPool(Poop poop)
    {
        // �ٽ� ��Ȱ��ȭ ������Ʈ ������ �ΰ� ���ÿ� �ִ´�.
        poop.transform.SetParent(storageParent);
        storageStack.Push(poop);
    }
}
