using System;
using UnityEngine;

namespace TypeA.GL01.ThirdPerson
{

    [RequireComponent(typeof(Animator))]

    public class TPCControl : MonoBehaviour
    {

        public float animSpeed = 1.0f;
        public float forwardSpeed = 2.8f;
        public float backwardSpeed = 0.72f;
        public float rotateSpeed = 2.7f;

        private Vector3 velocity;

        private Animator anim;


        void Start()
        {
            anim = GetComponent<Animator>();
        }


        void FixedUpdate()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            anim.SetFloat("Speed", v);
            anim.SetFloat("Direction", h);
            anim.speed = animSpeed;

            velocity = new Vector3(0, 0, v);
            velocity = transform.TransformDirection(velocity);

            if (v > 0.1)
            {
                velocity *= forwardSpeed;
            }
            else if (v < -0.1)
            {
                velocity *= backwardSpeed;
            }

            transform.localPosition += velocity * Time.fixedDeltaTime;
            transform.Rotate(0, h * rotateSpeed, 0);
        }
    }
}
