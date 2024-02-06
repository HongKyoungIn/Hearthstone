using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour {
    public static CardManager Inst { get; private set; } // �Ŵ����� �ϳ��� �����ϱ� ������ �̱������� ����.
    void Awake() => Inst = this;

    [SerializeField] ItemSO itemSO;
    [SerializeField] GameObject cardPrefab; // ���� �߰�

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
            // print(PopItem().name);
            Addcard(true); // Test�� ���� ȣ��
        }
    }

    void Addcard(bool isMine) {
        var cardObject = Instantiate(cardPrefab, Vector3.zero, Quaternion.identity);
        var card = cardObject.GetComponent<Card>();
        card.Setup(PopItem(), isMine);
    }
}
