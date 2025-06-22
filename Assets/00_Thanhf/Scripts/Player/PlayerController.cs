using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Enum định nghĩa hướng của player
    public enum E_DirectionPlayer
    {
        NONE,
        FORWARD,
        BACK,
        LEFT,
        RIGHT
    }

    private Vector3 startPosTouch;
    private Vector3 endPosTouch;
    private Vector3 dirPlayerPos;
    private E_DirectionPlayer currentDirection = E_DirectionPlayer.NONE;

    // Tốc độ di chuyển của player
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private bool isMoving;
    [SerializeField] private Transform startPoint;
    [SerializeField] private GameObject _currentBrickRay;
    [SerializeField] BrickPlayer brickPlayer;
    [SerializeField] private float _timeToMove = 0.1f;
    private void Update()
    {
        GetInputTouch();
    }

    private void Init()
    {
        transform.position = startPoint.position;
        currentDirection = E_DirectionPlayer.NONE;
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

    private IEnumerator MoveStepByStep(float _time)
    {
        isMoving = true;

        while (currentDirection != E_DirectionPlayer.NONE)
        {
            bool canMove = CastRay();

            if (!canMove)
            {
                currentDirection = E_DirectionPlayer.NONE;
                break;
            }

            // Dừng di chuyển nếu không đủ gạch để qua cầu
            if (_currentBrickRay != null && _currentBrickRay.CompareTag(TagConst.TAG_BRIDGE) && brickPlayer.Count <= 0)
            {
                currentDirection = E_DirectionPlayer.NONE;
                _currentBrickRay = null;
                break;
            }

            Move();

            if (_currentBrickRay != null && _currentBrickRay.CompareTag(TagConst.TAG_BRICK))
            {
                brickPlayer.Count++;
                _currentBrickRay.GetComponent<BoxCollider>().enabled = false;
                _currentBrickRay.GetComponent<MeshRenderer>().enabled = false;
                _currentBrickRay = null;
            }
            if (_currentBrickRay != null && _currentBrickRay.CompareTag(TagConst.TAG_BRIDGE))
            {
                brickPlayer.Count--;
                _currentBrickRay.GetComponent<BoxCollider>().enabled = false;
                _currentBrickRay.GetComponent<MeshRenderer>().enabled = false;
                _currentBrickRay = null;
            }
            yield return new WaitForSeconds(_time);
        }
        isMoving = false;
    }

    private bool CastRay()
    {

        if (currentDirection == E_DirectionPlayer.NONE)
        {
            // return;
            return false;
        }

        // Xác định hướng raycast dựa trên currentDirection
        Vector3 rayDirection = Vector2.zero;
        switch (currentDirection)
        {
            case E_DirectionPlayer.FORWARD:
                rayDirection = Vector3.forward;
                break;
            case E_DirectionPlayer.BACK:
                rayDirection = Vector3.back;
                break;
            case E_DirectionPlayer.LEFT:
                rayDirection = Vector3.left;
                break;
            case E_DirectionPlayer.RIGHT:
                rayDirection = Vector3.right;
                break;
        }

        // Tạo raycast từ vị trí player
        Vector3 rayOrigin = new Vector3(transform.position.x, 0.1f, transform.position.z);
        RaycastHit hit;

        bool hasHit = Physics.Raycast(rayOrigin, rayDirection, out hit, 1);

        Vector3 endPoint;
        if (hasHit)
        {
            endPoint = hit.point;
            Debug.Log($"Raycast hit: {hit.collider.name} at {hit.point}");
            _currentBrickRay = hit.collider.gameObject;
            return !hit.collider.CompareTag(TagConst.TAG_GROUND) && !hit.collider.CompareTag(TagConst.TAG_WIN_AREA);
        }
        else
        {
            endPoint = rayOrigin + rayDirection;
            return true;
        }

        Debug.DrawRay(rayOrigin, (endPoint - rayOrigin), Color.red, 1f);
    }

    #region Controller
    // Xác định hướng từ vector
    private E_DirectionPlayer GetDirectionFromVector(Vector3 direction)
    {
        // Nếu vector quá nhỏ, trả về None
        // Xác định hướng dựa trên góc
        // Right: 315°-45°, Top: 45°-135°, Left: 135°-225°, Down: 225°-315°

        if (direction.sqrMagnitude < 0.01f)
        {
            return E_DirectionPlayer.NONE;
        }

        // Tính góc của vector (độ)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (angle < 0)
        {
            angle += 360;
        }


        if (angle >= 315f || angle < 45f)
        {
            return E_DirectionPlayer.RIGHT;
        }
        else if (angle >= 45f && angle < 135f)
        {
            return E_DirectionPlayer.FORWARD;
        }
        else if (angle >= 135f && angle < 225f)
        {
            return E_DirectionPlayer.LEFT;
        }
        else
        {
            return E_DirectionPlayer.BACK;
        }
    }
    // Xoay player dựa trên hướng
    private void RotatePlayer()
    {
        Vector3 rotation = Vector3.zero;
        switch (currentDirection)
        {
            case E_DirectionPlayer.FORWARD:
                rotation = new Vector3(0, 180, 0); // Hướng tiến về phía trước
                break;
            case E_DirectionPlayer.BACK:
                rotation = new Vector3(0, 0, 0); // Hướng tiến về phía sau
                break;
            case E_DirectionPlayer.LEFT:
                rotation = new Vector3(0, 90, 0); // Hướng trái
                break;
            case E_DirectionPlayer.RIGHT:
                rotation = new Vector3(0, 270, 0); // Hướng phải
                break;
            case E_DirectionPlayer.NONE:
                return; // Không xoay nếu không có hướng
        }
        transform.rotation = Quaternion.Euler(rotation);
    }

    // Di chuyển player dựa trên hướng
    private void Move()
    {
        Vector3 moveDirection = Vector3.zero;

        switch (currentDirection)
        {
            case E_DirectionPlayer.FORWARD:
                moveDirection = Vector3.forward;
                break;
            case E_DirectionPlayer.BACK:
                moveDirection = Vector3.back;
                break;
            case E_DirectionPlayer.LEFT:
                moveDirection = Vector3.left;
                break;
            case E_DirectionPlayer.RIGHT:
                moveDirection = Vector3.right;
                break;
            case E_DirectionPlayer.NONE:
                moveDirection = Vector3.zero;
                break;
        }

        // Di chuyển player
        transform.position += moveDirection * moveSpeed;
    }
    #endregion
}