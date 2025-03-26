using UnityEngine;

public class GameWorld : MonoBehaviour
{
    public static GameWorld Singleton { get; private set; }

    public static GameObjectPool FilesPool => Singleton._filesPool;
    public static GameObjectPool DiskPool => Singleton._diskPool;
    public static GameObjectPool MiniCursorsPool => Singleton._miniCursorsPool;
    public static GameObjectPool MoniesPool => Singleton._moniesPool;
    public static GameObjectPool DamagePool => Singleton._damagePool;

    [SerializeField] private File _file;
    [SerializeField] private Disk _disk;
    [SerializeField] private MiniCursor _miniCursor;
    [SerializeField] private Money _money;
    [SerializeField] private FlowNumbers _flowDamage;

    [SerializeField] private Transform _wallLeft, _wallRight;

    private GameObjectPool _filesPool;
    private GameObjectPool _diskPool;
    private GameObjectPool _miniCursorsPool;
    private GameObjectPool _moniesPool;
    private GameObjectPool _damagePool;

    private void Awake()
    {
        if (Singleton == null) Singleton = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        _wallLeft.position = Camera.main.ViewportToWorldPoint(new(0, 0.5f));
        _wallRight.position = Camera.main.ViewportToWorldPoint(new(1, 0.5f));

        _filesPool = new(_file.gameObject);
        _diskPool = new(_disk.gameObject);
        _miniCursorsPool = new(_miniCursor.gameObject);
        _moniesPool = new(_money.gameObject);
        _damagePool = new(_flowDamage.gameObject);
    }
}
