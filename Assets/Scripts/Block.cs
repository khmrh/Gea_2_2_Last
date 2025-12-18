using UnityEngine;

public enum ItemType { Dirt, Grass, Water, Fshovel, Tshovel, Sshovel, Null, Wood, Iron, Diamond, Leaf}
public class Block : MonoBehaviour
{
    [Header("Block stat")]
    public ItemType type = ItemType.Dirt;
    public int maxHP = 3;
    [HideInInspector] public int hp;

    public int dropCount = 1;
    public bool mineable = true;

    void Awake()
    {
        hp = maxHP;
        if (GetComponent<Collider>() == null) gameObject.AddComponent<BoxCollider>();
        if (string.IsNullOrEmpty(gameObject.tag) || gameObject.tag == "UnTagged") 
            gameObject.tag = "Block";
    }

    public void Hit(ItemType _type , Inventory inven)
    {
        if (!mineable) return;

        switch (_type)
        {
            case ItemType.Fshovel:

                if (inven != null && dropCount > 0)
                    inven.add(type, dropCount + 2);

                Destroy(gameObject);
                break;
            case ItemType.Tshovel:
                if(inven != null && dropCount > 0)
                        inven.add(type, dropCount + 4);

                Destroy(gameObject);
                break;
            case ItemType.Sshovel:
                if (inven != null && dropCount > 0)
                    inven.add(type, dropCount + 6);

                Destroy(gameObject);
                break;
            case ItemType.Null:

                hp -= 1;

                if (hp <= 0)
                {
                    if (inven != null && dropCount > 0)
                        inven.add(type, dropCount);

                    Destroy(gameObject);
                }
                break;
        }

        
    }
}
