using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public enum OffMeshLinkMoveMethod
{
    Teleport,
    NormalSpeed,
    Parabola
}

[RequireComponent(typeof(NavMeshAgent))]
public class AgentLinkMover : MonoBehaviour
{
    public static AgentLinkMover instance;

    public OffMeshLinkMoveMethod method = OffMeshLinkMoveMethod.Parabola;

    private void Awake()
    {
        instance = this;
    }

    IEnumerator Start()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.autoTraverseOffMeshLink = false;
        while (true)
        {
            if (agent.isOnOffMeshLink)
            {
                transform.LookAt(agent.currentOffMeshLinkData.endPos);
                agent.updateRotation = false;

                if (method == OffMeshLinkMoveMethod.NormalSpeed)
                    yield return StartCoroutine(NormalSpeed(agent));
                else if (method == OffMeshLinkMoveMethod.Parabola)
                    yield return StartCoroutine(Parabola(agent, 2.0f, 0.5f));
                agent.CompleteOffMeshLink();
                agent.updateRotation = true;
            }
            yield return null;
        }
    }

    IEnumerator NormalSpeed(NavMeshAgent agent)
    {
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
        while (agent.transform.position != endPos)
        {
            agent.transform.position = Vector3.MoveTowards(agent.transform.position, endPos, agent.speed * Time.deltaTime);
            yield return null;
        }

    }

    IEnumerator Parabola(NavMeshAgent agent, float height, float duration)
    {
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 startPos = agent.transform.position;
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
        float normalizedTime = 0.0f;
        while (normalizedTime < 1.0f)
        {
            float yOffset = height * 4.0f * (normalizedTime - normalizedTime * normalizedTime);
            agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
    }
}