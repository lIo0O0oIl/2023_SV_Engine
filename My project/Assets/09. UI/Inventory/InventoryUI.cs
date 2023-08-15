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
        //Ŭ������ slot�� �༮���� ���� ã�Ƽ� ������ �ѹ��� 0������ �ű�� 
        // �װɷ� Slot�̶�� Ŭ������ ���� _slotList�� �־����.
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
                UIController.Instance.Message.AddMessage("�κ��丮�� ��ĭ�� �����", 3f);
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
                //���� ��ġ�� �ǵ����� �ǰ���
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
                item.slotNumber = slot.slotNumber; //�̰� �߰��Ǿ���.
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
        return _slotList.Find(x => x.Child == null); //���� ���ʿ� �ִ� ����ִ� ������ �����´�.
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
        //��� ������ ã�Ƽ� ���߿��� worldBound �� position�� ���ϴ� �༮�� ã�ƿ��� 
        return _slotList.Find(s => s.Elem.worldBound.Contains(position)); //�ش� RECT�ȿ� �������� �ִ��� �˻���
    }

    public override void Close()
    {
        //���� ���� fade�� ���µ� close�Ǵ°Ÿ� �����°Ű� �׷��� �ʴٸ� �����ſ� �Ǵݴ´�.
        if (!_root.ClassListContains("fade") && _isLoadedFromServer)
        {
            SaveToDB();
            Debug.Log("�����Ѵ�.");
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
