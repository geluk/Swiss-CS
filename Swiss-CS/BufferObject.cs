// VBO details:
/*
 *  // Generate one vertex array and store its reference in the VaoId variable.
 * GL.GenVertexArrays(1, out VaoId);
 * GL.BindVertexArray(VaoId);
 * // Generate one buffer and store its reference in the VboId variable.
 * GL.GenBuffers(1, out VboId);
 * // Bind the previously bound vertex array to the newly created Vertex Buffer.
 * GL.BindBuffer(BufferTarget.ArrayBuffer, VboId);
 * // Store the data defined in the vertices array in the Vertex Buffer.
 * // -The data must be copied to the buffer bound to the ArrayBuffer target, which is the VBO that was just created.
 * // -Specify the buffer size, so the GPU knows how much space the buffer will take up (in this case, the length of the vertices array times the amount of bytes a float takes up (4)
 * // -The array of vertices to be stored in the GPU's memory
 * // -The memory will not be modified and will be used for drawing purposes
 * GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length *4), vertices, BufferUsageHint.StaticDraw);
 * // Define the attributes of the Vertex Buffer;
 * // -The buffer is stored at the first index
 * // -Each vertex is made up of four elements
 * // -The data type of the vertices is float
 * // -Do not normalize the values
 * GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 0, 0);
 * // Enable drawing of the vertex array with index 0
 * GL.EnableVertexAttribArray(0);
 * // Now do the same for the color array
 * GL.GenBuffers(1, out ColorBufferId);
 * GL.BindBuffer(BufferTarget.ArrayBuffer, ColorBufferId);
 * GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(colors.Length *4), colors, BufferUsageHint.StaticDraw);
 * GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 0, 0);
 * GL.EnableVertexAttribArray(1);
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL;

namespace Swiss_CS
{
	class BufferObject
	{
		private static bool[] UsedIds;
		private int vertexAttribId = -1;

		public BufferObject()
		{
			if (UsedIds == null) {
				int maxIds = 16;
				//GL.GetInteger(GetPName.MaxVertexAttribs, out maxIds);

				UsedIds = new bool[maxIds];
			}
			arrayObjectId = -1;
			bufferObjectId = -1;
		}

		internal int arrayObjectId { get; private set; }
		internal int bufferObjectId { get; private set; }

		internal BufferTarget BufferTarget { get; private set; }

		const int FloatSize = 4;

		/// <summary>
		/// Generates a buffer object from the specified parameters, which can be accessed trough this object.
		/// </summary>
		/// <param name="data">The data to be stored in the VBO</param>
		/// <param name="dataSize">The amount of vertices each data block contains</param>
		/// <param name="bufferTarget">Specifies the buffer type</param>
		/// <param name="bufferUsageHint">Specifies how the buffer should be used</param>
		/// <param name="genAO">Should an Array Object be generated for this Buffer Object?</param>
		public void Generate(float[] data, int dataSize, BufferTarget bufferTarget, BufferUsageHint bufferUsageHint, bool genAO)
		{
			this.BufferTarget = bufferTarget;

			// Set up some additional local variables:
			int i = 0;
			while (UsedIds[i]) {
				i++;
			}
			UsedIds[i] = true;
			// The vertex attribute Id
			vertexAttribId = i;

			// The amount of buffer objects that should be generated
			int amount = 1;

			int var;
			if (genAO) {
				GL.GenVertexArrays(amount, out var);
				arrayObjectId = var;
				GL.BindVertexArray(arrayObjectId);
			}

			GL.GenBuffers(amount, out var);
			bufferObjectId = var;
			GL.BindBuffer(bufferTarget, bufferObjectId);
			GL.BufferData(bufferTarget, (IntPtr)(data.Length * FloatSize), data, bufferUsageHint);
			GL.VertexAttribPointer(vertexAttribId, dataSize, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexAttribArray(vertexAttribId);
		}
		/// <summary>
		/// Binds this buffer to the buffertarget specified during its creation
		/// </summary>
		public void Bind()
		{
			GL.BindBuffer(this.BufferTarget, bufferObjectId);
		}

		/// <summary>
		/// Destroys this buffer.
		/// </summary>
		public void Destroy()
		{
			if (vertexAttribId != -1) {
				GL.DisableVertexAttribArray(vertexAttribId);
				GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

				int var = bufferObjectId;
				GL.DeleteBuffers(1, ref var);

				if (arrayObjectId != -1) {
					var = arrayObjectId;
					GL.BindVertexArray(0);
					GL.DeleteVertexArrays(1, ref var);
				}
			}
			vertexAttribId = -1;
			bufferObjectId = -1;
		}
	}
}
