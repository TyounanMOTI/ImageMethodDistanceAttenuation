using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImageMethodDistanceAttenuation
{
    public static class ImageSourcePositionCalculator
    {
        public static Vector3 GetImageSourcePosition(Vector3 sourcePosition, Plane plane)
        {
            return sourcePosition - 2.0f * GetPointToPlaneDistance(sourcePosition, plane) * plane.normal;
        }

        public static void GetImageSourcePositions(Vector3 sourcePosition, RoomDefinition room, int numReflection,
            Vector3[] positionBuffer)
        {
            // validate position buffer length
            if (positionBuffer.Length < GetImageSourceCount(numReflection))
            {
                return;
            }

            var imageSourcePosition = new Vector3();
            for (var x = -numReflection; x <= numReflection; x++)
            {
                imageSourcePosition.x = x * room.size.x + ((x + 1) & 1) * sourcePosition.x +
                                        (x & 1) * (room.size.x - sourcePosition.x);
                for (var y = -numReflection; y <= numReflection; y++)
                {
                    imageSourcePosition.y = y * room.size.y + ((y + 1) & 1) * sourcePosition.y +
                                            (y & 1) * (room.size.y - sourcePosition.y);
                    for (var z = -numReflection; z <= numReflection; z++)
                    {
                        imageSourcePosition.z = z * room.size.z + ((z + 1) & 1) * sourcePosition.z +
                                                (z & 1) * (room.size.z - sourcePosition.z);
                       var imageSourceIndex = (z + numReflection) +
                                               (y + numReflection) * (numReflection * 2 + 1) +
                                               (x + numReflection) * (int) Math.Pow((numReflection * 2 + 1), 2);
                        positionBuffer[imageSourceIndex] = imageSourcePosition;
                    }
                }
            }
        }

        private static float GetPointToPlaneDistance(Vector3 point, Plane plane)
        {
            return Mathf.Abs(Vector3.Dot(point - plane.position, plane.normal));
        }

        public static int GetImageSourceCount(int numReflection)
        {
            return (int)Math.Pow(numReflection * 2 + 1, 3);
        }
    }
}
