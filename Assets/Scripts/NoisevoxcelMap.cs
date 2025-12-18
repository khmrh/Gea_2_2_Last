using UnityEngine;

public class NoisevoxcelMap : MonoBehaviour
{
    public GameObject blockPrefabDirt;
    public GameObject blockPrefabGrass;
    public GameObject blockPrefabwater;
    public GameObject blockPrefabWood;
    public GameObject blockPrefabLeaf;
    public GameObject blockPrefabIron;
    public GameObject blockPrefabDiamond;
    public int width = 20;
    public int depth = 20;
    public int maxHeight = 16;
    public int waterLevel = 4;
    public int treeLevel = 5;
    public int IronLevel = 8;
    public int diamondLevel = 4;



    [SerializeField] float noiseScale = 20f;


    void Start()
    {
        float offsetX = Random.Range(-9999f, 9999f);
        float offsetZ = Random.Range(-9999f, 9999f);
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                float nx = (x + offsetX) / noiseScale;
                float nz = (z + offsetZ) / noiseScale;
                float noise = Mathf.PerlinNoise(nx, nz);

                // 1. 해당 좌표의 최대 높이 결정
                int h = Mathf.FloorToInt(noise * maxHeight);
                if (h <= 0) h = 1;

                // 2. 블록 쌓기 (y축 루프)
                for (int y = 0; y <= h; y++)
                {
                    if (y == h)
                    {
                        // 가장 윗 표면은 무조건 잔디
                        PlaseGrass(x, y, z);
                    }
                    else
                    {
                        // 땅 속: 광석이나 흙 배치
                        if (y <= diamondLevel && Random.Range(0, 50) == 0)
                            PlaseDiamond(x, y, z);
                        else if (y <= IronLevel && Random.Range(0, 20) == 0)
                            PlaseIron(x, y, z);
                        else
                            PlaseDirt(x, y, z);
                    }
                }
                // 3. 나무 심기 (y 루프 밖에서 딱 한 번 실행)
                // 지형이 높든 낮든 h는 항상 그 좌표의 '잔디' 위치
                if (x > 2 && x < width - 3 && z > 2 && z < depth - 3)
                {
                    // 물속이 아닐 때만 나무 생성 (h가 waterLevel보다 높을 때)
                    if (h > waterLevel)
                    {
                        // treeLevel 확률로 생성
                        if (Random.Range(0, treeLevel) == 0)
                        {
                            // 잔디(h) 바로 위인 h + 1 위치에 나무 생성
                            CreateTree(x, h + 1, z);
                        }
                    }
                }
                // 4. 물 채우기
                for (int y = h + 1; y <= waterLevel; y++)
                {
                    PlaseWater(x, y, z);
                }
            }
        }
    }


    void CreateTree(int x, int y, int z)
    {
        // 50% 확률로 입구 컷
        if (Random.value > 0.5f) return;

        // 검사 범위 자체를 무작위로 설정
        int dynamicRange = Random.Range(2, 6);

        for (int cx = -dynamicRange; cx <= dynamicRange; cx++)
        {
            for (int cz = -dynamicRange; cz <= dynamicRange; cz++)
            {
                // 모든 방향을 다 검사하지 않고 무작위로 건너뜀 (대칭성 파괴)
                if (Random.value > 0.8f) continue;
                // 바보 멍청이
                for (int cy = -3; cy <= 3; cy++)
                {
                    if (transform.Find($"Wood_{x + cx}_{y + cy}_{z + cz}") != null)
                    {
                        return;
                    }
                }
            }
        }

        int height = Random.Range(5, 8);

        int leafStart = height - 2;
        int radius = 2;

        for (int ly = leafStart; ly <= height + 1; ly++)
        {
            for (int lx = -radius; lx <= radius; lx++)
            {
                for (int lz = -radius; lz <= radius; lz++)
                {
                    if (lx * lx + lz * lz <= radius * radius)
                    {
                        if (Random.value > 0.2f)
                        {
                            PlaseLeaf(x + lx, y + ly, z + lz);
                        }
                    }
                }
            }
        }
        for (int i = 0; i < height; i++)
        {
            PlaseWood(x, y + i, z);
        }
    }

    void PlaseLeaf(int x, int y, int z)
    {
        if (x >= 0 && x < width && z >= 0 && z < depth)
        {
            var go = Instantiate(blockPrefabLeaf, new Vector3(x, y, z), Quaternion.identity, transform);
            go.name = $"Leaf_{x}_{y}_{z}";
        }
    }


    private void PlaseWater(int x, int y, int z)
    {
        var go = Instantiate(blockPrefabwater, new Vector3(x, y, z), Quaternion.identity, transform);
        go.name = $"Water_{x}_{y}_{z}";

        var b = go.GetComponent<Block>() ?? go.AddComponent<Block>();
        b.type = ItemType.Leaf;
        b.maxHP = 1;
        b.dropCount = 1;
        b.mineable = true;
    }

    private void PlaseDirt(int x, int y, int z)
    {
        var go = Instantiate(blockPrefabDirt, new Vector3(x, y, z), Quaternion.identity, transform);
        go.name = $"Dirt_{x}_{y}_{z}";

        var b = go.GetComponent<Block>() ?? go.AddComponent<Block>();
        b.type = ItemType.Dirt;
        b.maxHP = 3;
        b.dropCount = 1;
        b.mineable = true;
    }

    private void PlaseGrass(int x, int y, int z)
    {
        var go = Instantiate(blockPrefabGrass, new Vector3(x, y, z), Quaternion.identity, transform);
        go.name = $"Grass_{x}_{y}_{z}";

        var b = go.GetComponent<Block>() ?? go.AddComponent<Block>();
        b.type = ItemType.Grass;
        b.maxHP = 3;
        b.dropCount = 1;
        b.mineable = true;
    }

    private void PlaseWood(int x, int y, int z)
    {
        var go = Instantiate(blockPrefabWood, new Vector3(x, y, z), Quaternion.identity, transform);
        go.name = $"Wood_{x}_{y}_{z}";

        var b = go.GetComponent<Block>() ?? go.AddComponent<Block>();
        b.type = ItemType.Wood;
        b.maxHP = 5;
        b.dropCount = 1;
        b.mineable = true;
    }

    private void PlaseIron(int x, int y, int z)
    {
        var go = Instantiate(blockPrefabIron, new Vector3(x, y, z), Quaternion.identity, transform);
        go.name = $"Iron_{x}_{y}_{z}";

        var b = go.GetComponent<Block>() ?? go.AddComponent<Block>();
        b.type = ItemType.Iron;
        b.maxHP = 10;
        b.dropCount = 1;
        b.mineable = true;
    }

    private void PlaseDiamond(int x, int y, int z)
    {
        var go = Instantiate(blockPrefabDiamond, new Vector3(x, y, z), Quaternion.identity, transform);
        go.name = $"Diamond_{x}_{y}_{z}";

        var b = go.GetComponent<Block>() ?? go.AddComponent<Block>();
        b.type = ItemType.Diamond;
        b.maxHP = 15;
        b.dropCount = 1;
        b.mineable = true;
    }

    public void PleaceTile(Vector3Int pos, ItemType type)
    {
        switch (type)
        {
            case ItemType.Dirt:
                PlaseDirt(pos.x, pos.y, pos.z);
                break;
            case ItemType.Grass:
                PlaseGrass(pos.x, pos.y, pos.z);
                break;
            case ItemType.Water:
                PlaseWater(pos.x, pos.y, pos.z);
                break;
            case ItemType.Wood:
                PlaseWood(pos.x, pos.y, pos.z);
                break;
            case ItemType.Iron:
                PlaseIron(pos.x, pos.y, pos.z);
                break;
            case ItemType.Diamond:
                PlaseDiamond(pos.x, pos.y, pos.z);
                break;
            case ItemType.Leaf:
                PlaseLeaf(pos.x, pos.y, pos.z);
                break;
        }
    }
}
