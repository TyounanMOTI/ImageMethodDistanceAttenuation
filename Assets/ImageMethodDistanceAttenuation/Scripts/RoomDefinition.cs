using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImageMethodDistanceAttenuation
{
    public enum CubeFace
    {
        X,
        Y,
        Z,
        MinusX,
        MinusY,
        MinusZ,
    }

    public struct Plane
    {
        public Vector3 position;
        public Vector3 normal;
    }

    public class RoomDefinition : MonoBehaviour
    {
        public Vector3 size => transform.localScale;
        public float speedOfSound = 343.0f;
        public float uniformReflectionCoefficient = 1.0f;

        public Plane GetPlane(CubeFace face)
        {
            switch (face)
            {
                case CubeFace.X:
                    return new Plane()
                    {
                        normal = new Vector3(-1, 0, 0),
                        position = new Vector3(size.x, 0, 0),
                    };
                case CubeFace.MinusX:
                    return new Plane()
                    {
                        normal = new Vector3(1, 0, 0),
                        position = new Vector3(0, 0, 0),
                    };
                case CubeFace.Y:
                    return new Plane()
                    {
                        normal = new Vector3(0, -1, 0),
                        position = new Vector3(0, size.y, 0),
                    };
                case CubeFace.MinusY:
                    return new Plane()
                    {
                        normal = new Vector3(0, 1, 0),
                        position = new Vector3(0, 0, 0),
                    };
                case CubeFace.Z:
                    return new Plane()
                    {
                        normal = new Vector3(0, 0, -1),
                        position = new Vector3(0, 0, size.z),
                    };
                case CubeFace.MinusZ:
                    return new Plane()
                    {
                        normal = new Vector3(0, 0, 1),
                        position = new Vector3(0, 0, 0),
                    };
                default:
                    throw new Exception("Undefined CubeFace has been specified.");
            }
        }
    }
}
