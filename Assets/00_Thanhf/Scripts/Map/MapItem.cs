using UnityEngine;
using UnityEngine.Timeline;

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

        if (_mesh != null && transform.CompareTag(TagConst.TAG_BRIDGE))
        {
            _mesh.enabled = false; // Disable mesh renderer for bridge bricks
        }
    }

    public void DisableComponent()
    {
        if (transform.CompareTag(TagConst.TAG_START_AREA) || transform.CompareTag(TagConst.TAG_WIN_AREA))
        {
            _collider.enabled = true;
            return;
        }
        else if (transform.CompareTag(TagConst.TAG_BRIDGE))
        {
            // Disable the collider and mesh renderer for bricks
            if (_collider != null)
            {
                _collider.enabled = false;
                _mesh.enabled = true;
            }

            return;
        }

        if (_collider != null) _collider.enabled = false;
        if (_mesh != null) _mesh.enabled = false;
    }
}
