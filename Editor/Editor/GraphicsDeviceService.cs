// =Realms Engine=
// =Realms Of the Mind=
// =Programmers=
// =Mute Lovestone=
// =GraphicsDeviceService.cs=
// = 11/3/2014 =
// =Editor=
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Graphics;
namespace Editor
{
    /// <summary>
    /// Helper class responsible for creating and managing the GraphicsDevice.
    /// All GraphicsDeviceControl instances share the same GraphicsDeviceService,
    /// so even though there can be many controls, there will only ever be a single
    /// underlying GraphicsDevice. This implements the standard IGraphicsDeviceService
    /// interface, which provides notification events for when the device is reset
    /// or disposed.
    /// </summary>
    class GraphicsDeviceService : IGraphicsDeviceService
    {
        #region Fields
        // Singleton device service instance.
        static GraphicsDeviceService singletonInstance;
        // Keep track of how many controls are sharing the singletonInstance.
        static int referenceCount;
        #endregion
        /// <summary>
        /// Constructor is private, because this is a singleton class:
        /// client controls should use the public AddRef method instead.
        /// </summary>
        GraphicsDeviceService(IntPtr windowHandle, int width, int height)
        {
            this.parameters = new PresentationParameters();
            
            this.parameters.BackBufferWidth = Math.Max(width, 1);
            this.parameters.BackBufferHeight = Math.Max(height, 1);
            this.parameters.BackBufferFormat = SurfaceFormat.Color;
            this.parameters.DepthStencilFormat = DepthFormat.Depth24;
            this.parameters.DeviceWindowHandle = windowHandle;
            this.parameters.PresentationInterval = PresentInterval.Immediate;
            this.parameters.IsFullScreen = false;
            
            this.graphicsDevice = new GraphicsDevice(GraphicsAdapter.DefaultAdapter,
                GraphicsProfile.Reach,
                this.parameters);
        }
        /// <summary>
        /// Gets a reference to the singleton instance.
        /// </summary>
        public static GraphicsDeviceService AddRef(IntPtr windowHandle,
            int width, int height)
        {
            // Increment the "how many controls sharing the device" reference count.
            if (Interlocked.Increment(ref referenceCount) == 1)
            { singletonInstance = new GraphicsDeviceService(windowHandle,
                width, height); }
            
            return singletonInstance;
        }
        /// <summary>
        /// Releases a reference to the singleton instance.
        /// </summary>
        public void Release(bool disposing)
        {
            // Decrement the "how many controls sharing the device" reference count.
            if (Interlocked.Decrement(ref referenceCount) == 0)
            {
                // If this is the last control to finish using the
                // device, we should dispose the singleton instance.
                if (disposing)
                {
                    if (this.DeviceDisposing != null)
                    { this.DeviceDisposing(this, EventArgs.Empty); }
                    
                    this.graphicsDevice.Dispose();
                }
                
                this.graphicsDevice = null;
            }
        }
        /// <summary>
        /// Resets the graphics device to whichever is bigger out of the specified
        /// resolution or its current size. This behavior means the device will
        /// demand-grow to the largest of all its GraphicsDeviceControl clients.
        /// </summary>
        public void ResetDevice(int width, int height)
        {
            if (this.DeviceResetting != null)
            { this.DeviceResetting(this, EventArgs.Empty); }
            
            this.parameters.BackBufferWidth = Math.Max(this.parameters.BackBufferWidth, width);
            this.parameters.BackBufferHeight = Math.Max(this.parameters.BackBufferHeight, height);
            
            this.graphicsDevice.Reset(this.parameters);
            
            if (this.DeviceReset != null)
            { this.DeviceReset(this, EventArgs.Empty); }
        }
        /// <summary>
        /// Gets the current graphics device.
        /// </summary>
        public GraphicsDevice GraphicsDevice
        {
            get { return this.graphicsDevice; }
        }
        GraphicsDevice graphicsDevice;
        // Store the current device settings.
        PresentationParameters parameters;
        // IGraphicsDeviceService events.
        public event EventHandler<EventArgs> DeviceCreated;
        public event EventHandler<EventArgs> DeviceDisposing;
        public event EventHandler<EventArgs> DeviceReset;
        public event EventHandler<EventArgs> DeviceResetting;
    }
}