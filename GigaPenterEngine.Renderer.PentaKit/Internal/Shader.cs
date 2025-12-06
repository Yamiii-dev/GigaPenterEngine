using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigaPenterEngine.Renderer.PentaKit.Internal
{
    internal class Shader : IDisposable
    {
        // Everything stored on the GPU's memory returns a handle, which we use to access said data.
        public int Handle;

        public Shader()
        {
            // Handles for our Vertex and Fragment Shader
            // Vertex shader handles placing vertices before they get rendered
            // Fragment shader handles how to turn those vertices into pixels
            int VertexShader;
            int FragmentShader;

            // Simple shader that takes the vertex position, applies the model, view and projection matrix, and forwards the UV coord to the fragment shader.
            // Model matrix handles placing the model somewhere in the game world
            // View matrix handles placing the model where the camera would be seeing it
            // Projection matrix handles turning the view into Normalized Device Coordinates (the GPU only renders vertexes that are in the coords -1 to 1)
            // UV coordinates tell the GPU where on the texture the vertex belongs
            string VertexShaderSource = @"#version 330 core

                                          layout(location = 0) in vec2 aPosition;
                                          layout(location = 1) in vec2 aTexCoord;

                                          out vec2 TexCoord;

                                          uniform mat4 model;
                                          uniform mat4 view;
                                          uniform mat4 projection;

                                          void main()
                                          {
                                              TexCoord = aTexCoord;
    
                                              gl_Position = vec4(aPosition, 0.0, 1.0) * model * view * projection;
                                          }";

            // Simple shader that takes the texture, color, and our current vertexes, and applies the texture * color to the current "fragment"
            string FragmentShaderSource = "#version 330 core\r\nout vec4 FragColor;\r\nin vec2 TexCoord;\r\n\r\nuniform vec4 Color;\r\nuniform sampler2D Texture0;\r\n\r\nvoid main()\r\n{\r\n    vec4 t1 = texture(Texture0, TexCoord);\r\n    FragColor = t1 * Color;\r\n}";

            // Create a place in the GPU's memory for the vertex shader, retrieve it's handle (the ID of the memory block) and send it
            VertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexShader, VertexShaderSource);

            // Creat a place in the GPU's memory for the fragment shader, retrieve it's handle (the ID of the memory block) and sent it
            FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, FragmentShaderSource);

            // Compile our vertex shader and retrieve result
            GL.CompileShader(VertexShader);
            GL.GetShader(VertexShader, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(VertexShader);
                Console.WriteLine(infoLog);
            }

            // Compile our fragment shader and retrieve the result
            GL.CompileShader(FragmentShader);
            GL.GetShader(FragmentShader, ShaderParameter.CompileStatus, out success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(FragmentShader);
                Console.WriteLine(infoLog);
            }

            // Create a place in the GPU's memory for the actual "binaries" of our shader
            Handle = GL.CreateProgram();

            // Put the compiled versions of our shaders into this new memory block
            GL.AttachShader(Handle, VertexShader);
            GL.AttachShader(Handle, FragmentShader);

            // Link the shaders into a singular program
            GL.LinkProgram(Handle);

            // Retrieve the result of our previous operation
            GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out success);
            if (success == 0)
            {
                string infoLog = GL.GetProgramInfoLog(Handle);
                Console.WriteLine(infoLog);
            }

            // Get rid of our singular shaders, as were only keeping the linked version in memory
            GL.DetachShader(Handle, VertexShader);
            GL.DetachShader(Handle, FragmentShader);
            GL.DeleteShader(VertexShader);
            GL.DeleteShader(FragmentShader);
        }

        // Called when we wanna use our shader
        public void Use()
        {
            GL.UseProgram(Handle);
        }

        // Get the location of an attribute (location is like an ID for the attribute so we know where to send data)
        public int GetAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(Handle, attribName);
        }

        // Set the value of an uniform variable
        public void SetInt(string name, int value)
        {
            int location = GL.GetUniformLocation(Handle, name);

            GL.Uniform1(location, value);
        }



        private bool disposedValue = false;

        // Get rid of the shader in the GPU's memory if we dispose our shader
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteProgram(Handle);
                disposedValue = true;
            }
        }
        ~Shader()
        {
            if (disposedValue == false)
            {
                Console.WriteLine("GPU Resource Leak! Did you forget to call Dispose()?");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Set the value of an uniform variable (Matrix4)
        internal void SetMatrix4(string name, Matrix4 value)
        {
            int location = GL.GetUniformLocation(Handle, name);
            GL.UniformMatrix4(location, true, ref value);
        }

        // Set the value of an uniform variable (Vector4)
        internal void SetVector4(string name, Vector4 value)
        {
            int location = GL.GetUniformLocation(Handle, name);
            GL.Uniform4(location, value.X, value.Y, value.Z, value.W);
        }
    }
}
