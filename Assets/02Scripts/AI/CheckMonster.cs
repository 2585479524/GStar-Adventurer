using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

public class CheckMonster : MonoBehaviour
{
    private BehaviorTree behaviorTree;
    private SharedGameObject target;
    private SphereCollider checkCollider;
    private List<Transform> targetMonsters = new List<Transform>();
    private GameObject player;

    private void Awake()
    {
        behaviorTree = GetComponentInParent<BehaviorTree>();
        target = (SharedGameObject)behaviorTree.GetVariable("player");
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        if (target.Value == null)
        {
            var newTarget = GetNearestTarget();
            target.Value = newTarget.gameObject;
        }
    }
    public void SetFollowRange(int range)
    {
        checkCollider = GetComponent<SphereCollider>();
        checkCollider.radius = range;
    }


    private void OnTriggerEnter(Collider other)
    {
        targetMonsters.Add(other.transform);
        var newTarget = GetNearestTarget();
        target.Value = newTarget.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        targetMonsters.Remove(other.transform);
        var newTarget = GetNearestTarget();
        target.Value = newTarget.gameObject;
    }

    public Transform GetNearestTarget()
    {
        for(int i = targetMonsters.Count-1; i >= 0; i--) 
        {
            if (targetMonsters[i] == null)
            {
                targetMonsters.Remove(targetMonsters[i]);
            }
        }

        Transform value = null;
        float distance = float.MaxValue;
        foreach(Transform target in targetMonsters)
        {
            float temp = Vector3.Distance(target.position, transform.position);
            if (distance > temp)
            {
                value = target;
                distance = temp;
            }
        }
        if (value == null)
        {
            value = player.transform;
        }
        return value;
    }
}
