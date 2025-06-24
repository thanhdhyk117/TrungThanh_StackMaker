using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapItem : MonoBehaviour
{
    public enum EMapItemType
    {
        None = 0,
        Wall = 1,
        Brick = 2,
        Bridge = 3,
        Water = 4,
        Stop = 5,
        Bounce = 6,
        WinArea = 7,
        StartArea = 8,
    }

    [SerializeField] private BoxCollider _collider;
    [SerializeField] private MeshRenderer _mesh;
    public EMapItemType MapItemType = EMapItemType.None;

    void Start()
    {
        Init();
    }

    void Reset()
    {
        Init();
    }

    private void Init()
    {
        if (_collider == null) _collider = GetComponent<BoxCollider>();
        if (_mesh == null) _mesh = GetComponent<MeshRenderer>();
    }


    public void DisableComponent()
    {
        if (_collider != null) _collider.enabled = false;
        if (_mesh != null) _mesh.enabled = false;
    }

    public void SetMapType(int typeMap)
    {
        MapItemType = (EMapItemType)typeMap;
        switch (MapItemType)
        {
            case EMapItemType.None:
            case EMapItemType.StartArea:
                _collider.enabled = false;
                _mesh.enabled = false;
                break;
            case EMapItemType.Wall:
            case EMapItemType.Brick:
            case EMapItemType.Bridge:
            case EMapItemType.Water:
            case EMapItemType.Stop:
            case EMapItemType.Bounce:
            case EMapItemType.WinArea:
                _collider.enabled = true;
                _mesh.enabled = true;
                break;
        }
    }

    void OnDisable()
    {
        DisableComponent();
    }
}
