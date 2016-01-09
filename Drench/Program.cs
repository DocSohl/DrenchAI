using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drench {
    class Program {
        static void Main(string[] args) {
            OpenTK.GameWindow window = new OpenTK.GameWindow(1280, 720);
            VisualDisplay display = new VisualDisplay(window);
            window.Run();
        }
    }
}
