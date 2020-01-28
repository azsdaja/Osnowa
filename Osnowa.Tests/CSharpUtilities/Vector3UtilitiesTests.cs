namespace Osnowa.Tests.CSharpUtilities
{
	using FluentAssertions;
	using NUnit.Framework;
	using UnityEngine;

	[TestFixture]
	public class Vector3UtilitiesTests
	{
		static readonly object[] TestCases = 
			{
		new object[]{new Vector3(0,0,0), new Vector3(1,1,1), 0.00f, new Vector3(0,0,0) }, // beginning — should be at beginning
		new object[]{new Vector3(0,0,0), new Vector3(1,1,1),-5.00f, new Vector3(0,0,0) }, // beginning (clamped from negative)
		new object[]{new Vector3(0,0,0), new Vector3(1,1,1), 0.25f, new Vector3(0.5f,0.5f,0.5f) }, // quarter way — should be at half distance
		new object[]{new Vector3(0,0,0), new Vector3(1,1,1), 0.50f, new Vector3(1,1,1) }, // half way — should be at target
		new object[]{new Vector3(0,0,0), new Vector3(1,1,1), 0.75f, new Vector3(0.5f, 0.5f, 0.5f) }, // three quarters way — should be at half distance
		new object[]{new Vector3(0,0,0), new Vector3(1,1,1), 1.00f, new Vector3(0,0,0) }, // full way — should be at beginning
		new object[]{new Vector3(0,0,0), new Vector3(1,1,1), 5.00f, new Vector3(0,0,0) }, // full way (clamped from more than 1)
			};

		[TestCaseSource("TestCases")]
		public void LerpThereAndBack_ReturnsCorrectResult(Vector3 first, Vector3 second, float progress, Vector3 expectedResult)
		{
			Vector3 result = Vector3Utilities.LerpThereAndBack(first, second, progress);

			result.Should().Be(expectedResult);
		}
	}
}