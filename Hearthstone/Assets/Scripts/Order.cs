using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order : MonoBehaviour {
    [SerializeField]
    Renderer[] backRenderers; // 뒤쪽에 있는 Renderer

    [SerializeField]
    Renderer[] middelRenderers; // 중앙에 있는 Renderer

    [SerializeField]
    string sortingLayerName; // SortingLayer 이름을 지정

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
