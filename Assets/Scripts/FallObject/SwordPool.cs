using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Į ������Ʈ Ǯ��
public class SwordPool : MonoBehaviour
{
    // �̱�������
    static SwordPool instance;
    public static SwordPool Instance => instance;

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
        Sword newSword = Instantiate(prefab, storage);
        storageStack.Push(newSword);
    }
    public Sword GetPool()
    {
        // ���� ������ ������ 0���϶��
        if (storageStack.Count <= 0)
            CreatePool();              // �߰��Ѵ�

        // ���ÿ��� �����´�
        Sword sword = storageStack.Pop();
        sword.transform.SetParent(transform);
        return sword;
    }
    public void ReturnPool(Sword sword)
    {
        // �ٽ� ��Ȱ��ȭ ������Ʈ ������ �ΰ� ���ÿ� �ִ´�.
        sword.transform.SetParent(storage);
        storageStack.Push(sword);
    }
}
