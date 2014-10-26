// =Realms Engine=
// =Realms Of the Mind=
// =Programmers=
// =Mute Lovestone=
// =GraphicsDeviceControl.cs=
// = 10/26/2014 =
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
    // System.Drawing and the XNA Framework both define Color and Rectangle
    // types. To avoid conflicts, we specify exactly which ones to use.
    using Color = System.Drawing.Color;
    using Rectangle = Microsoft.Xna.Framework.Rectangle;
    /// <summary>
    /// Custom control uses the XNA Framework GraphicsDevice to render onto
    /// a Windows Form. Derived classes can override the Initialize and Draw
    /// methods to add their own drawing code.
    /// </summary>
    abstract public class GraphicsDeviceControl : Control
    {
        #region Fields
        // However many GraphicsDeviceControl instances you have, they all share
        // the same underlying GraphicsDevice, managed by this helper service.
        GraphicsDeviceService graphicsDeviceService;
        #endregion
        #region Properties
        /// <summary>
        /// Gets a GraphicsDevice that can be used to draw onto this control.
        /// </summary>
        public GraphicsDevice GraphicsDevice
        {
            get { return this.graphicsDeviceService.GraphicsDevice; }
        }
        /// <summary>
        /// Gets an IServiceProvider containing our IGraphicsDeviceService.
        /// This can be used with components such as the ContentManager,
        /// which use this service to look up the GraphicsDevice.
        /// </summary>
        public ServiceContainer Services
        {
            get { return this.services; }
        }
        ServiceContainer services = new ServiceContainer();
        #endregion
        #region Initialization
        /// <summary>
        /// Initializes the control.
        /// </summary>
        protected override void OnCreateControl()
        {
            // Don't initialize the graphics device if we are running in the designer.
            if (!this.DesignMode)
            {
                this.graphicsDeviceService = GraphicsDeviceService.AddRef(this.Handle,
                    this.ClientSize.Width,
                    this.ClientSize.Height);
                
                // Register the service, so components like ContentManager can find it.
                this.services.AddService<IGraphicsDeviceService>(this.graphicsDeviceService);
                
                // Give derived classes a chance to initialize themselves.
                this.Initialize();
            }
            
            base.OnCreateControl();
        }
        /// <summary>
        /// Disposes the control.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (this.graphicsDeviceService != null)
            {
                this.graphicsDeviceService.Release(disposing);
                this.graphicsDeviceService = null;
            }
            
            base.Dispose(disposing);
        }
        #endregion
        #region Paint
        /// <summary>
        /// Redraws the control in response to a WinForms paint message.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            string beginDrawError = this.BeginDraw();
            
            if (string.IsNullOrEmpty(beginDrawError))
            {
                // Draw the control using the GraphicsDevice.
                this.Draw();
                this.EndDraw();
            }
            else
            { this.PaintUsingSystemDrawing(e.Graphics, beginDrawError); }
        }
        /// <summary>
        /// Attempts to begin drawing the control. Returns an error message string
        /// if this was not possible, which can happen if the graphics device is
        /// lost, or if we are running inside the Form designer.
        /// </summary>
        string BeginDraw()
        {
            // If we have no graphics device, we must be running in the designer.
            if (this.graphicsDeviceService == null)
            { return string.Format("{0}\n\n{1}", this.Text, this.GetType()); }
            
            // Make sure the graphics device is big enough, and is not lost.
            string deviceResetError = this.HandleDeviceReset();
            
            if (!string.IsNullOrEmpty(deviceResetError))
            { return deviceResetError; }

            // Many GraphicsDeviceControl instances can be sharing the same
            // GraphicsDevice. The device backbuffer will be resized to fit the
            // largest of these controls. But what if we are currently drawing
            // a smaller control? To avoid unwanted stretching, we set the
            // viewport to only use the top left portion of the full backbuffer.
            Viewport viewport = new Viewport();
            
            viewport.X = 0;
            viewport.Y = 0;
            
            viewport.Width = this.ClientSize.Width;
            viewport.Height = this.ClientSize.Height;

            viewport.MinDepth = 0;
            viewport.MaxDepth = 1;

            this.GraphicsDevice.Viewport = viewport;
            
            return null;
        }
        /// <summary>
        /// Ends drawing the control. This is called after derived classes
        /// have finished their Draw method, and is responsible for presenting
        /// the finished image onto the screen, using the appropriate WinForms
        /// control handle to make sure it shows up in the right place.
        /// </summary>
        void EndDraw()
        {
            try
            {
                Rectangle sourceRectangle = new Rectangle(0, 0, this.ClientSize.Width,
                    this.ClientSize.Height);
                
                this.GraphicsDevice.Present(sourceRectangle, null, this.Handle);
            }
            catch
            { }
        }
        /// <summary>
        /// Helper used by BeginDraw. This checks the graphics device status,
        /// making sure it is big enough for drawing the current control, and
        /// that the device is not lost. Returns an error string if the device
        /// could not be reset.
        /// </summary>
        string HandleDeviceReset()
        {
            bool deviceNeedsReset = false;
            
            switch (this.GraphicsDevice.GraphicsDeviceStatus)
            {
                case GraphicsDeviceStatus.Lost: return "Graphics device lost";
                    
                case GraphicsDeviceStatus.NotReset: deviceNeedsReset = true; break;
                default:
                    // If the device state is ok, check whether it is big enough.
                    PresentationParameters pp = this.GraphicsDevice.PresentationParameters;
                    
                    deviceNeedsReset = (this.ClientSize.Width > pp.BackBufferWidth) ||
                                       (this.ClientSize.Height > pp.BackBufferHeight);
                    break;
            }
            
            // Do we need to reset the device?
            if (deviceNeedsReset)
            {
                try
                { this.graphicsDeviceService.ResetDevice(this.ClientSize.Width,
                    this.ClientSize.Height); }
                catch (Exception e)
                { return string.Format("Graphics device reset failed\n\n{0}", e); }
            }
            
            return null;
        }
        /// <summary>
        /// If we do not have a valid graphics device (for instance if the device
        /// is lost, or if we are running inside the Form designer), we must use
        /// regular System.Drawing method to display a status message.
        /// </summary>
        protected virtual void PaintUsingSystemDrawing(Graphics graphics, string text)
        {
            graphics.Clear(Color.CornflowerBlue);
            
            using (Brush brush = new SolidBrush(Color.Black))
            {
                using (StringFormat format = new StringFormat())
                {
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;
                    
                    graphics.DrawString(text, this.Font, brush, this.ClientRectangle, format);
                }
            }
        }
        /// <summary>
        /// Ignores WinForms paint-background messages. The default implementation
        /// would clear the control to the current background color, causing
        /// flickering when our OnPaint implementation then immediately draws some
        /// other color over the top using the XNA Framework GraphicsDevice.
        /// </summary>
        protected override void OnPaintBackground(PaintEventArgs pevent)
        { }
        #endregion
        #region Abstract Methods
        /// <summary>
        /// Derived classes override this to initialize their drawing code.
        /// </summary>
        protected abstract void Initialize();
        /// <summary>
        /// Derived classes override this to draw themselves using the GraphicsDevice.
        /// </summary>
        protected abstract void Draw();
        #endregion
    }
}