using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFragment : MonoBehaviour
{

    public float followSpeed = 1;
    public bool flyToPlayer = false;  //是否直接飞向玩家
    public float rotateSpeed = 300;

    private Transform playerPos;

    private bool canFollow = false;

    private void Start()
    {
        playerPos = GameObject.Find("Player").transform;
    }

    void Update()
    {
        if (flyToPlayer)
        {
            FlyToPlayer();
            return;
        }
        transform.Rotate(Vector3.up, Time.deltaTime * rotateSpeed);
        if (canFollow)
        {
            FlyToPlayer();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canFollow = true;
            MessageManager.Instance.EatPudding();
            SoundManager.Instance.PlaySfx("GetStar", transform.position);
            Destroy(gameObject);
        }
    }

    private void FlyToPlayer()
    {
        Vector3 dir = (playerPos.position - transform.position).normalized;
        transform.Translate(dir * followSpeed * Time.deltaTime);
    }
}
