using UnityEngine;

public class GameWorld : MonoBehaviour
{
    public static GameWorld Singleton { get; private set; }

    public static GameObjectPool FilesPool => Singleton._filesPool;
    public static GameObjectPool MiniCursorsPool => Singleton._miniCursorsPool;

    [SerializeField] private File _file;
    [SerializeField] private MiniCursor _miniCursor;

    private GameObjectPool _filesPool;
    private GameObjectPool _miniCursorsPool;

    private void Awake()
    {
        if (Singleton == null) Singleton = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        _filesPool = new(_file.gameObject);
        _miniCursorsPool = new(_miniCursor.gameObject);
    }
}
