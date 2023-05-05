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

        bool IsRunning { get; }

        void StartCamera();
        void StopCamera();

        bool TryGetLatestSensorFrameData(out SensorFrameData frameData);
    }
}