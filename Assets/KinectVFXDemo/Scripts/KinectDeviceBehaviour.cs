using UnityEngine;

namespace KinectVFXDemo
{
    public class KinectDeviceBehaviour : MonoBehaviour
    {
        private IDevice _kinect;

        private void Start()
        {
            _kinect = new KinectSensorDevice();
            _kinect.StartCamera();
        }

        public bool TryGetLatestSensorFrameData(out SensorFrameData frameData)
        {
            if (_kinect != null)
            {
                return _kinect.TryGetLatestSensorFrameData(out frameData);
            }

            frameData = new SensorFrameData();
            return false;
        }

        private void OnDestroy()
        {
            _kinect.Dispose();
        }
    }
}