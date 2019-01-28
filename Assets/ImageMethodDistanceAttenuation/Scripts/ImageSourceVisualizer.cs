using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ImageMethodDistanceAttenuation
{
    public class ImageSourceVisualizer : MonoBehaviour
    {
        public RoomDefinition room;
        public Transform source;
        public Transform listener;
        public GameObject imageSourcePrefab;
        public GameObject roomMeshPrefab;

        [HideInInspector]
        public bool showPath = false;

        private int reflectCount;
        private readonly List<Transform> imageSources = new List<Transform>();
        private readonly List<Transform> imageRooms = new List<Transform>();
        private Vector3[] imageSourcePositions;

        private void Update()
        {
            if (reflectCount == 0)
            {
                return;
            }

            ImageSourcePositionCalculator.GetImageSourcePositions(source.position, room, reflectCount,
                this.imageSourcePositions);
            for (var x = -reflectCount; x <= reflectCount; x++)
            {
                for (var y = -reflectCount; y <= reflectCount; y++)
                {
                    for (var z = -reflectCount; z <= reflectCount; z++)
                    {
                        var imageRoomIndex = (z + reflectCount) +
                                             (y + reflectCount) * (reflectCount * 2 + 1) +
                                             (x + reflectCount) * (int)Math.Pow((reflectCount * 2 + 1), 2);

                        if (x == 0 && y == 0 && z == 0)
                        {
                            imageRooms[imageRoomIndex].gameObject.SetActive(false);
                            imageSources[imageRoomIndex].gameObject.SetActive(false);
                            continue;
                        }

                        imageRooms[imageRoomIndex].position = new Vector3(room.size.x * x, room.size.y * y, room.size.z * z) + room.size / 2.0f;
                        imageRooms[imageRoomIndex].localScale = room.size;

                        imageSources[imageRoomIndex].position = imageSourcePositions[imageRoomIndex];

                        if (showPath)
                        {
                            Debug.DrawLine(imageSourcePositions[imageRoomIndex], listener.position, Color.cyan);
                        }
                    }
                }
            }
        }

        public void Reflect()
        {
            reflectCount++;

            var imageSourceCount = Math.Pow(reflectCount * 2 + 1, 3);
            var newImageSourceCount = imageSourceCount - imageSources.Count;
            for (var newImageSourceIndex = 0; newImageSourceIndex < newImageSourceCount; newImageSourceIndex++)
            {
                imageSources.Add(Instantiate(imageSourcePrefab, transform).transform);
                imageRooms.Add(Instantiate(roomMeshPrefab, transform).transform);
            }

            imageSourcePositions = new Vector3[(int) Math.Pow(reflectCount * 2 + 1, 3)];
        }
    }
}
