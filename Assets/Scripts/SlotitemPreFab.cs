using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotitemPreFab : MonoBehaviour, IPointerClickHandler
{
    public Image itemimage;
    public TextMeshProUGUI itemText;
    public ItemType BlockType;
    public CraftingPanel craftingPanel;

    public void itemSetting(Sprite itemSprite, string txt, ItemType type)
    {
        itemimage.sprite = itemSprite;
        itemText.text = txt;
        BlockType = type;
    }

    void Awake()
    {
        if (!craftingPanel)
            craftingPanel = FindObjectOfType<CraftingPanel>(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Right) return;
        if (!craftingPanel) return;

        craftingPanel.AddPlanned(BlockType, 1);
    }
}
