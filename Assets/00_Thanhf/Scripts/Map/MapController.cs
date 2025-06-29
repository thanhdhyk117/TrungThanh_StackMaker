using UnityEngine;

public class MapController : MonoBehaviour
{
    public static MapController Instance { get; private set; }

    [SerializeField] private GameObject _bridgeObj, _brickObj;
    private int _brickCount;
    private int _bridgeCount;
    public int BrickCount => _brickCount;
    public int BridgeCount => _bridgeCount;

    // Update is called once per frame
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Init();
    }

    [ContextMenu("Init Map")]
    private void Init()
    {

        if (_bridgeObj == null || _brickObj == null)
        {
            Debug.LogError("Bridge or Brick object is not assigned in MapController.");
            return;
        }

        if (_bridgeObj.transform.childCount <= 0 || _brickObj.transform.childCount <= 0)
        {
            Debug.LogError("Bridge or Brick count must be greater than zero.");
            return;
        }

        _brickCount = 0;
        _bridgeCount = 0;

        _brickCount = _brickObj.transform.childCount;
        _bridgeCount = _bridgeObj.transform.childCount;
    }
}
