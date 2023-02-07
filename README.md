# Dodge Sword

## 📖SUMMARY

- 하늘에서 떨어지는 칼을 피해 끝까지 살아남는 게임입니다.
    
    끝까지 살아남아 최고점을 달성하세요!
    
[Google Play 출시](https://play.google.com/store/apps/details?id=com.cheezeomelette.DodgeSword)

![dodgeSword게임_시작.png](Dodge%20Sword%20ba2f0c024cf4432f91602c95cdd9ed6b/dodgeSword%25EA%25B2%258C%25EC%259E%2584_%25EC%258B%259C%25EC%259E%2591.png)

## 💡주요기능

### 🗡️검 생성

- 검이 생성되는 위치를 균일하게(집중적으로 생성되지 않게) 하기 위한 함수
- 리스트에 균일한 생성위치를 만들어두고 한번 생성된 위치는 리스트의 가장 뒤로 보낸 후 리스트에서 추출할 인덱스의 최대값을 한칸 앞당긴다.
- 생성될 때 랜덤한 오프셋을 더해서 같은위치에 떨어지지 않게 한다.

```csharp
// 초기 생성위치 리스트 초기화
private void ListInit()
{
    // 1단위로 균일하게 칼 생성위치 리스트를 만든다.
    for (int i = 0; i < locationList.Capacity; i++)
        locationList.Add(i);
}
```

```csharp
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
```

### 📦오브젝트 관리

- 검 오브젝트를 재사용하여 메모리 낭비를 줄이기 위해 오브젝트 풀링을 사용했다.
- 비활성화된 오브젝트를 담아 둘 스택을 생성하고 필요할 때마다 GetPool()함수를 사용해서 스택에서 꺼내온다. 만약 스택이 비어있다면 CreatePool()함수를 사용해서 새로운 오브젝트를 만든다.
- 사용한 오브젝트는 다시 스택에 넣어준다.

```csharp
public Sword GetPool()
{
    // 만약 스택의 개수가 0이하라면
    if (storageStack.Count <= 0)
        CreatePool();              // 추가한다
    // 스택에서 가져온다
    Sword poop = storageStack.Pop();
    poop.transform.SetParent(transform);
    return poop;
}
public void ReturnPool(Sword poop)
{
    // 다시 비활성화 오브젝트 하위에 두고 스택에 넣는다.
    poop.transform.SetParent(storage);
    storageStack.Push(poop);
}
```

### ⏰오브젝트 전용 시간 생성

- 사망 연출을 할 때 Time.timeScale을 조절했는데 의도하지 않은 UI 속도에도 영향이 생겼다.
- 이를 해결하기 위해 오브젝트의 시간을 담당할 변수를 만들어 기존 시간과 분리해서 사용하기로 했다
- 기존의 Time.deltaTime을 활용해서 오브젝트의 움직임에 사용할 시간을 새로 정의해서 사용한다.

```csharp
public static class ObjectTime 
{
    public static float timeScale = 1f;
    public static float deltaTime => Time.deltaTime * timeScale;
}
```

- 칼이 떨어지는 것을 구현한 함수이다.
```csharp
private void Update()
{
    velocity += ObjectTime.deltaTime * (acc + randAcc);
    transform.position += new Vector3(0, ObjectTime.deltaTime * velocity, 0);
}
```