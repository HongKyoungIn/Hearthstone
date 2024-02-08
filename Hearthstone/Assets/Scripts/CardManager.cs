using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour {
    public static CardManager Inst { get; private set; } // �Ŵ����� �ϳ��� �����ϱ� ������ �̱������� ����.
    void Awake() => Inst = this;

    [SerializeField] ItemSO itemSO;
    [SerializeField] GameObject cardPrefab; 
    [SerializeField] List<Card> myCards; // ���� �߰�
    [SerializeField] List<Card> otherCards; // ���� �߰�

    List<Item> itemBuffer;

    public Item PopItem() {
        if (itemBuffer.Count == 0) { // ���� ī�带 �� �̾� ���۰� ������ �ִ� ī���� ������ 0���� �Ǹ�
            SetupItemBuffer(); // �ٽ� ���� ���ۿ� 100���� ī�带 ����
        }

        Item item = itemBuffer[0]; // ���� �� �տ� �ִ� ī�带 �̴´�.
        itemBuffer.RemoveAt(0); // ���� ī�带 ���ۿ��� �����.
        return item; // ī�带 �̾Ƴ���.
    }

    void SetupItemBuffer() {
        itemBuffer = new List<Item>();
        for(int i = 0; i < itemSO.items.Length; i++) { // item �迭�� ����ִ� 10���� ī��
            Item item = itemSO.items[i]; // 10���� ī�带 �����´�.
            for(int j = 0; j < item.percent; j++) { // ������ ī�常ŭ�� �ۼ�Ʈ ��ŭ �ݺ���Ų��
                itemBuffer.Add(item); // �� 100���� ī�尡 ���� ������ ī�� ������ �� ī���� �ۼ�Ʈ��ŭ ����.
            }
        }

        for(int i = 0; i < itemBuffer.Count; i++) { // ������� ���ִ� ī�带 �����ϰ� �����ش�.
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
        if(Input.GetKeyDown(KeyCode.Keypad1)) { // ���� 1�� Ű�� ������
            Addcard(true); // Test�� ���� ȣ��. �� ī�尡 ����
        }
        if (Input.GetKeyDown(KeyCode.Keypad2)) { // ���� 2�� Ű�� ������
            Addcard(false); // Test�� ���� ȣ��. ��� ī�尡 ����
        }
    }

    void Addcard(bool isMine) {
        var cardObject = Instantiate(cardPrefab, Vector3.zero, Quaternion.identity);
        var card = cardObject.GetComponent<Card>();
        card.Setup(PopItem(), isMine);
        (isMine ? myCards : otherCards).Add(card); // �����߰�. ���� �� ī���� myCards�� Add �ƴ϶�� otherCards�� Add

        SetOriginOrder(isMine);
    }

    void SetOriginOrder(bool isMine) {
        int count = isMine ? myCards.Count : otherCards.Count;
        for(int i = 0; i < count; i++) {
            var targetCard = isMine ? myCards[i] : otherCards[i];
            targetCard?.GetComponent<Order>().SetOriginOrder(i); // targetCard�� ���� �Ѵٸ� Order�� SetOriginOrder�� �̿��� Order�� �����Ѵ�.
        }
    }
}
