using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMap : MonoBehaviour
{
    [SerializeField] GameObject _brickPrefab;
    [SerializeField] Transform startInstance, _brickParent;

    [SerializeField] int _horizontalValue, _verticalValue;

    [ContextMenu("Make map")]
    private void MakeMap()
    {
        CreateMapByMatrix(_horizontalValue, _verticalValue);
    }

    private void CreateMapByMatrix(int _x, int _y)
    {
        if (_x <= 0 || _y <= 0) return;

        float prefabScaleX = _brickPrefab.transform.localScale.x;
        float prefabScaleY = _brickPrefab.transform.localScale.y;

        for (int i = 0; i < _x; i++)
        {
            for (int j = 0; j < _y; j++)
            {
                Vector3 _newObjPos = startInstance.position + new Vector3(prefabScaleX * i * transform.localScale.x, 0, prefabScaleY * j * transform.localScale.z);

                GameObject _newObj = Instantiate(_brickPrefab, _newObjPos, Quaternion.Euler(new Vector3(-90, 0, -180)), _brickParent);
                _newObj.SetActive(true);
                if (_newObj.GetComponent<MapItem>() == null) _newObj.AddComponent<MapItem>();
            }
        }
    }
}
