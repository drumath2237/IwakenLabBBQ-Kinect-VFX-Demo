using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Kinect.Sensor;
using UnityEngine;

namespace KinectVFXDemo
{
    public class KinectSensorDevice : IDevice
    {
        public int Width { get; private set; }

        public int Height { get; private set; }

        public bool IsRunning { get; private set; }

        private Device _kinect;

        private Color[] _colorBuffer = null;
        private Vector3[] _positionBuffer = null;

        public void StartCamera()
        {
            _kinect = Device.Open();
            _kinect.StartCameras(new DeviceConfiguration
            {
                ColorFormat = ImageFormat.ColorBGRA32,
                ColorResolution = ColorResolution.R720p,
                DepthMode = DepthMode.WFOV_2x2Binned,
                CameraFPS = FPS.FPS30
            });

            var depthResolution = _kinect.GetCalibration().DepthCameraCalibration;
            Width = depthResolution.ResolutionWidth;
            Height = depthResolution.ResolutionHeight;

            _ = Task.Run(() =>
            {
                while (IsRunning)
                {
                    ObtainFrameData();
                }
            });

            IsRunning = true;
        }

        public void StopCamera()
        {
            IsRunning = false;
            _kinect?.Dispose();
            _kinect = null;
        }

        private void ObtainFrameData()
        {
            using var kinectTransformation = _kinect.GetCalibration().CreateTransformation();

            using var capture = _kinect.GetCapture();

            if (capture?.Depth == null || capture.Color == null)
            {
                return;
            }

            using var mappedColorImage = kinectTransformation.ColorImageToDepthCamera(capture);
            _colorBuffer = mappedColorImage
                .GetPixels<BGRA>()
                .ToArray()
                .Select(pixel => new Color(pixel.R / 256f, pixel.G / 256f, pixel.B / 256f))
                .ToArray();

            Width = mappedColorImage.WidthPixels;
            Height = mappedColorImage.HeightPixels;

            using var depthImage = kinectTransformation.DepthImageToPointCloud(capture.Depth);
            _positionBuffer = depthImage
                .GetPixels<Short3>()
                .ToArray()
                .Select(short3 => new Vector3(short3.X / 1000f, short3.Y / 1000f, short3.Z / 1000f))
                .ToArray();
        }

        public bool TryGetLatestSensorFrameData(out SensorFrameData frameData)
        {
            if (!IsRunning)
            {
                frameData = new SensorFrameData();
                return false;
            }

            if (_colorBuffer == null || _colorBuffer.Length == 0)
            {
                frameData = new SensorFrameData();
                return false;
            }

            if (_positionBuffer == null || _positionBuffer.Length == 0)
            {
                frameData = new SensorFrameData();
                return false;
            }

            frameData = new SensorFrameData
            {
                Width = Width,
                Height = Height,
                Colors = _colorBuffer,
                Positions = _positionBuffer
            };

            return true;
        }

        public void Dispose()
        {
            StopCamera();
        }
    }
}