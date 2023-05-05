using System;
using UnityEngine;

namespace KinectVFXDemo
{
    public struct SensorFrameData
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public Vector3[] Positions { get; set; }
        public Color[] Colors { get; set; }
    }

    public interface IDevice : IDisposable
    {
        /// <summary>
        /// Sensor Width
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Sensor Height
        /// </summary>
        int Height { get; }

        bool IsReady { get; }

        void StartCamera();

        bool TryGetLatestSensorFrameData(out SensorFrameData frameData);
    }

    public abstract class DeviceBehaviour : MonoBehaviour, IDevice
    {
        public abstract int Width { get; }
        public abstract int Height { get; }
        public abstract bool IsReady { get; }

        public abstract void StartCamera();

        public abstract bool TryGetLatestSensorFrameData(out SensorFrameData frameData);

        public abstract void Dispose();
    }
}