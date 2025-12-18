using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingPanel : MonoBehaviour
{
    public Inventory inventory;
    public List<CraftingRecipe> recipeList;
    public GameObject root;
    public TMP_Text plannedText;
    public Button craftButton;
    public Button clearButton;
    public TMP_Text hintText;

    readonly Dictionary<ItemType, int> planned = new();

    bool isOpen;

    void Start()
    {
       SetOpen(false);
       craftButton.onClick.AddListener(DoCraft);
       clearButton.onClick.AddListener(ClearPlanned);
       RefreshPlannedUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            SetOpen(!isOpen);
    }

    public void SetOpen(bool open)
    {
        isOpen = open;
        if (root)
            root.SetActive(open);

        if (!open)
            ClearPlanned();
    }

    public void AddPlanned(ItemType type, int count = 1)
    {
        if (!planned.ContainsKey(type))
            planned[type] = 0;
        planned[type] += count;

        RefreshPlannedUI();
        SetHint($"{type} x{count} Ãß°¡ ¿Ï·á");
    }

    public void ClearPlanned()
    {
        planned.Clear();
        RefreshPlannedUI();
        SetHint("ÃÊ±âÈ­ ¿Ï·á");
    }

    void RefreshPlannedUI()
    {
        if (!plannedText)
            return;

        if (planned.Count == 0)
        {
            plannedText.text = "¿ìÅ¬¸¯À¸·Î À÷·á¸¦ Ãß°¡ÇÏ¼¼¿ä.";
            return;
        }

        var sb = new StringBuilder();

        foreach (var item in planned)
            sb.AppendLine($"{item.Key} x{item.Value}");
        plannedText.text = sb.ToString();
    }

    void SetHint(string msg) { 

        if (hintText)
            hintText.text = msg;
    }

    void DoCraft()
    {
        if (planned.Count == 0) { 
            SetHint("À÷·á°¡ ºÎÁ·ÇÕ´Ï´Ù.");
            return;
        }

        //ÀÎº¥ ¼ö·® Ã¼Å©
        foreach (var PlannedItem in planned)
        {
            if (inventory.GetCount(PlannedItem.Key) < PlannedItem.Value)
            {
                SetHint($"{PlannedItem.Key} °¡ ºÎÁ·ÇÕ´Ï´Ù.");
                return;
            }
        }

        var matchedProduct = FindMatch(planned);
        if (matchedProduct == null)
        {
            SetHint("¾Ë¸ÂÀº ·¹½ÃÇÇ°¡ ¾ø½À´Ï´Ù.");
            return;
        }

        //Àç·á ¼Ò¸ð
        foreach (var PlannedItem in planned)
            inventory.Consume(PlannedItem.Key, PlannedItem.Value);

        //°á°ú¹° Áö±Þ
        foreach (var p in matchedProduct.outputs)
            inventory.add(p.type, p.count);

        ClearPlanned();

        SetHint($"Á¶ÇÕ ¿Ï·á : {matchedProduct.displayName}");
    }

    CraftingRecipe FindMatch(Dictionary<ItemType, int> planned)
    {
        foreach (var recipe in recipeList)
        {
            //ÇÊ¿äÇÑ Àç·áµéÀ» ÃæºÐÈ÷ °®­Ÿ´ÂÁö
            bool ok = true;
            foreach (var ing in recipe.inputs)
            {
                if (!planned.TryGetValue(ing.type, out int have) || have != ing.amount)
                {
                    ok = false;
                    break;
                }
            }

            if (ok)
                return recipe;
        }
        return null;
    }



}
