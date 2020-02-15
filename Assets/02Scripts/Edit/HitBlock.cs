using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class HitBlock : MonoBehaviour
{
    Vector3 MouseSeleCubePos;
    bool IsDele;
    public Text IM;
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            IsDele = false;
            IM.text = "添加模式";
        }
        if (Input.GetKey(KeyCode.W))
        {
            IsDele = true;
            IM.text = "删除模式";
        }
        if (Input.GetMouseButtonDown(0))
        {
            // 根据3D空间中的点坐标，获得对应的大Chunk对象，Chunk里已经提供了该方法
            Chunk chunk = Chunk.GetChunk(MouseSeleCubePos);
            if (!chunk)
            {
                Debug.Log("NoChunk");
                return;
            }
            if (IsDele)
            {
                chunk.ChangeBlock(MouseSeleCubePos, BlockType.None);
            }
            else
            {
                chunk.ChangeBlock(MouseSeleCubePos, BlockType.DragonFruit);
            }
        }
    }



    public static Vector3 FromWorldPositionToCubePosition(Vector3 position)
    {
        Vector3 resut = Vector3.zero;
        resut.x = position.x > 0 ? (int)position.x * 1f + 0.5f : (int)position.x * 1f - 0.5f;
        resut.y = position.y > 0 ? (int)position.y * 1f + 0.5f : (int)position.y * 1f - 0.5f;
        resut.z = position.z > 0 ? (int)position.z * 1f + 0.5f : (int)position.z * 1f - 0.5f;
        return resut;
    }
    bool GetMouseRayPoint(out Vector3 addCubePosition)
    {
        /*if (!HaveTouchNoUi)
        {
            addCubePosition = Vector3.zero;
            hitObject = null;
            return false;
        }*/
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        // LayerMask layerMask = 1 << 10;
        if (Physics.Raycast(ray, out hitInfo))
        {
            //hitObject = hitInfo.collider.gameObject;
            //Debug.DrawRay(hitInfo.point, Vector3.up, Color.red);
            Vector3 point;
            if (IsDele)
            {
                point = hitInfo.point + ray.direction * 0.01f;
            }
            else
            {
                point = hitInfo.point - ray.direction * 0.01f;
            }

            addCubePosition = FromWorldPositionToCubePosition(point);

            return true;
        }
        addCubePosition = Vector3.zero;
        return false;
    }

    private void OnDrawGizmos()
    {
        if (GetMouseRayPoint(out MouseSeleCubePos))
        {
            Gizmos.DrawWireCube(MouseSeleCubePos, Vector3.one);
        }
    }


}