using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drench {
    class VisualDisplay {

        GameWindow window;


        public VisualDisplay(GameWindow window) {
            this.window = window;

            window.UpdateFrame += window_UpdateFrame;
            window.RenderFrame += window_RenderFrame;
            window.Resize += window_Resize;
            window.KeyPress += window_KeyPress;
            window.Closed += window_Closed;
            window.Load += window_Load;
        }

        void window_RenderFrame(object sender, FrameEventArgs e) {
            GL.ClearColor(Color.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            window.SwapBuffers();
        }

        private void window_Load(object sender, EventArgs e) {
            window.WindowBorder = WindowBorder.Hidden;
            window.WindowState = WindowState.Fullscreen;
            GL.ClearColor(Color.Black);
        }

        void window_UpdateFrame(object sender, FrameEventArgs e) {

        }

        void window_Resize(object sender, EventArgs e) {
            // Set orthographic rendering (useful when you want 2D)
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(window.ClientRectangle.Left, window.ClientRectangle.Right,
                window.ClientRectangle.Bottom, window.ClientRectangle.Top, -1.0, 1.0);
            GL.Viewport(window.ClientRectangle.Size);
        }

        void window_KeyPress(object sender, OpenTK.KeyPressEventArgs e) {
            window.Exit();
        }

        void window_Closed(object sender, EventArgs e) {
            Environment.Exit(0);
        }
    }
}
