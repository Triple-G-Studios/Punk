using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Punk
{
    public class VertMovingPlatform : MonoBehaviour
    {
        public Transform platform;
        public GameObject[] waypoints;
        public int index = 0;

        public float speed = 1.5f;

        private void Update()
        {
            if (waypoints[index].transform.position == transform.position)
            {
                index++;
                if (index >= waypoints.Length)
                {
                    index = 0;
                }
            }
            transform.position = waypoints[index].transform.position;
        }
    }
}
