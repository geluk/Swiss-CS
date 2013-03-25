using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Swiss_CS.Utils;

namespace Swiss_CS
{
	class Chunk
	{
		const float BLOCKW = 1f;

		private Vector3i location;
		public Vector3i Location { get { return location; } }

		private BufferObject vbo = new BufferObject();

		public byte[,,] blocks;

		public Chunk(Vector3i location, int x, int y, int z)
		{
			this.location = location;
			blocks = new byte[x,y,z];
		}
		public void Populate(byte[,,] blocks)
		{
			if (blocks.Length != this.blocks.Length) {
				return;
			}
			int x = blocks.GetLength(0);
			int y = blocks.GetLength(1);
			int z = blocks.GetLength(2);
			Console.WriteLine("x={0}, y={1}, z={2}", x, y, z);
		}
		public float[] genVertexArray()
		{
			float[] vertices = new float[blocks.Length * 4 * 8];

			int totalIndex = 0;
			for (int x = 0; x < blocks.GetLength(0); x++) {
				for (int y = 0; x < blocks.GetLength(0); y++) {
					for (int z = 0; x < blocks.GetLength(0); z++) {
						addVertex(ref vertices, new Vector4(x, y, z, 1), totalIndex);
						totalIndex++;
						addVertex(ref vertices, new Vector4(x + BLOCKW, y, z, 1), totalIndex);
						totalIndex++;
						addVertex(ref vertices, new Vector4(x + BLOCKW, y, z + BLOCKW, 1), totalIndex);
						totalIndex++;
						addVertex(ref vertices, new Vector4(x, y, z + BLOCKW, 1), totalIndex);
						totalIndex++;
						addVertex(ref vertices, new Vector4(x, y + BLOCKW, z, 1), totalIndex);
						totalIndex++;
						addVertex(ref vertices, new Vector4(x + BLOCKW, y + BLOCKW, z, 1), totalIndex);
						totalIndex++;
						addVertex(ref vertices, new Vector4(x + BLOCKW, y + BLOCKW, z + BLOCKW, 1), totalIndex);
						totalIndex++;
						addVertex(ref vertices, new Vector4(x, y + BLOCKW, z + BLOCKW, 1), totalIndex);
						totalIndex++;
					}
				}
			}
			return vertices;
		}
		private void addVertex(ref float[] vertices, Vector4 vertex, int position)
		{
			vertices[position] = vertex.X;
			vertices[position + 1] = vertex.Y;
			vertices[position + 2] = vertex.Z;
			vertices[position + 3] = vertex.W;
		}

		private float[] genColorArray()
		{
			return null;
		}

		public void GenVBOs()
		{

			float[] vertices = genVertexArray();

			float[] colors = genColorArray();

			//vbo.GenVBO(vertices, colors);

		}
	}
}
