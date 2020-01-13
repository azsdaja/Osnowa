using FluentAssertions;
using NUnit.Framework;
using UnityEngine;

namespace Piec.Tests.CSharpUtilities
{
	using UnityUtilities;

	[TestFixture]
	public class BoundsIntUtilitiesTests
	{
		private static readonly object[] With_TestCases =
{
			new object[] { new BoundsInt(0, 0, 0, 2, 2, 2), new Vector3Int(0, 0, 0), new BoundsInt(0, 0, 0, 2, 2, 2) },
			new object[] { new BoundsInt(0, 0, 0, 2, 2, 2), new Vector3Int(1, 1, 1), new BoundsInt(0, 0, 0, 2, 2, 2) },
			new object[] { new BoundsInt(0, 0, 0, 2, 2, 2), new Vector3Int(2, 2, 2), new BoundsInt(0, 0, 0, 3, 3, 3) },
			new object[] { new BoundsInt(0, 0, 0, 2, 2, 2), new Vector3Int(-1, 1, 1), new BoundsInt(-1, 0, 0, 3, 2, 2) },
			new object[] { new BoundsInt(0, 0, 0, 2, 2, 2), new Vector3Int(-1, -1, -1), new BoundsInt(-1, -1, -1, 3, 3, 3) },
		};
		[TestCaseSource(nameof(With_TestCases))]
		public void With_ReturnsCorrectResult(BoundsInt source, Vector3Int consideredPosition, BoundsInt expectedResult)
		{
			BoundsInt result = BoundsIntUtilities.With(source, consideredPosition);

			result.Should().Be(expectedResult);
		}
	}
}