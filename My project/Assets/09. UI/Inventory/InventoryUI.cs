using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryUI : WindowUI
{
    private List<Slot> _slotList;
    private Dictionary<int, Item> _itemDictionary = new Dictionary<int, Item>();

    private VisualTreeAsset _itemTemplate;

    public InventoryUI(VisualElement root, VisualTreeAsset itemTemplate) : base(root)
    {
        //Ŭ������ slot�� �༮���� ���� ã�Ƽ� ������ �ѹ��� 0������ �ű�� 
        // �װɷ� Slot�̶�� Ŭ������ ���� _slotList�� �־����.
        _itemTemplate = itemTemplate;
        _slotList = root.Query<VisualElement>(className: "slot").ToList().Select((elem, idx) => new Slot(elem, idx)).ToList();

        #region �׽�ũ �ڵ�

        var item = root.Q<VisualElement>(className: "item");
        VisualElement slot = item.parent.parent;
        item.parent.RemoveFromHierarchy(); //��ä�� ���ư�.

        slot.Add(item);

        item.AddManipulator(new Dragger((evt, target, beforeSlot) => 
        { 
            // �巡�� �ϴ� ���� ������� �� �߻�
        }));

        #endregion
    }
}
