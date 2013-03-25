using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL;

namespace Swiss_CS
{
	class ShaderProgram : List<Shader>
	{
		private bool created = false;
		private int programId = -1;

		internal void AddShader(Shader s)
		{
			base.Add(s);
		}
		internal void Create()
		{
			programId = GL.CreateProgram();
			// Attach the shaders to the shader program object
			for (int i = 0; i < base.Count; i++) {
				GL.AttachShader(programId, base[i].ShaderId);
			}
			// Link the shader program and start using it.
			GL.LinkProgram(programId);
			GL.UseProgram(programId);
		}
		internal void Destroy()
		{
			if (programId == -1) {
				return;
			}

			GL.UseProgram(0);

			for (int i = 0; i < base.Count; i++) {
				GL.DetachShader(programId, base[i].ShaderId);
			}
			for (int i = 0; i < base.Count; i++) {
				GL.DeleteShader(base[i].ShaderId);
			}

			GL.DeleteProgram(programId);
			programId = -1;
		}
	}
}
