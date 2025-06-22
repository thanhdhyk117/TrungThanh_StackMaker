using System.Collections.Generic;
using UnityEngine;

public class BrickPlayer : MonoBehaviour
{
    [SerializeField] private GameObject _brickPrefab;
    [SerializeField] private Transform _playerBody;
    [SerializeField] private Transform _brickParent;
    [SerializeField] private Vector3 _startBrickPos = Vector3.zero;
    [SerializeField] private Vector3 _offset = new Vector3(0, 0.3f, 0);

    private int _brickCount;
    private readonly List<GameObject> _listBrick = new List<GameObject>();

    public int Count
    {
        get => _brickCount;
        set
        {
            _brickCount = Mathf.Max(0, value);
            UpdateBrickPositions();
        }
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        if (_brickPrefab == null || _playerBody == null || _brickParent == null)
        {
            Debug.LogError("BrickPrefab, PlayerBody, or BrickParent is not assigned!");
            enabled = false;
            return;
        }
        _brickCount = 0;
        _playerBody.localPosition = new Vector3(0, 1.3f, 0);
        ClearBricks();
    }

    private void ClearBricks()
    {
        foreach (GameObject brick in _listBrick)
        {
            if (brick != null)
            {
                Destroy(brick);
            }
        }
        _listBrick.Clear();
    }

    [ContextMenu("Add")]
    private void UpdateBrickPositions()
    {
        // Ensure enough bricks in the pool
        while (_listBrick.Count < _brickCount)
        {
            GameObject newBrick = Instantiate(_brickPrefab, _brickParent);
            newBrick.SetActive(false); // Inactive until positioned
            _listBrick.Add(newBrick);
        }

        // Update brick positions and visibility
        for (int i = 0; i < _listBrick.Count; i++)
        {
            bool isActive = i < _brickCount;
            _listBrick[i].SetActive(isActive);
            if (isActive)
            {
                _listBrick[i].transform.localPosition = _startBrickPos + i * _offset;
            }
        }

        // Update player body position
        _playerBody.localPosition = new Vector3(0, 1.3f, 0) + (_brickCount > 0 ? (_brickCount - 1) * _offset : Vector3.zero);
    }

    private void OnDestroy()
    {
        ClearBricks();
    }
}