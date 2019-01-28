using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImageMethodDistanceAttenuation
{
    public class SingleAxisImageSourceVisualizer : MonoBehaviour
    {
        public RoomDefinition room;
        public Transform source;
        public GameObject imageSourcePrefab;
        public GameObject roomMeshPrefab;

        private int reflectCount;
        private CubeFace currentCubeFace;
        private readonly List<Transform> imageSources = new List<Transform>();
        private readonly List<Transform> imageRooms = new List<Transform>();

        private void Update()
        {
            var previousImageSourcePosition = source.position;
            for (var reflect = 0; reflect < reflectCount; reflect++)
            {
                var imageSource = imageSources[reflect];

                var realPlane = room.GetPlane(currentCubeFace);
                var imagePlane = new Plane()
                {
                    position = realPlane.position * (reflect + 1),
                    normal = realPlane.normal,
                };
                imageSource.position = ImageSourcePositionCalculator.GetImageSourcePosition(previousImageSourcePosition, imagePlane);
                previousImageSourcePosition = imageSource.position;

                var imageRoom = imageRooms[reflect];
                imageRoom.position = realPlane.position * (reflect + 1) + room.size / 2.0f;
                imageRoom.transform.localScale = room.size;
            }
        }

        public void Reflect(CubeFace face)
        {
            if (currentCubeFace != face)
            {
                currentCubeFace = face;
                reflectCount = 0;
                imageSources.Clear();
                imageRooms.Clear();
                for (var i = 0; i < transform.childCount; i++)
                {
                    Destroy(transform.GetChild(i).gameObject);
                }
            }

            imageSources.Add(Instantiate(imageSourcePrefab, transform).transform);
            imageRooms.Add(Instantiate(roomMeshPrefab, transform).transform);
            reflectCount++;
        }
    }
}
