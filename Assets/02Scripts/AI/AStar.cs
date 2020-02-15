using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A星寻路
public static class AStar 
{
    // 内部类Node
    public class Node : IComparable
    {
        private const int Cost = 10;
        public Vector3Int pos;
        public Node Parent;

        public int G;
        public int H;
        public int F;

        public Node()
        {
            Parent = null;
            G = 0;
        }

        public Node(Vector3Int _pos)
        {
            Parent = null;
            pos = _pos;
            G = 0;
        }

        public Node(int x, int y, int z)
        {
            pos.x = x;
            pos.y = y;
            pos.z = z;
        }

        //获取父节点
        public void SetParent(Node node)
        {
            Parent = node;
            G = node.G + Cost;
        }

        //设置终点、获取H值
        public void SetGoal(Vector3Int goalPos)
        {
            H = getHCost(pos, goalPos);
        }

        public void UpdateF()
        {
            F = G + H;
        }

        private int getHCost(Vector3Int nowPos, Vector3Int goalPos)
        {
            int dx = Mathf.Abs(nowPos.x - goalPos.x);
            int dz = Mathf.Abs(nowPos.z - goalPos.z);
            return Cost * (dx + dz);
        }

        public int CompareTo(object obj)
        {
            Node node = (Node)obj;
            if (F < node.F)
                return -1;//升序
            if (F > node.F)
                return 1;
            return 0;
        }

        public override string ToString()
        {
            return pos.x + " " + pos.y + " " + pos.z + " " + G + " " + H + " " + F;
        }
    }

    private static int heightSize = 30;
    private static int mapCell = 21;
    private static int heightCell = 9;
    private static int distance;
    public static int[,,] heightMap;//三维数组
    private static Node[,,] nodeMap;//地图方块信息
    private const int hCost = 10;//曼哈顿代价常数
    private static List<Node> openList = new List<Node>();//开放有限队列
    private static List<Node> closeList = new List<Node>();//小黑屋
    private static Stack<Vector3Int> pathStack = new Stack<Vector3Int>(); //寻路结果栈

    // 对——角色控制接口，提供一条路径
      private static int[] findRoundArray = { -1,0, -1,-1, 0,-1, 1,-1, 1,0, 1,1, 0,1, -1,1 };
    //第三个参数检测寻路判断类型  
    public static Stack<Vector3Int> FindPath(Vector3Int playerPos, Vector3Int goalPos, int pathType)
    {
        //清空缓存
        pathStack.Clear();
        openList.Clear();
        closeList.Clear();
        distance = 0;

        nodeMap = new Node[mapCell, heightCell, mapCell];
        for (int i = 0; i < mapCell; i++)
            for (int j = 0; j < heightCell; j++)
                for (int k = 0; k < mapCell; k++)
                    nodeMap[i, j, k] = new Node(playerPos.x - mapCell / 2 + i, playerPos.y - heightCell/2 + j, playerPos.z - mapCell / 2 + k);

        Vector3Int nowPos = playerPos;
        Node startNode = nodeMap[mapCell / 2, heightCell / 2, mapCell / 2];
        openList.Add(startNode);//1、添加起点

        //循环搜索
        while (openList.Count != 0)
        {
            Node nowNode = openList[0];//取出结点
            nowPos = nowNode.pos;

            //判断是否找到终点
            if (nowPos.x == goalPos.x && nowPos.z == goalPos.z && nowPos.y == goalPos.y)
            {
                DealPathStack(nowNode);
                return pathStack;
            }

            //遍历周围节点
            for (int i = 0; i <= findRoundArray.Length - 2; i += 2)
            {
                Vector3Int nextPos = new Vector3Int(nowPos.x + findRoundArray[i], nowPos.y, nowPos.z + findRoundArray[i + 1]);
                if (isWalkable(nowPos, ref nextPos, pathType, i, playerPos))//查看是否可达
                {
                    int x = nextPos.x + mapCell/2  -playerPos.x; 
                    int y = nextPos.y + heightCell/2 - playerPos.y;
                    int z = nextPos.z + mapCell/2  -playerPos.z;
                    if(y < 0 || y >= heightCell)
                    {
                        continue;
                    }
                    //Debug.Log(nextPos + " " + x + " " + y + " " + z);
                    Node nextNode = nodeMap[x, y, z];

                    int g = nowNode.G + 10;

                    if (nodeMap[x, y, z].G == 0 || nodeMap[x, y, z].G > g) //是否需要更新父亲节点
                    {
                        nodeMap[x, y, z].G = g;
                        nodeMap[x, y, z].SetParent(nowNode);
                    }
                    nodeMap[x, y, z].SetGoal(goalPos);
                    nodeMap[x, y, z].UpdateF();

                    if (!openList.Contains(nextNode))
                    {
                        openList.Add(nextNode);
                    }
                    openList.Sort();//排序
                }
            }

            //now从open移除，加入close
            openList.Remove(nowNode);
            closeList.Add(nowNode);//进入小黑屋
            distance += 1;
            if (distance >= 25)
                return null;
        }
        return null;//没有结果
    }


    //是否可以到达判断条件
    private static bool isWalkable(Vector3Int nowPos, ref Vector3Int nextPos, int pathType, int index, Vector3Int playerPos)
    {
        //斜方向判断两侧是否有方块
        if (index == 2)
            if (Chunk.GetBlock(nowPos - new Vector3(1, 0, 0)) != BlockType.None || Chunk.GetBlock(nowPos - new Vector3(0, 0, 1)) != BlockType.None)
                return false;
        if (index == 6)
            if (Chunk.GetBlock(nowPos + new Vector3(1, 0, 0)) != BlockType.None || Chunk.GetBlock(nowPos - new Vector3(0, 0, 1)) != BlockType.None)
                return false;
        if (index == 10)
            if (Chunk.GetBlock(nowPos + new Vector3(1, 0, 0)) != BlockType.None || Chunk.GetBlock(nowPos + new Vector3(0, 0, 1)) != BlockType.None)
                return false;
        if (index == 14)
            if (Chunk.GetBlock(nowPos - new Vector3(1, 0, 0)) != BlockType.None || Chunk.GetBlock(nowPos + new Vector3(0, 0, 1)) != BlockType.None)
                return false;

        //不能跳 
        if (pathType == 0)
        {
            //若该点有方块
            if (Chunk.GetBlock(nextPos) != BlockType.None)
            {
                return false;
            }

            if (Chunk.GetBlock(nextPos) == BlockType.None)
            {
                if (nextPos.y > 1)
                {
                    if (Chunk.GetBlock(nextPos - new Vector3(0, 1, 0)) == BlockType.None)
                    {
                        nextPos.y -= 1;
                        //if (nextPos.y < playerPos.y && Chunk.GetBlock(nextPos - new Vector3(0, 1, 0)) == BlockType.None)
                        //{
                        //    nextPos.y += 1;
                        //    return false;
                        //}
                    }
                }
            }
        }

        //能跳 
        if (pathType == 1)
        {
            //如果该点没有方块，判断下方两点是否有方块, 若下方两点都没有方块，则绕过
            if (Chunk.GetBlock(nextPos) == BlockType.None)
            {
                if (nextPos.y > 1)
                {
                    if (Chunk.GetBlock(nextPos - new Vector3(0, 1, 0)) == BlockType.None)
                    {
                        nextPos.y -= 1;
                        if (nextPos.y < playerPos.y  && Chunk.GetBlock(nextPos - new Vector3(0, 1, 0)) == BlockType.None)
                        {
                            nextPos.y += 1;
                            return false;
                        }
                    }
                }
            }

            //如果该点有方块，判断上方一点是否有方块, 若上方一点有方块，则绕过
            if (Chunk.GetBlock(nextPos) != BlockType.None)
            {
                if (nextPos.y < heightSize - 1)
                {
                    if (Chunk.GetBlock(nextPos + new Vector3(0, 1, 0)) != BlockType.None)
                    {
                        return false;
                    }
                    else
                    {
                        nextPos.y += 1;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        //closelist判断
        for (int i = 0; i < closeList.Count; i++)
            if (closeList[i].pos.x == nextPos.x && closeList[i].pos.z == nextPos.z)
                return false;

        return true;
    }

    //vector转node
    private static Node Vector3Node(Vector3Int pos)
    {
        return new Node(pos);
    }
    private static Vector3Int Node3Vector(Node node)
    {
        return new Vector3Int(node.pos.x, node.pos.y, node.pos.z);
    }
    private static void DealPathStack(Node node)
    {
        if (node.Parent == null)
        {
            return;
        }
       // Debug.Log(node);
        pathStack.Push(Node3Vector(node));
        DealPathStack(node.Parent);
    }
}
