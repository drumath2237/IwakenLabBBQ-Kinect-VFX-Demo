using System;
using UnityEngine;

namespace KinectVFXDemo
{
    public class SensorTest : MonoBehaviour
    {
        [SerializeField]
        private KinectDeviceBehaviour kinect;

        private void Update()
        {
            var success = kinect.TryGetLatestSensorFrameData(out var frameData);
            Debug.Log(success);
        }
    }
}