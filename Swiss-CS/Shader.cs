using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL;

namespace Swiss_CS
{
	class Shader : IDisposable
	{
		internal int ShaderId { get; private set; }

		public void Generate(string source, ShaderType shaderType)
		{
			// Create a vertex shader object and store its identifier in the VertexShaderId variable
			ShaderId = GL.CreateShader(shaderType);
			// Copy the source code to the specified vertex shader object
			GL.ShaderSource(ShaderId, source);
			// Compile the source code
			GL.CompileShader(ShaderId);
		}

		public void Dispose()
		{
			if (ShaderId != -1)
			{
				GL.DeleteShader(ShaderId);
				ShaderId = -1;
			}
		}
	}
}
