using System.Collections;
using UnityEngine;

public enum EDirectionPlayer
{
    None = 0,
    Forward = 1,
    Right = 2,
    Left = 3,
    Back = 4,
}

public class PlayerController : MonoBehaviour
{
    private Vector3 startPosTouch;
    private Vector3 endPosTouch;
    private Vector3 dirPlayerPos;
    private EDirectionPlayer currentDirection = EDirectionPlayer.None;
    private Coroutine moveCoroutine;

    private readonly Vector3[] _rotationVectors = {
        Vector3.zero,           // None
        new Vector3(0, 180, 0), // Forward
        new Vector3(0, 270, 0), // Right
        new Vector3(0, 90, 0),  // Left
        new Vector3(0, 0, 0)    // Back
    };

    [SerializeField] private Transform startPoint;
    [SerializeField] private BrickPlayer brickPlayer;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float timeToMove = 0.01f;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private int animStateCount = 0;

    [Header("Dependencies")]
    [SerializeField] private LevelComplete levelComplete;
    public LevelComplete LevelComplete => levelComplete;
    [SerializeField] private UIController uiController;

    private int score;
    private MapItem currentBrickRay;

    public int Score
    {
        get => score;
        set
        {
            score = value;
            if (uiController != null) uiController.FillAmount += value;
        }
    }

    public BrickPlayer BrickPlayer => brickPlayer;


    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        GetInputTouch();
    }

    private void Initialize()
    {
        if (startPoint == null || brickPlayer == null)
        {
            Debug.LogError("StartPoint or BrickPlayer is not assigned!");
            enabled = false;
            return;
        }
        transform.position = startPoint.position;
        currentDirection = EDirectionPlayer.None;
        dirPlayerPos = Vector3.zero;
    }

    private void GetInputTouch()
    {
        if (Camera.main == null) return;

        var mousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            startPosTouch = mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            endPosTouch = mousePosition;
            dirPlayerPos = (endPosTouch - startPosTouch).normalized;
            currentDirection = GetDirectionFromVector(dirPlayerPos);
            RotatePlayer();
            if (moveCoroutine != null) StopCoroutine(moveCoroutine);
            moveCoroutine = StartCoroutine(MoveStepByStep(timeToMove));
        }
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

        Vector3 rayOrigin = new Vector3(transform.position.x, transform.root.position.y + 0.1f, transform.position.z);
        bool hasHit = Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, 1f);
        Debug.DrawRay(rayOrigin, rayDirection * 1f, hasHit ? Color.red : Color.green, 1f);

        if (hasHit)
        {
            if (hit.collider.CompareTag(TagConst.TAG_GROUND))
            {
                Debug.Log("Hit wall (TAG_GROUND)");
                return false;
            }

            var interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                currentBrickRay = hit.collider.GetComponent<MapItem>();
                return interactable.Interact(this);
            }

            currentBrickRay = null;
            Debug.Log("No interactable component, allowing movement");
            return true;
        }

        currentBrickRay = null;
        Debug.Log("Raycast missed, allowing movement");
        return true;
    }

    private IEnumerator MoveStepByStep(float timeToMove)
    {
        while (currentDirection != EDirectionPlayer.None)
        {
            bool canMove = CastRay();

            if (!canMove || brickPlayer == null)
            {
                StopMoving();
                break;
            }

            Move();

            if (currentBrickRay != null && currentBrickRay.IsStop)
            {
                Debug.Log("Stopping at stop point after moving");
                StopMoving();
                break;
            }

            if (currentBrickRay != null && currentBrickRay.DirectionPush != EDirectionPlayer.None && !currentBrickRay.IsStop)
            {
                currentDirection = currentBrickRay.DirectionPush;
                RotatePlayer();
                currentBrickRay = null;
            }

            yield return new WaitForSeconds(timeToMove);
        }

        moveCoroutine = null;
    }

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

    private void RotatePlayer()
    {
        if (currentDirection == EDirectionPlayer.None)
        {
            return;
        }

        transform.rotation = Quaternion.Euler(_rotationVectors[(int)currentDirection]);
    }

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

    private void StopMoving()
    {
        currentDirection = EDirectionPlayer.None;
        currentBrickRay = null;
    }

    public void EndLevel()
    {
        StopMoving();
        Debug.Log("Level Ended");
        StartCoroutine(EffectEndLevel());
    }

    private IEnumerator EffectEndLevel()
    {
        while (brickPlayer != null && brickPlayer.Count > 0)
        {
            brickPlayer.Count--;
            Score++;
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitUntil(() => brickPlayer == null || brickPlayer.Count == 0);
        if (levelComplete != null) levelComplete.EffectEndLevel();
    }

    public void ChangeAnim(int animationIndex)
    {
        if (animator == null || animationIndex < 0)
        {
            return;
        }

        if (animStateCount != animationIndex)
        {
            animator.SetInteger("renwu", animationIndex);
            animStateCount = animationIndex;
        }
    }

    public void SetTimeToMove(float time)
    {
        timeToMove = time;
    }
}