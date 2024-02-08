using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour {
    public static CardManager Inst { get; private set; } // 매니저는 하나만 존재하기 때문에 싱글톤으로 선언.
    void Awake() => Inst = this;

    [SerializeField] ItemSO itemSO;
    [SerializeField] GameObject cardPrefab; 
    [SerializeField] List<Card> myCards; // 새로 추가
    [SerializeField] List<Card> otherCards; // 새로 추가

    List<Item> itemBuffer;

    public Item PopItem() {
        if (itemBuffer.Count == 0) { // 만약 카드를 다 뽑아 버퍼가 가지고 있는 카드의 갯수가 0개가 되면
            SetupItemBuffer(); // 다시 새로 버퍼에 100장의 카드를 셋팅
        }

        Item item = itemBuffer[0]; // 버퍼 맨 앞에 있는 카드를 뽑는다.
        itemBuffer.RemoveAt(0); // 뽑은 카드를 버퍼에서 지운다.
        return item; // 카드를 뽑아낸다.
    }

    void SetupItemBuffer() {
        itemBuffer = new List<Item>();
        for(int i = 0; i < itemSO.items.Length; i++) { // item 배열에 담겨있는 10개의 카드
            Item item = itemSO.items[i]; // 10개의 카드를 가져온다.
            for(int j = 0; j < item.percent; j++) { // 각각의 카드만큼의 퍼센트 만큼 반복시킨다
                itemBuffer.Add(item); // 총 100장의 카드가 들어가며 각가의 카드 갯수는 각 카드의 퍼센트만큼 들어간다.
            }
        }

        for(int i = 0; i < itemBuffer.Count; i++) { // 순서대로 들어가있는 카드를 랜덤하게 섞어준다.
            int rand = Random.Range(i, itemBuffer.Count);
            Item temp = itemBuffer[i];
            itemBuffer[i] = itemBuffer[rand];
            itemBuffer[rand] = temp;
        }
    }

    void Start() {
        SetupItemBuffer();
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Keypad1)) { // 만약 1번 키를 누르면
            Addcard(true); // Test를 위한 호출. 내 카드가 생성
        }
        if (Input.GetKeyDown(KeyCode.Keypad2)) { // 만약 2번 키를 누르면
            Addcard(false); // Test를 위한 호출. 상대 카드가 생성
        }
    }

    void Addcard(bool isMine) {
        var cardObject = Instantiate(cardPrefab, Vector3.zero, Quaternion.identity);
        var card = cardObject.GetComponent<Card>();
        card.Setup(PopItem(), isMine);
        (isMine ? myCards : otherCards).Add(card); // 새로추가. 만약 내 카드라면 myCards에 Add 아니라면 otherCards에 Add

        SetOriginOrder(isMine);
    }

    void SetOriginOrder(bool isMine) {
        int count = isMine ? myCards.Count : otherCards.Count;
        for(int i = 0; i < count; i++) {
            var targetCard = isMine ? myCards[i] : otherCards[i];
            targetCard?.GetComponent<Order>().SetOriginOrder(i); // targetCard가 존재 한다면 Order의 SetOriginOrder를 이용해 Order를 설정한다.
        }
    }
}
