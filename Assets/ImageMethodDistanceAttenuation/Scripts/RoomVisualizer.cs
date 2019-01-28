using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImageMethodDistanceAttenuation
{
    [RequireComponent(typeof(RoomDefinition))]
    public class RoomVisualizer : MonoBehaviour
    {
        private RoomDefinition room;

        private void Start()
        {
            room = GetComponent<RoomDefinition>();
        }

        private void OnDrawGizmos()
        {
            if (room == null)
            {
                room = GetComponent<RoomDefinition>();
            }

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(room.size / 2.0f, room.size);
        }
    }
}
