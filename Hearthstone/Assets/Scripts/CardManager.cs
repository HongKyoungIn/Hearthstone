using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour {
    public static CardManager Inst { get; private set; } // 매니저는 하나만 존재하기 때문에 싱글톤으로 선언.
    void Awake() => Inst = this;

    /*
    [SerializeField]
    - 인스펙터 창에 노출시켜 접근 가능하지만 다른 외부 스크립트에선 수정을 못하게 한다.
      즉, private 변수이지만 inspector 창에서 접근 가능한다.
    */
    [SerializeField] ItemSO itemSO;
    [SerializeField] GameObject cardPrefab; // GameObject : Unity의 기본 객체 타입입니다. 모든 게임 오브젝트는 GameObject 클래스의 인스턴스입니다. GameObject 타입의 cardPrefab 변수를 선언.
    [SerializeField] List<Card> myCards; // Card 타입의 리스트인 myCards를 선언.(내 카드 리스트)
    [SerializeField] List<Card> otherCards;// Card 타입의 리스트인 otherCards를 선언.(상대 카드 리스트)

    List<Item> itemBuffer; // Item 타입의 리스트인 itemBuffer를 선언. 카드 덱을 담고있는 버퍼를 의미한다.

    public Item PopItem() { // Item을 꺼내는 메서드. 즉, 다음 카드를 뽑는 메서드
        if (itemBuffer.Count == 0) { // 만약 카드를 다 뽑아 버퍼가 가지고 있는 카드의 갯수가 0개가 되면
            SetupItemBuffer(); // 다시 새로 버퍼에 100장의 카드를 셋팅
        }

        Item item = itemBuffer[0]; // 버퍼 맨 앞에 있는 카드를 뽑는다.
        itemBuffer.RemoveAt(0); // 뽑은 카드를 버퍼에서 지운다.
        return item; // 카드를 뽑아낸다.
    }

    void SetupItemBuffer() { // Buffer에 아이템을 채우는 메서드
        itemBuffer = new List<Item>(); // Item 타입의 객체를 담을 수 있는 새로운 리스트를 생성하고, 그것을 itemBuffer 변수에 할당하는 것을 의미합니다. 즉, itemBuffer라는 이름의 Item 객체 리스트를 생성하는 것을 의미합니다. 
        for (int i = 0; i < itemSO.items.Length; i++) { // item 배열에 담겨있는 10개의 카드
            Item item = itemSO.items[i]; // 10개의 카드를 가져온다.
            for(int j = 0; j < item.percent; j++) { // 각각의 카드만큼의 퍼센트 만큼 반복시킨다
                itemBuffer.Add(item); // 총 100장의 카드가 들어가며 각가의 카드 갯수는 각 카드의 퍼센트만큼 들어간다.
            }
        }

        // 버퍼에 순서대로 들어가있는 카드를 랜덤하게 섞어주는 반복문.
        for (int i = 0; i < itemBuffer.Count; i++) { // 0부터 버퍼의 크기만큼 반복
            int rand = Random.Range(i, itemBuffer.Count); // rand 변수에 0 ~ 99까지 랜덤한 수를 뽑는다.
            Item temp = itemBuffer[i]; // 임시 Item 타입의 변수 temp에 현재 버퍼의 i번째 정보를 저장한다.
            itemBuffer[i] = itemBuffer[rand]; // 현재 버퍼의 i번째 정보를 랜덤하게 뽑은 버퍼의 rand번째 정보로 바꾼다.
            itemBuffer[rand] = temp; // 버퍼의 rand번째 정보는 i번째 정보로 바꾼다.
            // 즉, 위 과정을 모두 수행하면 현재 i번째 정보와 랜덤하게 뽑은 rand번째 정보가 서로 바뀐다.
        }
    }

    void Start() {
        SetupItemBuffer(); // 버퍼에 100장의 카드를 확률에 근거하여 랜덤한 순서로 설정한다.
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
