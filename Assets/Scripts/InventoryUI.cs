using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    //큐브별 스프라이트
    public Sprite dirtsprite;
    public Sprite watersprite;
    public Sprite grasssprite;
    public Sprite Woodsprite;
    public Sprite Leafsprite;
    public Sprite Ironsprite;
    public Sprite Diamondsprite;
    public Sprite fshovelsprite;
    public Sprite tshovelsprite;
    public Sprite sshovelsprite;

    public List<Transform> slot = new List<Transform>();
    public GameObject SlotItem;
    List<GameObject> items = new List<GameObject>();

    public int selectedIndex = -1;
    public void UpdateInventory(Inventory myInven)
    {
        foreach (var slotItem in items)
        {
            Destroy(slotItem);
        }
        items.Clear();

        int idx = 0;
        foreach (var item in myInven.items)
        {
            var go = Instantiate(SlotItem, slot[idx].transform);
            go.transform.localPosition = Vector3.zero;
            SlotitemPreFab sItem = go.GetComponent<SlotitemPreFab>();
            items.Add(go);

            switch (item.Key)
            {
                case ItemType.Dirt:
                    sItem.itemSetting(dirtsprite, "x" + item.Value.ToString(), item.Key);
                    break;
                case ItemType.Grass:
                    sItem.itemSetting(grasssprite, "x" + item.Value.ToString(), item.Key);
                    break;
                case ItemType.Water:
                    sItem.itemSetting(watersprite, "x" + item.Value.ToString(), item.Key);
                    break;
                case ItemType.Fshovel:
                    sItem.itemSetting(fshovelsprite, "x" + item.Value.ToString(), item.Key);
                    break;
                case ItemType.Wood:
                    sItem.itemSetting(Woodsprite, "x" + item.Value.ToString(), item.Key);
                    break;
                case ItemType.Iron:
                    sItem.itemSetting(Ironsprite, "x" + item.Value.ToString(), item.Key);
                    break;
                case ItemType.Diamond:
                    sItem.itemSetting(Diamondsprite, "x" + item.Value.ToString(), item.Key);
                    break;
                case ItemType.Tshovel:
                    sItem.itemSetting(tshovelsprite, "x" + item.Value.ToString(), item.Key);
                    break;
                case ItemType.Sshovel:
                    sItem.itemSetting(sshovelsprite, "x" + item.Value.ToString(), item.Key);
                    break;
                case ItemType.Leaf:
                    sItem.itemSetting(Leafsprite, "x" + item.Value.ToString(), item.Key);
                    break;
            }
            idx++;
        }
    }

    private void Update()
    {
        for (int i = 0; i < Mathf.Min(9, slot.Count); i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SetSeclectedIndex(i);
            }
        }
    }

    public void SetSeclectedIndex(int idx)
    {
        RssetSelection();
        if (selectedIndex == idx)
        {
            selectedIndex = -1;
        }
        else
        {
            if (idx >= items.Count)
            {
                selectedIndex = -1;
            }
            else
            {
                Setselection(idx);
                selectedIndex = idx;
            }
        }
    }

    public void RssetSelection()
    {
        foreach (var slot in slot)
        {
            slot.GetComponent<Image>().color = Color.white;
        }
    }

    void Setselection(int _idx)
    {
        slot[_idx].GetComponent<Image>().color = Color.yellow;
    }

    public ItemType GetInventorySlot()
    {
        return items[selectedIndex].GetComponent<SlotitemPreFab>().BlockType;
    }
}
