using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Piec.Tests.RNG
{
	public class NoiseTests
	{
		[Test]
		public void Noise2D_Test()
		{
			var noise = new Noise2D(333);
			float [,] values = new float[100,100];


			for (int i = 0; i < 100; i++)
			{
				for (int j = 0; j < 100; j++)
				{
					values[i, j] = noise.Noise(i, j);
				}
			}
			IDictionary<float, int> widmo = new SortedDictionary<float, int>();

			for (int i = 0; i < 100; i++)
			{
				for (int j = 0; j < 100; j++)
				{
					float val = values[i, j];
					string value = val < 0.2f ? " " : val < 0.4f ? "." : val < 0.6f ? ":" : val < 0.8f ? "|" : "#";
					//Console.Write(value);


					float przedzial = (float)Math.Ceiling(val * 10) / 10f;
					if (!widmo.ContainsKey(przedzial)) widmo[przedzial] = 1;
					else ++widmo[przedzial];
				}
				//Console.Write("\r\n");
			}

			foreach (KeyValuePair<float, int> keyValuePair in widmo)
			{
				Console.WriteLine(keyValuePair.Key + ": " + keyValuePair.Value);
			}
		}

		[Test]
		public void PerlinTest()
		{
			float [,] values = new float[100,100];

			IDictionary<float, int> widmo = new SortedDictionary<float, int>();

			for (int i = 0; i < 100; i++)
			{
				for (int j = 0; j < 100; j++)
				{
					values[i, j] = Perlin.Noise(i, j, 3);
				}
			}

			for (int i = 0; i < 100; i++)
			{
				for (int j = 0; j < 100; j++)
				{
					float val = values[i, j];
					string value = val < 0.2f ? " " : val < 0.4f ? "." : val < 0.6f ? ":" : val < 0.8f ? "|" : "#";
					//Console.Write(value);

					float przedzial = (float) Math.Ceiling(val*10)/10f;
					if (!widmo.ContainsKey(przedzial)) widmo[przedzial] = 1;
					else ++widmo[przedzial];
				}
			}

			foreach (KeyValuePair<float, int> keyValuePair in widmo)
			{
				Console.WriteLine(keyValuePair.Key + ": " + keyValuePair.Value);
			}
		}
	}
}