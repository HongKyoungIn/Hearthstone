using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order : MonoBehaviour {
    [SerializeField]
    Renderer[] backRenderers; // ���ʿ� �ִ� Renderer

    [SerializeField]
    Renderer[] middelRenderers; // �߾ӿ� �ִ� Renderer

    [SerializeField]
    string sortingLayerName; // SortingLayer �̸��� ����

    int originOrder;

    public void SetOriginOrder(int originOrder) {
        this.originOrder = originOrder;
        SetOrder(originOrder);
    }

    public void SetMostFrontOrder(bool isMostFront) {
        SetOrder(isMostFront ? 100 : originOrder);
    }

    public void SetOrder(int order) {
        int mulOrder = order * 10;

        foreach (var renderer in backRenderers) {
            renderer.sortingLayerName = sortingLayerName;
            renderer.sortingOrder = mulOrder;
        }

        foreach (var renderer in middelRenderers) {
            renderer.sortingLayerName = sortingLayerName;
            renderer.sortingOrder = mulOrder + 1;
        }
    }
}
