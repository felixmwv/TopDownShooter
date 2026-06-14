using UnityEngine;

public class EnemyFactory
{
    private readonly GameObject _basicPrefab;
    private readonly GameObject _fastPrefab;

    public EnemyFactory(GameObject basicPrefab, GameObject fastPrefab)
    {
        _basicPrefab = basicPrefab;
        _fastPrefab = fastPrefab;
    }

    public Enemy Create(EnemyType type)
    {
        EnemyData data = GetData(type);
        GameObject prefab = GetPrefab(type);
        GameObject obj = Object.Instantiate(prefab);
        obj.SetActive(false);
        
        var sr = obj.GetComponent<SpriteRenderer>();
        float radius = sr != null
            ? Mathf.Min(sr.bounds.extents.x, sr.bounds.extents.y)
            : 0.25f;

        return new Enemy(obj.transform, data, radius);
    }

    private EnemyData GetData(EnemyType type) => type switch
    {
        EnemyType.BASIC => new EnemyData { Speed = 2f },
        EnemyType.FAST  => new EnemyData { Speed = 5f },
        _               => new EnemyData { Speed = 2f }
    };

    private GameObject GetPrefab(EnemyType type) => type switch
    {
        EnemyType.BASIC => _basicPrefab,
        EnemyType.FAST  => _fastPrefab,
        _               => _basicPrefab
    };
}