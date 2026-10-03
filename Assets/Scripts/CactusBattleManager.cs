using UnityEngine;

public class CactusBattleManager : MonoBehaviour
{
    [SerializeField]
    private CactusController firstCactusController;
    [SerializeField]
    private CactusController secondCactusController;
    [SerializeField, Min(0f)]
    private float attackDistance = 0.25f;

    private bool fighting = false;

    private void Update()
    {
        if (firstCactusController == null || secondCactusController == null)
            return;

        bool bothCactiVisible = firstCactusController.IsVisible && secondCactusController.IsVisible;
        float distance = Vector3.Distance(firstCactusController.transform.position, secondCactusController.transform.position);

        if (!fighting && bothCactiVisible && distance < attackDistance)
        {
            fighting = true;

            firstCactusController.StartAttack(secondCactusController.transform);
            secondCactusController.StartAttack(firstCactusController.transform);
        }
        else if (fighting && (!bothCactiVisible || distance > attackDistance))
        {
            fighting = false;

            firstCactusController.StopAttack();
            secondCactusController.StopAttack();
        }
    }
}
