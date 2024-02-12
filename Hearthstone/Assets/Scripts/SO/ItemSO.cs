using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // 이 클래스의 인스턴스를 Unity에서 직렬화 할 수 있게 합니다.
public class Item {
    public string name;
    public int attack;
    public int health;
    public Sprite sprite;
    public float percent;
}

[CreateAssetMenu(fileName = "ItemSO", menuName = "Scriptable Object/ItemSO")]

// ItemSO 클래스는 Unity의 ScriptableObject를 상속받습니다. 
// ScriptableObject : 게임에 특정 데이터를 저장하거나 공유하는 데 사용하는 클래스로, 일반적으로 게임 설정, AI 상태, 인벤토리 아이템 등을 저장하는 데 사용됩니다.
public class ItemSO : ScriptableObject {
    public Item[] items; // items는 Item 타입의 객체들을 담는 배열입니다. Item 클래스의 인스턴스들, 즉 Item 객체들이 items 배열에 저장됩니다.
}