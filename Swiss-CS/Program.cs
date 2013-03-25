// Released to the public domain. Use, modify and relicense at will.

using System;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using OpenTK.Input;

using System.Threading;
using Swiss_CS.Utils;

namespace Swiss_CS
{
	class Game : GameWindow
	{
		private BufferObject vbo = new BufferObject();
		private BufferObject cbo = new BufferObject();
		private ShaderProgram sp = new ShaderProgram();

		string VertexShader = 
		@"#version 400
		layout(location=0) in vec4 in_Position;
		layout(location=1) in vec4 in_Color;
		out vec4 ex_Color;
 
		void main(void)
		{
		   gl_Position = in_Position;
		   ex_Color = in_Color;
		}";

		string FragmentShader = 
		@"#version 400
		in vec4 ex_Color;
		out vec4 out_Color;
 
		void main(void)
		{
		    out_Color = ex_Color;
		}";

		int VertexShaderId, FragmentShaderId, ProgramId;

		public Game()
			: base(800, 600, GraphicsMode.Default, "OpenTK Quick Start Sample")
		{
			var ver = GL.GetString(StringName.Version);
			Console.WriteLine(ver);
			VSync = VSyncMode.On;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			CreateShaders();
			CreateVBO();

			GL.ClearColor(0.1f, 0.1f, 0.1f, 0.9f);
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);

			DestroyShaders();
			DestroyVBO();
		}

		private void CreateVBO()
		{
			float[] vertices = {
				-0.8f, -0.8f, 0.0f, 1.0f,
				 0.0f,  0.8f, 0.0f, 1.0f,
				 0.8f, -0.8f, 0.0f, 1.0f
			};
 
			float[] colors = {
				1.0f, 0.0f, 0.0f, 1.0f,
				0.0f, 1.0f, 0.0f, 1.0f,
				0.0f, 0.0f, 1.0f, 1.0f
			};

			vbo.Generate(vertices, 4, BufferTarget.ArrayBuffer, BufferUsageHint.StaticDraw, true);
			cbo.Generate(colors, 4, BufferTarget.ArrayBuffer, BufferUsageHint.StaticDraw, false);
		}

		void DestroyVBO()
		{
			vbo.Destroy();
			cbo.Destroy();
		}

		void CreateShaders()
		{
			var vertSh = new Shader();
			vertSh.Generate(VertexShader, ShaderType.VertexShader);
			var fragSh = new Shader();
			fragSh.Generate(FragmentShader, ShaderType.FragmentShader);
			sp.Add(vertSh);
			sp.Add(fragSh);

			sp.Create();
		}

		void DestroyShaders()
		{
			sp.Destroy();
		}

		/// <summary>
		/// Called when your window is resized. Set your viewport here. It is also
		/// a good place to set up your projection matrix (which probably changes
		/// along when the aspect ratio of your window).
		/// </summary>
		/// <param name="e">Not used.</param>
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);

			Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, Width / (float)Height, 1.0f, 64.0f);
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadMatrix(ref projection);
		}

		/// <param name="e">Contains timing information.</param>
		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			base.OnUpdateFrame(e);

			rot += 1;
			if (Keyboard[Key.Escape])
				Exit();
		}

		float rot = 0;

		Matrix4 modelview = Matrix4.LookAt(Vector3.Zero, Vector3.UnitZ, Vector3.UnitY);

		/// <param name="e">Contains timing information.</param>
		protected override void OnRenderFrame(FrameEventArgs e)
		{
			base.OnRenderFrame(e);

			

			// Clear the screen
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadMatrix(ref modelview);

			GL.DrawArrays(BeginMode.Triangles, 0, 3);


			SwapBuffers();
		}

		[STAThread]
		static void Main()
		{
			using (Game game = new Game()) {
				game.Run(60.0);
			}
		}
	}
}