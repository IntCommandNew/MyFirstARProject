using UnityEngine;
using Vuforia;

public class CactusController : MonoBehaviour
{
    [SerializeField]
    private string attackTrigger = "TrAttack";
    [SerializeField]
    private string idleTrigger = "TrIdle";

    [SerializeField, Min(1f)]
    private float rotationSpeed = 300f;

    private Animator animator;
    private ObserverBehaviour imageTarget;
    private Quaternion originalLocalRotation;
    private Transform opponentPosition;

    public bool IsAttacking { get; private set; }

    public bool IsVisible
    {
        get
        {
            if (imageTarget == null)
                return true;

            var status = imageTarget.TargetStatus.Status;

            return status == Status.TRACKED || status == Status.EXTENDED_TRACKED;
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        imageTarget = GetComponentInParent<ObserverBehaviour>();
        originalLocalRotation = transform.localRotation;
    }

    private void Update()
    {
        if (IsAttacking && opponentPosition != null)
            TurnTowardsWorldPoint(opponentPosition.position);
        else
            ReturnToOriginalRotation();
    }

    public void StartAttack(Transform position)
    {
        opponentPosition = position;
        if (IsAttacking)
            return;

        IsAttacking = true;

        animator.ResetTrigger(idleTrigger);
        animator.SetTrigger(attackTrigger);
    }

    public void StopAttack()
    {
        opponentPosition = null;
        if (!IsAttacking)
            return;

        IsAttacking = false;

        animator.ResetTrigger(attackTrigger);
        animator.SetTrigger(idleTrigger);
    }

    private void TurnTowardsWorldPoint(Vector3 worldPoint)
    {
        Vector3 up = transform.parent != null ? transform.parent.up : Vector3.up;
        Vector3 direction = Vector3.ProjectOnPlane(worldPoint - transform.position, up);
        if (direction.sqrMagnitude < 0.000001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction, up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void ReturnToOriginalRotation()
    {
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, originalLocalRotation, rotationSpeed * Time.deltaTime);
    }
}
