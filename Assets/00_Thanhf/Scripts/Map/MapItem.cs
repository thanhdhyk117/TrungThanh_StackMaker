using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapItem : MonoBehaviour
{
    [SerializeField] private BoxCollider _collider;
    [SerializeField] private MeshRenderer _mesh;
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

        _collider.enabled = true;
        _mesh.enabled = true;
    }


    public void DisableComponent()
    {
        if (_collider != null) _collider.enabled = false;
        if (_mesh != null) _mesh.enabled = false;
    }

    void OnDisable()
    {
        DisableComponent();
    }
}
