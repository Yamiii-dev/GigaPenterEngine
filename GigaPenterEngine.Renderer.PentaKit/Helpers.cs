using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigaPenterEngine.Renderer.PentaKit
{
    internal static class Helpers
    {
        public static Vector2 ConvertToTK(GigaPenterEngine.Core.Vector2 input)
        {
            return new Vector2(input.X, input.Y);
        }

        public static Vector3 ConvertToTK(GigaPenterEngine.Core.Vector3 input)
        {
            return new Vector3(input.X, input.Y, input.Z);
        }
    }
}
