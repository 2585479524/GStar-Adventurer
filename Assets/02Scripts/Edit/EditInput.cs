using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditInput : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 endPos;
    public InputField[] text;
    Chunk choosedChunk;
    EditMode editMode;
    private void Start()
    {
        editMode = GameObject.Find("Player").GetComponent<EditMode>();
    }
    public void ChangeValue() {
        if (text[0].text!="")
            startPos.x = int.Parse(text[0].text) + 0.5f;
        if (text[1].text != "")
            startPos.y = int.Parse(text[1].text) + 0.5f;
        if (text[2].text != "")
            startPos.z = int.Parse(text[2].text) + 0.5f;
        if (text[3].text != "")
            endPos.x = int.Parse(text[3].text) + 0.5f;
        if (text[4].text != "")
            endPos.y = int.Parse(text[4].text) + 0.5f;
        Debug.Log(text[5].text);
        if (text[5].text != "")
            endPos.z = int.Parse(text[5].text) + 0.5f;
    }


    public void Replace() {
        Debug.Log(startPos);
        Debug.Log(endPos);
        int a = 1, b = 1, c = 1;
        if (startPos.x > endPos.x)
            a = -1;
        if (startPos.y > endPos.y)
            b = -1;
        if (startPos.z > endPos.z)
            c = -1;

        List<Chunk> chunks = new List<Chunk>();
        for (float i = startPos.x; i <= endPos.x; i += a)
        {
            for (float j = startPos.y; j <= endPos.y; j += b)
            {
                for (float k = startPos.z; k <= endPos.z; k += c)
                {
                    Vector3 blockPos = new Vector3(i, j, k);
                    Chunk chunk = Chunk.GetChunk(blockPos);
                    if (!(chunks.Contains(chunk)))
                        chunks.Add(chunk);
                    chunk.SetBlock(blockPos, (BlockType)(editMode.GetSele()));
                }
            }
        }
        for (int i = 0; i < chunks.Count; i++)
        {
            chunks[i].BuildChunk();
        }
    }

    public void Delete()
    {
        Debug.Log(startPos);
        Debug.Log(endPos);
        int a = 1, b = 1, c = 1;
        if (startPos.x > endPos.x)
            a = -1;
        if (startPos.y > endPos.y)
            b = -1;
        if (startPos.z > endPos.z)
            c = -1;
        List<Chunk> chunks = new List<Chunk>();
        for (float i = startPos.x; i <= endPos.x; i += a)
        {
            for (float j = startPos.y; j <= endPos.y; j += b)
            {
                for (float k = startPos.z; k <= endPos.z; k += c)
                {
                    Vector3 blockPos = new Vector3(i, j, k);
                    Chunk chunk = Chunk.GetChunk(blockPos);
                    if (!(chunks.Contains(chunk)))
                        chunks.Add(chunk);
                    chunk.SetBlock(blockPos, BlockType.None);
                }
            }
        }
        for (int i = 0; i < chunks.Count; i++)
        {
            chunks[i].BuildChunk();
        }
        
    }

    public void Change()
    {
        Debug.Log(startPos);
        Debug.Log(endPos);
        int a = 1, b = 1, c = 1;
        if (startPos.x > endPos.x)
            a = -1;
        if (startPos.y > endPos.y)
            b = -1;
        if (startPos.z > endPos.z)
            c = -1;

        List<Chunk> chunks = new List<Chunk>();
        for (float i = startPos.x; i <= endPos.x; i += a)
        {
            for (float j = startPos.y; j <= endPos.y; j += b)
            {
                for (float k = startPos.z; k <= endPos.z; k += c)
                {
                    Vector3 blockPos = new Vector3(i, j, k);
                    Chunk chunk = Chunk.GetChunk(blockPos);
                    if (!(chunks.Contains(chunk)))
                        chunks.Add(chunk);
                    if(Chunk.GetBlock(blockPos)!= BlockType.None)
                        chunk.SetBlock(blockPos, (BlockType)(editMode.GetSele()));
                }
            }
        }
        for (int i = 0; i < chunks.Count; i++)
        {
            chunks[i].BuildChunk();
        }
    }

}
