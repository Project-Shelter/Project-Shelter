using NavMeshPlus.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum Agent
{
    WithObjects,
    WithoutObjects
}

public class NavMeshController : MonoSingleton<NavMeshController>
{
    private Dictionary<Agent, int> agentTypeId = new();
    private Dictionary<Agent, NavMeshAgent> agents = new();
    private Dictionary<Agent, NavMeshSurface> surfaces = new();

    private void Awake()
    {
        BindAgentTypeId();
    }

    private void BindAgentTypeId()
    {
        foreach (Agent agent in Enum.GetValues(typeof(Agent)))
        {
            GameObject agentObject = new GameObject(agent.ToString());
            NavMeshAgent navMeshAgent = agentObject.AddComponent<NavMeshAgent>();

            for (int i = 0; i < NavMesh.GetSettingsCount(); i++)
            {
                NavMeshBuildSettings settings = NavMesh.GetSettingsByIndex(index: i);
                if (agent.ToString() == NavMesh.GetSettingsNameFromID(agentTypeID: settings.agentTypeID))
                {
                    navMeshAgent.agentTypeID = settings.agentTypeID;
                    agentTypeId[agent] = settings.agentTypeID;
                    agents[agent] = navMeshAgent;
                    surfaces[agent] = Util.FindChild<NavMeshSurface>(gameObject, agent.ToString() + "Mesh");
                    surfaces[agent].BuildNavMesh();
                    break;
                }
            }
        }
    }

    public float CalculatePathLength(NavMeshPath path)
    {
        float length = 0.0f;

        if (path.status != NavMeshPathStatus.PathInvalid && path.corners.Length > 1)
        {
            for (int i = 1; i < path.corners.Length; ++i)
            {
                length += Vector3.Distance(path.corners[i - 1], path.corners[i]);
            }
        }

        return length;
    }

    public float CalculateDiff(Vector3 startPosition, Vector3 targetPosition)
    {
        NavMeshPath pathWithObstacles = new NavMeshPath();
        NavMeshPath pathWithoutObstacles = new NavMeshPath();

        agents[Agent.WithObjects].enabled = false;
        agents[Agent.WithoutObjects].enabled = false;
        agents[Agent.WithObjects].transform.position = startPosition;
        agents[Agent.WithoutObjects].transform.position = startPosition;
        agents[Agent.WithObjects].enabled = true;
        agents[Agent.WithoutObjects].enabled = true;

        agents[Agent.WithObjects].CalculatePath(targetPosition, pathWithObstacles);
        agents[Agent.WithoutObjects].CalculatePath(targetPosition, pathWithoutObstacles);

        float lengthWithObstacles = CalculatePathLength(pathWithObstacles);
        float lengthWithoutObstacles = CalculatePathLength(pathWithoutObstacles);
        float difference = Mathf.Abs(lengthWithObstacles - lengthWithoutObstacles);
        return difference;
    }

    public void ChangeAgentType(NavMeshAgent agent, Agent agentType)
    {
        agent.agentTypeID = agentTypeId[agentType];
    }

    public void UpdateMesh(Agent agentType)
    {
        surfaces[agentType].UpdateNavMesh(surfaces[agentType].navMeshData);
        //surfaces[agentType].BuildNavMesh();
    }
}
