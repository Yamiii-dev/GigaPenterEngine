using GigaPenterEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigaPenterEngine.Renderer.PentaKit
{
    // As the position and rotation are already stored in the Transform component, we only need to store the scale of our view.
    public class Camera : Component
    {
        public float scale = 1f;
    }
}
