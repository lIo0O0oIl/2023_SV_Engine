using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Item
{
    public ItemSO dataSO;
    public int slotNumber;

    private VisualElement _root;
    private VisualElement _sprite;
    private int _count;
    private Label _countLabel;

    public int Count
    {
        get => _count;
        set
        {
            _count = value;
            _countLabel.text = _count.ToString();
        }
    }

    public Item(VisualElement root, ItemSO data, int slotNumber, int count)
    {
        _root = root;
        this.slotNumber = slotNumber;
        dataSO = data;

        _sprite = root.Q<VisualElement>("Sprite");
        _countLabel = root.Q<Label>("CountLabel");

        _sprite.style.backgroundImage = new StyleBackground(dataSO.sprite);
        Count = count;
    }
}
