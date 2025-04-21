using UnityEngine;

// 적 팩토리 클래스
public class EnemyFactory : MonoBehaviour
{
    // 싱글톤 패턴 적용
    private static EnemyFactory _instance;

    public static EnemyFactory Instance
    {
        get
        {
            if(_instance == null)
            {
                GameObject go = new GameObject("EnemyFactory");
                _instance = go.AddComponent<EnemyFactory>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    // 프리팹 참조
    public GameObject gruntPrefab;
    public GameObject runnerPrefab;
    public GameObject tankPrefab;

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // 적 생성 메서드
    public IEnemy CreateEnemy(EnemyType type, Vector3 position)
    {
        GameObject enemyObject = null;

        // 적 타입에 따라 다른 프리팹 사용
        switch(type)
        {
            case EnemyType.Grunt:
                enemyObject = Instantiate(gruntPrefab);
                break;
            case EnemyType.Runner:
                enemyObject = Instantiate(runnerPrefab);
                break;
            case EnemyType.Tank:
                enemyObject = Instantiate(tankPrefab);
                break;
        }

        IEnemy enemy = enemyObject.GetComponent<IEnemy>();

        enemy.Initialize(position);
        return enemy;
    }   
}
