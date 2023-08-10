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
        //클래스가 slot인 녀석들을 전부 찾아서 각각을 넘버를 0번부터 매기고 
        // 그걸로 Slot이라는 클래스를 만드어서 _slotList에 넣어줘라.
        _itemTemplate = itemTemplate;
        _slotList = root.Query<VisualElement>(className: "slot").ToList().Select((elem, idx) => new Slot(elem, idx)).ToList();

        #region 테스크 코드

        var item = root.Q<VisualElement>(className: "item");
        VisualElement slot = item.parent.parent;
        item.parent.RemoveFromHierarchy(); //통채로 날아가.

        slot.Add(item);

        item.AddManipulator(new Dragger((evt, target, beforeSlot) => 
        { 
            // 드래그 하던 것을 드랍했을 떄 발생
        }));

        #endregion
    }
}
