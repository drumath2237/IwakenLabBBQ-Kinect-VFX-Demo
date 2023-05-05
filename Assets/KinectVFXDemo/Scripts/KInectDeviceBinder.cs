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
        private DeviceBehaviour device;

        [VFXPropertyBinding(nameof(System.Int32))]
        public ExposedProperty widthProperty;

        [VFXPropertyBinding(nameof(System.Int32))]
        public ExposedProperty heightProperty;

        [VFXPropertyBinding(nameof(GraphicsBuffer))]
        public ExposedProperty positionBufferProperty;

        [VFXPropertyBinding(nameof(GraphicsBuffer))]
        public ExposedProperty colorBufferProperty;


        public override bool IsValid(VisualEffect component)
        {
            return device;
        }

        public override void UpdateBinding(VisualEffect component)
        {
            if (!device.TryGetLatestSensorFrameData(out var frameData))
            {
                return;
            }

            var width = frameData.Width;
            var height = frameData.Height;

            var positionsBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, width * height,
                Marshal.SizeOf<Vector3>());
            positionsBuffer.SetData(frameData.Positions);

            var colorsBuffer =
                new GraphicsBuffer(GraphicsBuffer.Target.Structured, width * height, Marshal.SizeOf<Color>());
            colorsBuffer.SetData(frameData.Colors);


            component.SetInt(widthProperty, width);
            component.SetInt(heightProperty, height);
            component.SetGraphicsBuffer(positionBufferProperty, positionsBuffer);
            component.SetGraphicsBuffer(colorBufferProperty, colorsBuffer);
        }
    }
}