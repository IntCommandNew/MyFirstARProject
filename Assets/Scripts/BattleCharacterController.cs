using UnityEngine;
using Vuforia;

public class BattleCharacterController : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField, Min(0f)]
    private float rotationSpeed = 10f;

    public bool IsVisible;

    private Transform currentTarget;
    private ObserverBehaviour observerBehaviour;

    private void Awake()
    {
        observerBehaviour = GetComponentInParent<ObserverBehaviour>();
    }

    private void OnEnable()
    {
        if (observerBehaviour != null)
        {
            observerBehaviour.OnTargetStatusChanged += OnTargetStatusChanged;
        }
    }

    private void OnDisable()
    {
        if (observerBehaviour != null)
        {
            observerBehaviour.OnTargetStatusChanged -= OnTargetStatusChanged;
        }
    }

    private void Update()
    {
        if (currentTarget == null)
            return;

        RotateTowardsTarget();
    }

    private void OnTargetStatusChanged(
        ObserverBehaviour behaviour,
        TargetStatus status)
    {
        IsVisible =
            status.Status == Status.TRACKED ||
            status.Status == Status.EXTENDED_TRACKED;

        if (!IsVisible)
        {
            StopAttack();
        }
    }

    public void StartAttack(Transform target)
    {
        if (target == null)
            return;

        currentTarget = target;

        animator.SetBool("isAttacking", true);
    }

    public void StopAttack()
    {
        currentTarget = null;

        animator.SetBool("isAttacking", false);
    }

    private void RotateTowardsTarget()
    {
        Vector3 direction = currentTarget.position - transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}