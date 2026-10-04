using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField, Min(0f)]
    private float attackDistance = 0.25f;

    private BattleCharacterController[] characters;

    private void Start()
    {
        characters = FindObjectsByType<BattleCharacterController>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
        );
    }

    private void Update()
    {
        foreach (BattleCharacterController character in characters)
        {
            if (character == null || !character.IsVisible)
            {
                if (character != null)
                    character.StopAttack();

                continue;
            }

            BattleCharacterController nearestEnemy =
                FindNearestEnemy(character);

            if (nearestEnemy == null)
            {
                character.StopAttack();
                continue;
            }

            float distance = Vector3.Distance(
                character.transform.position,
                nearestEnemy.transform.position
            );

            if (distance <= attackDistance)
            {
                character.StartAttack(nearestEnemy.transform);
            }
            else
            {
                character.StopAttack();
            }
        }
    }

    private BattleCharacterController FindNearestEnemy(
        BattleCharacterController character)
    {
        BattleCharacterController nearestEnemy = null;
        float nearestDistance = float.MaxValue;

        foreach (BattleCharacterController otherCharacter in characters)
        {
            if (otherCharacter == null ||
                otherCharacter == character ||
                !otherCharacter.IsVisible)
            {
                continue;
            }

            float distance = Vector3.Distance(
                character.transform.position,
                otherCharacter.transform.position
            );

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = otherCharacter;
            }
        }

        return nearestEnemy;
    }
}