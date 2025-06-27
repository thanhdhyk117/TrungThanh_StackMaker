using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField] private GameObject _bridgeObj, _brickObj;
    [SerializeField] private int _brickCount, _bridgeCount;
    [SerializeField] private int _brickPassedCount = 0;
    [SerializeField] private int _bridgePassedCount = 0;

    [SerializeField] private PlayerController _playerController;
    [SerializeField] private LevelComplete _levelComplete;
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {

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

        for (int i = 0; i < _brickObj.transform.childCount; i++)
        {
            Transform child = _brickObj.transform.GetChild(i).GetChild(0);

            if (child.CompareTag(TagConst.TAG_BRICK))
            {
                _brickCount++;
            }
            else
            {
                continue;
            }
        }

        for (int i = 0; i < _bridgeObj.transform.childCount; i++)
        {
            Transform child = _bridgeObj.transform.GetChild(i).GetChild(0);

            if (child.CompareTag(TagConst.TAG_BRIDGE))
            {
                _bridgeCount++;
            }
            else
            {
                continue;
            }


        }
    }
}
