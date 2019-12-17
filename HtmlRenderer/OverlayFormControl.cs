using System;
using System.Windows.Forms;

namespace RainbowMage.HtmlRenderer
{
    public class OverlayFormControl : OverlayForm
    {
        public OverlayFormControl(string overlayVersion, string overlayName, string url, int maxFrameRate = 30, object api = null)
          : base(overlayVersion, overlayName, url, maxFrameRate, api)
        {
        }

        protected override System.Windows.Forms.CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ClassStyle = cp.ClassStyle | CP_NOCLOSE_BUTTON;
                return cp;
            }
        }

        protected override void OnRenderFrame()
        {
          this.Invalidate();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
          // OnPaint will blit non-transparently, so skip background to avoid flashes.
        }

        protected override void OnPaint(PaintEventArgs e)
        {
           try 
           {
              if (surfaceBuffer.IsDisposed || this.terminated)
              {
                  return;
              }

              IntPtr handle = IntPtr.Zero;
              if (this.InvokeRequired)
              {
                  this.Invoke(new Action(() =>
                  {
                      handle = this.Handle;
                  }));
              }
              else
              {
                  handle = this.Handle;
              }

              var destContext = NativeMethods.GetDC(handle);
              NativeMethods.BitBlt(destContext, 0, 0, surfaceBuffer.Width, surfaceBuffer.Height, surfaceBuffer.DeviceContext, 0, 0, NativeMethods.SRCCOPY);
            }
            catch
            {
            }
        }
    }
}