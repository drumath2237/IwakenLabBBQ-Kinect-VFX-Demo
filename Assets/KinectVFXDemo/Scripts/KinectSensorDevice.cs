using System;
using Microsoft.Azure.Kinect.Sensor;
using UnityEngine;

namespace KinectVFXDemo
{
    public class KinectSensorDevice : DeviceBehaviour
    {
        public override int Width => _width;
        public override int Height => _height;
        public override bool IsReady => _isReady;

        private int _width, _height;
        private bool _isReady;

        private Device _kinect;

        private void Start()
        {
            _kinect = Device.Open();
        }

        public override void StartCamera()
        {
            _kinect.StartCameras(new DeviceConfiguration
            {
                ColorFormat = ImageFormat.ColorBGRA32,
                ColorResolution = ColorResolution.R720p,
                DepthMode = DepthMode.WFOV_2x2Binned,
                CameraFPS = FPS.FPS30
            });

            var depthResolution = _kinect.GetCalibration().DepthCameraCalibration;
            _width = depthResolution.ResolutionWidth;
            _height = depthResolution.ResolutionHeight;

            _isReady = true;
        }

        public override bool TryGetLatestSensorFrameData(out SensorFrameData frameData)
        {
            throw new System.NotImplementedException();
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}