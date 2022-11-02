using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Į ������Ʈ Ǯ��
public class SwordManager : MonoBehaviour
{
    // �̱�������
    static SwordManager instance;
    public static SwordManager Instance => instance;

    // ��Ȱ��ȭ�ص� ������ġ
    [SerializeField] Transform storage;
    [SerializeField] Sword prefab;
    [SerializeField] int createCount;

    // ����� ������Ʈ ����
    Stack<Sword> storageStack;

    private void Awake()
    {
        instance = this;

        // ��Ȱ��ȭ �صд�.
        storage.gameObject.SetActive(false);

        // ���� ��ü ����.
        storageStack = new Stack<Sword>();
    }

    private void Start()
    {
        // ���۽� �̸� Ǯ�� �����д�
        for (int i = 0; i < createCount; i++)
            CreatePool();
    }

    private void CreatePool()
    {
        // storage������Ʈ�� ������ prefab�� clone���� �����Ѵ�.
        Sword newPoop = Instantiate(prefab, storage);
        storageStack.Push(newPoop);
    }
    public Sword GetPool()
    {
        // ���� ������ ������ 0���϶��
        if (storageStack.Count <= 0)
            CreatePool();              // �߰��Ѵ�

        // ���ÿ��� �����´�
        Sword poop = storageStack.Pop();
        poop.transform.SetParent(transform);
        return poop;
    }
    public void ReturnPool(Sword poop)
    {
        // �ٽ� ��Ȱ��ȭ ������Ʈ ������ �ΰ� ���ÿ� �ִ´�.
        poop.transform.SetParent(storage);
        storageStack.Push(poop);
    }
}
