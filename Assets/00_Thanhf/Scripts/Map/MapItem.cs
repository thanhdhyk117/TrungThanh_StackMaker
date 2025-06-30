using UnityEngine;

public interface IInteractable
{
    bool Interact(PlayerController player);
}

public abstract class MapItem : MonoBehaviour, IInteractable
{
    [SerializeField] protected EDirectionPlayer directionPush = EDirectionPlayer.None;
    [SerializeField] protected bool isStop = false;
    protected bool isPush = false;

    protected Collider Collider { get; private set; }
    protected MeshRenderer MeshRenderer { get; private set; }

    protected virtual void Awake()
    {
        Initialize();
    }

    private void Reset()
    {
        Initialize();
    }

    private void Initialize()
    {
        Collider = GetComponent<Collider>();
        MeshRenderer = GetComponent<MeshRenderer>();

        if (Collider == null)
        {
            Collider = gameObject.AddComponent<BoxCollider>();
        }

        if (MeshRenderer == null)
        {
            MeshRenderer = gameObject.AddComponent<MeshRenderer>();
        }

        OnInitialize();
    }

    protected virtual void OnInitialize()
    {
    }

    public abstract bool Interact(PlayerController player);

    protected void SetPushState(bool state)
    {
        isPush = state;
    }

    public EDirectionPlayer DirectionPush => directionPush;
    public bool IsStop => isStop;
    public bool IsPush => isPush;
}