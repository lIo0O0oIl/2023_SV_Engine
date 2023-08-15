using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;


public class InventoryUI : WindowUI
{
    private List<Slot> _slotList;
    private Dictionary<int, Item> _itemDictionary = new Dictionary<int, Item>();

    private VisualTreeAsset _itemTemplate;
    private bool _isLoadedFromServer = false;

    public InventoryUI(VisualElement root, VisualTreeAsset itemTemplate) : base(root)
    {
        //클래스가 slot인 녀석들을 전부 찾아서 각각을 넘버를 0번부터 매기고 
        // 그걸로 Slot이라는 클래스를 만드어서 _slotList에 넣어줘라.
        _itemTemplate = itemTemplate;
        _slotList = root.Query<VisualElement>(className: "slot").ToList().Select((elem, idx) => new Slot(elem, idx)).ToList();
    }


    public void AddItem(ItemSO dataSO, int count, int slotNumber = -1)
    {
        if (_itemDictionary.TryGetValue(dataSO.itemCode, out Item findItem))
        {
            findItem.Count += count;
            return;
        }

        VisualElement itemElem = _itemTemplate.Instantiate().Q<VisualElement>("Item");
        Slot emptySlot;
        if (slotNumber < 0)
        {
            emptySlot = FindEmptySlot();
            if (emptySlot == null)
            {
                UIController.Instance.Message.AddMessage("인벤토리에 빈칸이 없어요", 3f);
                return;
            }
        }
        else
        {
            emptySlot = FindSlotByNumber(slotNumber);
        }

        emptySlot.Elem.Add(itemElem);

        var item = new Item(itemElem, dataSO, emptySlot.slotNumber, count);
        _itemDictionary.Add(dataSO.itemCode, item);

        itemElem.AddManipulator(new Dragger((evt, target, beforeSlot) =>
        {
            var slot = FindSlot(evt.mousePosition);

            target.RemoveFromHierarchy();
            if (slot == null)
            {
                //원래 위치로 되돌리면 되겠지
                beforeSlot.Add(target);
            }
            else if (slot.Child != null)
            {
                VisualElement existItem = slot.Child;
                existItem.RemoveFromHierarchy();
                slot.Elem.Add(target);

                foreach (var kvP in _itemDictionary)
                {
                    if (kvP.Value.slotNumber == slot.slotNumber)
                    {
                        kvP.Value.slotNumber = FindSlotByElement(beforeSlot).slotNumber;
                        break;
                    }
                }

                item.slotNumber = slot.slotNumber;
                beforeSlot.Add(existItem);
            }
            else
            {
                item.slotNumber = slot.slotNumber; //이거 추가되었어.
                slot.Elem.Add(target);
            }
        }));
    }

    public void SaveToDB()
    {
        List<ItemVO> voList = _itemDictionary.Values.Select(item =>
            new ItemVO { itemCode = item.dataSO.itemCode, count = item.Count, slotNumber = item.slotNumber }).ToList();

        InventoryVO invenVO = new InventoryVO { list = voList, count = voList.Count };

        if (NetworkManager.Instance == null) return;
        NetworkManager.Instance.PostRequest("inven", invenVO, (type, msg) =>
        {
            if (type == MessageType.ERROR)
            {
                UIController.Instance.Message.AddMessage(msg, 3f);
            }
        });
    }

    private Slot FindEmptySlot()
    {
        return _slotList.Find(x => x.Child == null); //가장 앞쪽에 있는 비어있는 슬롯을 가져온다.
    }

    private Slot FindSlotByNumber(int slotNumber)
    {
        return _slotList[slotNumber];
    }

    private Slot FindSlotByElement(VisualElement elem)
    {
        return _slotList.Find(s => s.Elem == elem);
    }

    private Slot FindSlot(Vector2 position)
    {
        //모든 슬롯을 찾아서 그중에서 worldBound 에 position이 속하는 녀석을 찾아오면 
        return _slotList.Find(s => s.Elem.worldBound.Contains(position)); //해당 RECT안에 포지션이 있는지 검사해
    }

    public override void Close()
    {
        //지금 내가 fade가 없는데 close되는거면 닫히는거고 그렇지 않다면 닫힌거에 또닫는다.
        if (!_root.ClassListContains("fade") && _isLoadedFromServer)
        {
            SaveToDB();
            Debug.Log("저장한다.");
        }
        base.Close();
    }

    public override void Open()
    {
        if (NetworkManager.Instance == null) return;

        _itemDictionary.Clear();
        _slotList.ForEach(s => s.Elem.Clear());

        NetworkManager.Instance.GetRequest("inven", "", (type, json) =>
        {
            if (type == MessageType.ERROR)
            {
                UIController.Instance.Message.AddMessage(json, 3f);
                return;
            }

            if (type == MessageType.SUCCESS)
            {
                InventoryVO vo = JsonUtility.FromJson<InventoryVO>(json);
                vo.list.ForEach(item =>
                {
                    ItemSO data = UIController.Instance.itemList.Find(x => x.itemCode == item.itemCode);
                    AddItem(data, item.count, item.slotNumber);
                });
            }
            _isLoadedFromServer = true;
        });
        base.Open();
    }
}
