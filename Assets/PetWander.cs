using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class PetWander : MonoBehaviour
{
    [Header("Wandering Settings")]
    public float wanderRadius = 5f;
    public float minWaitTime = 1f;
    public float maxWaitTime = 4f;
    public float moveSpeed = 2f;

    [Header("Player")]
    public Transform player;
    public float maxDistanceFromPlayer = 8f;

    private NavMeshAgent agent;
    private bool isWandering;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            agent.speed = moveSpeed;
            agent.stoppingDistance = 0.3f;
        }

        StartCoroutine(WanderRoutine());
    }

    private IEnumerator WanderRoutine()
    {
        while (true)
        {
            if (player == null)
            {
                yield return new WaitForSeconds(1f);
                continue;
            }

            if (agent == null || !agent.enabled)
            {
                yield return new WaitForSeconds(1f);
                continue;
            }

            Vector3 randomPosition =
                player.position +
                Random.insideUnitSphere * wanderRadius;

            randomPosition.y = player.position.y;

            NavMeshHit hit;

            if (NavMesh.SamplePosition(
                randomPosition,
                out hit,
                wanderRadius,
                NavMesh.AllAreas))
            {
                if (Vector3.Distance(
                    hit.position,
                    player.position) <= maxDistanceFromPlayer)
                {
                    agent.SetDestination(hit.position);

                    isWandering = true;

                    while (agent.pathPending)
                        yield return null;

                    while (agent.enabled &&
                           !agent.pathPending &&
                           agent.remainingDistance >
                           agent.stoppingDistance)
                    {
                        yield return null;
                    }

                    isWandering = false;
                }
            }

            float waitTime =
                Random.Range(minWaitTime, maxWaitTime);

            yield return new WaitForSeconds(waitTime);
        }
    }
}