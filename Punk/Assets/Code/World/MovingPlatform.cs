using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor;
using UnityEngine;

namespace Punk
{
    public class MovingPlatform : MonoBehaviour
    {
        public Transform platform;
        public GameObject[] waypoints;
        public int index = 0;

        public float speed = 1.5f;

        private void Update()
        {
            if (Vector2.Distance(waypoints[index].transform.position, transform.position) < 1f)
            {
                index++;
                if (index >= waypoints.Length)
                {
                    index = 0;
                }
            }
            transform.position = Vector2.MoveTowards(transform.position, waypoints[index].transform.position, Time.deltaTime * speed);
        }
    }
}
