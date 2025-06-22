using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickPlayer : MonoBehaviour
{
    [SerializeField] private GameObject _brickPrefab;
    [SerializeField] private Transform _playerBody, _brickParent;

    [SerializeField] private Vector3 _startBrickPos;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private int _brickCount;
    [SerializeField] private List<GameObject> _listBrick = new List<GameObject>();
    public int Count
    {
        get
        {
            return _brickCount;
        }
        set
        {
            _brickCount = Mathf.Max(0, value);
            UpdateBrickPositions();
        }
    }

    void Start()
    {
        Init();
    }

    private void Init()
    {
        _brickCount = 0;
        _playerBody.localPosition = new Vector3(0, 1.3f, 0);
        _listBrick.Clear();
    }

    private void ClearBricks()
    {
        foreach (GameObject brick in _listBrick)
        {
            if (brick != null)
                Destroy(brick);
        }
        _listBrick.Clear();
    }

    [ContextMenu("Add")]
    private void UpdateBrickPositions()
    {
        // Kiem tra xem so luong gach va so luong trong list:
        //Neu so luong gach lon hon => them vao list, neu khong thi chuyen den buoc tiep
        while (_listBrick.Count < _brickCount)
        {
            GameObject newBrick = Instantiate(_brickPrefab, _brickParent);
            newBrick.SetActive(true);
            _listBrick.Add(newBrick);
        }

        // Cap nhat lai vi tri gach va vi tri cua nguoi choi
        for (int i = 0; i < _listBrick.Count; i++)
        {
            bool isActive = i < _brickCount;
            _listBrick[i].SetActive(isActive);
            if (isActive)
            {
                _listBrick[i].transform.localPosition = _startBrickPos + i * _offset;
            }
        }

        _playerBody.localPosition = new Vector3(0, 1.3f, 0) + (_brickCount > 0 ? (_brickCount - 1) * _offset : Vector3.zero);
    }

    private void OnDestroy()
    {
        ClearBricks();
    }
}
