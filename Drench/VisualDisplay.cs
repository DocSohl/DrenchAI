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
        Square[,] board;
        Color[] colors;
        float GridSize;
        float GridPosX;
        float GridPosY;
        float ControlSize;
        float ControlPosX;
        float ControlPosY;


        public VisualDisplay(GameWindow window) {
            this.window = window;

            window.UpdateFrame += window_UpdateFrame;
            window.RenderFrame += window_RenderFrame;
            window.Resize += window_Resize;
            window.KeyPress += window_KeyPress;
            window.Closed += window_Closed;
            window.Load += window_Load;
            window.MouseUp += window_MouseUp;

            board = new Square[14, 14];
            colors = new Color[6];
            GridSize = window.Height / 15f;
            GridPosX = window.Width / 20f;
            GridPosY = window.Height / 20f;
            ControlSize = window.Height / 10f;
            ControlPosX = window.Width * (0.7f);
            ControlPosY = window.Height * (0.2f);
            setupBoard();
        }

        void setupBoard() {
            colors[0] = Color.Green;
            colors[1] = Color.Pink;
            colors[2] = Color.Purple;
            colors[3] = Color.SpringGreen;
            colors[4] = Color.Red;
            colors[5] = Color.Yellow;
            Random rnd = new Random();
            for (int x = 0; x < 14; ++x) {
                for (int y = 0; y < 14; ++y) {
                    Square s = new Square();
                    s.c = colors[rnd.Next(0,6)];
                    s.x = x;
                    s.y = y;
                    s.xPos = x * GridSize + GridPosX;
                    s.yPos = y * GridSize + GridPosY;
                    board[x, y] = s;
                }
            }
            for (int x = 0; x < 14; ++x) {
                for (int y = 0; y < 14; ++y) {
                    Square s = board[x, y];
                    if (x > 0) s.left = board[x - 1, y];
                    if (x < 13) s.right = board[x + 1, y];
                    if (y > 0) s.up = board[x, y - 1];
                    if (y < 13) s.down = board[x, y + 1];
                }
            }
        }

        class Square {
            public int x, y;
            public float xPos, yPos;
            public Square up, down, left, right;
            public Color c;

            public Square() {
                c = Color.Black;
            }
        }

        void drench(int index) {
            Console.WriteLine(colors[index].Name);

        }


        void DrawCanvas() {
            for (int x = 0; x < 14; ++x) {
                for (int y = 0; y < 14; ++y) {
                    Square s = board[x,y];
                    GL.Color3(s.c);
                    drawRect(s.xPos, s.yPos, GridSize - 1, GridSize - 1);
                }
            }
        }

        void DrawControls() {
            GL.Color3(colors[0]);
            drawCircle(ControlPosX + 0 * ControlSize, ControlPosY + 0 * ControlSize, ControlSize / 2, 36);
            GL.Color3(colors[1]);
            drawCircle(ControlPosX + 2 * ControlSize, ControlPosY + 0 * ControlSize, ControlSize / 2, 36);
            GL.Color3(colors[2]);
            drawCircle(ControlPosX + 4 * ControlSize, ControlPosY + 0 * ControlSize, ControlSize / 2, 36);
            GL.Color3(colors[3]);
            drawCircle(ControlPosX + 0 * ControlSize, ControlPosY + 2 * ControlSize, ControlSize / 2, 36);
            GL.Color3(colors[4]);
            drawCircle(ControlPosX + 2 * ControlSize, ControlPosY + 2 * ControlSize, ControlSize / 2, 36);
            GL.Color3(colors[5]);
            drawCircle(ControlPosX + 4 * ControlSize, ControlPosY + 2 * ControlSize, ControlSize / 2, 36);
        }

        public static void drawCircle(float x, float y, float radius, int segments) {
            GL.Begin(PrimitiveType.Polygon);

            for (int i = 0; i < segments; i++) {
                float theta = (2.0f * (float)Math.PI * (float)i) / (float)segments;
                float xx = radius * (float)Math.Cos(theta);
                float yy = radius * (float)Math.Sin(theta);
                GL.Vertex2(x + xx, y + yy);
            }

            GL.End();
        }

        public static void drawRect(float x, float y, float width, float height) {
            GL.Begin(PrimitiveType.Polygon);
            GL.Vertex2(x, y);
            GL.Vertex2(x + width, y);
            GL.Vertex2(x + width, y + height);
            GL.Vertex2(x, y + height);
            GL.Vertex2(x, y);
            GL.End();
        }

        bool calcIntersect(float x, float y, float tx, float ty, float radius) {
            float xdist = tx - x;
            float ydist = ty - y;
            float xdist2 = xdist * xdist;
            float ydist2 = ydist * ydist;
            float rad2 = radius * radius;
            if (xdist2 + ydist2 < rad2) return true;
            return false;
        }

        void window_MouseUp(object sender, MouseButtonEventArgs e) {
            float r = ControlSize / 2;
            int index = -1;
            if (calcIntersect(e.X, e.Y, ControlPosX + 0 * ControlSize, ControlPosY + 0 * ControlSize, r)) index = 0;
            if (calcIntersect(e.X, e.Y, ControlPosX + 2 * ControlSize, ControlPosY + 0 * ControlSize, r)) index = 1;
            if (calcIntersect(e.X, e.Y, ControlPosX + 4 * ControlSize, ControlPosY + 0 * ControlSize, r)) index = 2;
            if (calcIntersect(e.X, e.Y, ControlPosX + 0 * ControlSize, ControlPosY + 2 * ControlSize, r)) index = 3;
            if (calcIntersect(e.X, e.Y, ControlPosX + 2 * ControlSize, ControlPosY + 2 * ControlSize, r)) index = 4;
            if (calcIntersect(e.X, e.Y, ControlPosX + 4 * ControlSize, ControlPosY + 2 * ControlSize, r)) index = 5;

            if (index != -1) {
                drench(index);
            }
            else {
                Console.WriteLine("None");
            }
        }

        void window_RenderFrame(object sender, FrameEventArgs e) {
            GL.ClearColor(Color.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            DrawCanvas();
            DrawControls();
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
            if (e.KeyChar == 'q')
                window.Exit();
            if (e.KeyChar == 'u')
                drench(0);
            if (e.KeyChar == 'i')
                drench(1);
            if (e.KeyChar == 'o')
                drench(2);
            if (e.KeyChar == 'j')
                drench(3);
            if (e.KeyChar == 'k')
                drench(4);
            if (e.KeyChar == 'l')
                drench(5);
        }

        void window_Closed(object sender, EventArgs e) {
            Environment.Exit(0);
        }
    }
}
