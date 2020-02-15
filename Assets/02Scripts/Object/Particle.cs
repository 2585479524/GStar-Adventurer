using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ParticleType
{
    EatHPCube,
    Success,
    Boom,
    Failed,
    Jump,

    PutCube,
    StarTrail
}
public class Particle : MonoBehaviour
{
    public List<ParticleSystem> particles = new List<ParticleSystem>();

    public ParticleSystem g;
    private bool isInit = false;
    private Vector3 offset;
    private void Awake()
    {
        MessageManager.Instance.InstanceParticle += InstanceParticle;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (g != null && isInit && g.isStopped)
        {
            Destroy(g);
            isInit = false;
        }
    }

    void InstanceParticle(ParticleType type, Vector3 v, Transform pa = null)
    {
        if (type == ParticleType.EatHPCube)
        {
            offset = new Vector3(0, 1, 0);
        }
        else if (type == ParticleType.Boom)
        {
            offset = Vector3.zero;
        }
        else if (type == ParticleType.Failed)
        {
            offset = new Vector3(0, -1, 0);
        }
        else if (type == ParticleType.Jump)
        {
            offset = new Vector3(0, -0.5f, 0);
        }
        else if (type == ParticleType.Success)
        {
            offset = Vector3.up;
        }
        else if (type == ParticleType.PutCube)
        {
            offset = new Vector3(0, -1, 0);
        }

        else if (type == ParticleType.StarTrail)
        {
            offset = new Vector3(-1, 0, 0);
        }
        else
        {
            Debug.Log("无");
        }
        g = Instantiate(particles[(int)type], v + offset, particles[(int)type].transform.rotation);
        if (pa)
        {
            g.transform.SetParent(pa);
        }
        isInit = true;
    }
}
