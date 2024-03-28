using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentTest : MonoBehaviour
{
    private NavMeshAgent agent;
    private GameObject target;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = ActorController.Instance.CurrentActor.gameObject;
    }

    void Update()
    {
        agent.SetDestination(target.transform.position);
    }
}
