using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

namespace KinectVFXDemo
{
    [VFXBinder("Custom/KinectDeviceBinder")]
    public class KinectDeviceBinder : VFXBinderBase

    {
        [SerializeField]
        private KinectDeviceBehaviour device;

        [VFXPropertyBinding("System.Int32")]
        public ExposedProperty widthProperty;

        [VFXPropertyBinding("System.Int32")]
        public ExposedProperty heightProperty;

        [VFXPropertyBinding("UnityEngine.GraphicsBuffer")]
        public ExposedProperty positionBufferProperty;

        [VFXPropertyBinding("UnityEngine.GraphicsBuffer")]
        public ExposedProperty colorBufferProperty;

        private GraphicsBuffer _positionsBuffer = null, _colorsBuffer = null;


        public override bool IsValid(VisualEffect component)
        {
            return device &&
                   widthProperty.ToString() != "" &&
                   heightProperty.ToString() != "" &&
                   positionBufferProperty.ToString() != "" &&
                   colorBufferProperty.ToString() != "";
        }

        public override void UpdateBinding(VisualEffect component)
        {
            if (!device.TryGetLatestSensorFrameData(out var frameData))
            {
                return;
            }

            var width = frameData.Width;
            var height = frameData.Height;

            _positionsBuffer ??= new GraphicsBuffer(
                GraphicsBuffer.Target.Structured,
                GraphicsBuffer.UsageFlags.None,
                width * height,
                Marshal.SizeOf(typeof(Vector3))
            );

            _positionsBuffer.SetData(frameData.Positions);

            _colorsBuffer ??= new GraphicsBuffer(
                GraphicsBuffer.Target.Structured,
                GraphicsBuffer.UsageFlags.None,
                width * height,
                Marshal.SizeOf(typeof(Color))
            );

            _colorsBuffer.SetData(frameData.Colors);

            component.SetInt(widthProperty, width);
            component.SetInt(heightProperty, height);
            component.SetGraphicsBuffer(positionBufferProperty, _positionsBuffer);
            component.SetGraphicsBuffer(colorBufferProperty, _colorsBuffer);
        }

        private void OnDestroy()
        {
            _positionsBuffer?.Dispose();
            _colorsBuffer?.Dispose();
        }
    }
}