using UnityEngine;

public class MapItem : MonoBehaviour
{
    [SerializeField] private BoxCollider _collider;
    [SerializeField] private MeshRenderer _mesh;
    public EDirectionPlayer directionPush = EDirectionPlayer.None;
    public bool isPush { get; private set; } = false;
    public bool isStop = false;

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
            _mesh.enabled = false; // Tắt mesh của nếu obj bridge
        }
    }

    public void DisableComponent()
    {

        if (transform.CompareTag(TagConst.TAG_START_AREA) || transform.CompareTag(TagConst.TAG_WIN_AREA))
        {
            // _collider.enabled = true;
            return;
        }
        else if (transform.CompareTag(TagConst.TAG_BRIDGE))
        {
            // Tắt mesh của bricks
            if (_collider != null)
            {
                _mesh.enabled = true;
            }
            isPush = true;
            return;
        }

        if (_mesh != null) _mesh.enabled = false;
        isPush = true;
        Debug.Log($"DisableComponent: {gameObject.name} isPush: {isPush}");
    }
}
