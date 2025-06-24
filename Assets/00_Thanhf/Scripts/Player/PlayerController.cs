using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Enum định nghĩa hướng của player
    public enum EDirectionPlayer
    {
        None = 0,
        Forward = 1,
        Right = 2,
        Left = 3,
        Back = 4,
    }
    private Vector3 startPosTouch;
    private Vector3 endPosTouch;
    private Vector3 dirPlayerPos;
    private EDirectionPlayer currentDirection = EDirectionPlayer.None;

    private readonly Vector3[] _rotationVectors = {
        Vector3.zero,           // None
        new Vector3(0, 180, 0), // Forward
        new Vector3(0, 270, 0), // Right
        new Vector3(0, 90, 0),  // Left
        new Vector3(0, 0, 0)    // Back
    };

    // Tốc độ di chuyển của player
    [SerializeField] private Transform startPoint;
    [SerializeField] private MapItem _currentBrickRay;
    [SerializeField] BrickPlayer brickPlayer;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float _timeToMove = 0.01f;
    [SerializeField] private bool isMoving;

    private readonly Dictionary<string, System.Action> _tagActions = new Dictionary<string, System.Action>();

    private void Awake()
    {
        // Initialize tag actions
        _tagActions[TagConst.TAG_BRICK] = () => brickPlayer.Count++;
        _tagActions[TagConst.TAG_BRIDGE] = () => brickPlayer.Count--;
    }

    void Start()
    {
        Init();
    }
    private void Update()
    {
        GetInputTouch();
    }

    private void Init()
    {
        if (startPoint == null || brickPlayer == null)
        {
            Debug.LogError("Loi point start");
            enabled = false;
            return;
        }
        transform.position = startPoint.position;
        currentDirection = EDirectionPlayer.None;
        isMoving = false;
        dirPlayerPos = Vector3.zero;
    }

    private void GetInputTouch()
    {
        // Chuyển tọa độ chuột từ màn hình sang viewport
        var mousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            startPosTouch = mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            endPosTouch = mousePosition;

            // Tính hướng di chuyển
            dirPlayerPos = (endPosTouch - startPosTouch).normalized;

            // Xác định hướng từ vector
            currentDirection = GetDirectionFromVector(dirPlayerPos);
            // Xoay player theo hướng
            RotatePlayer();
            StartCoroutine(MoveStepByStep(_timeToMove));
        }
    }

    private IEnumerator MoveStepByStep(float timeToMove)
    {
        isMoving = true;

        while (currentDirection != EDirectionPlayer.None)
        {
            bool canMove = CastRay();

            if (!canMove || brickPlayer == null)
            {
                currentDirection = EDirectionPlayer.None;
                break;
            }

            // Dừng di chuyển nếu không đủ gạch để qua cầu
            if (_currentBrickRay != null && _currentBrickRay.CompareTag(TagConst.TAG_BRIDGE) && brickPlayer.Count <= 0)
            {
                currentDirection = EDirectionPlayer.None;
                _currentBrickRay = null;
                break;
            }

            Move();


            if (_currentBrickRay != null && _tagActions.TryGetValue(_currentBrickRay.tag, out var action))
            {
                action.Invoke();
                _currentBrickRay.SetMapType(0);
                _currentBrickRay = null;
            }
            yield return new WaitForSeconds(timeToMove);
        }

        isMoving = false;
    }

    private bool CastRay()
    {
        if (currentDirection == EDirectionPlayer.None)
        {
            return false;
        }

        Vector3 rayDirection = currentDirection switch
        {
            EDirectionPlayer.Forward => Vector3.forward,
            EDirectionPlayer.Back => Vector3.back,
            EDirectionPlayer.Left => Vector3.left,
            EDirectionPlayer.Right => Vector3.right,
            _ => Vector3.zero
        };

        Vector3 rayOrigin = new Vector3(transform.position.x, 0.1f, transform.position.z);
        bool hasHit = Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, 1f);

#if UNITY_EDITOR
        Vector3 endPoint = hasHit ? hit.point : rayOrigin + rayDirection;
        Debug.Log(hasHit ? $"Raycast hit: {hit.collider.name} at {hit.point}" : "Raycast missed");
        Debug.DrawRay(rayOrigin, rayDirection, Color.red, 1f);
#endif

        if (hasHit)
        {
            _currentBrickRay = hit.collider.GetComponent<MapItem>();
            return !hit.collider.CompareTag(TagConst.TAG_GROUND) && !hit.collider.CompareTag(TagConst.TAG_WIN_AREA);
        }

        _currentBrickRay = null;
        return true;
    }

    #region Controller
    // Xác định hướng từ vector
    private EDirectionPlayer GetDirectionFromVector(Vector3 direction)
    {
        if (direction.sqrMagnitude < 0.01f)
        {
            return EDirectionPlayer.None;
        }

        float absX = Mathf.Abs(direction.x);
        float absY = Mathf.Abs(direction.y);

        if (absX > absY)
        {
            return direction.x > 0 ? EDirectionPlayer.Right : EDirectionPlayer.Left;
        }
        return direction.y > 0 ? EDirectionPlayer.Forward : EDirectionPlayer.Back;
    }


    // Xoay player dựa trên hướng
    private void RotatePlayer()
    {
        if (currentDirection == EDirectionPlayer.None)
        {
            return;
        }

        transform.rotation = Quaternion.Euler(_rotationVectors[(int)currentDirection]);
    }

    // Di chuyển player dựa trên hướng
    private void Move()
    {
        Vector3 moveDirection = currentDirection switch
        {
            EDirectionPlayer.Forward => Vector3.forward,
            EDirectionPlayer.Back => Vector3.back,
            EDirectionPlayer.Left => Vector3.left,
            EDirectionPlayer.Right => Vector3.right,
            _ => Vector3.zero
        };

        transform.position += moveDirection * moveSpeed;
    }
    #endregion
}