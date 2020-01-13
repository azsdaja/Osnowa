using System;
using System.Linq;
using ECS.Features.Statuses;
using FluentAssertions;
using NUnit.Framework;

namespace Osnowa.Tests.ECS
{
	[TestFixture]
	public class AllStatusComponentsTests
	{
		[Test]
		public void AllComponentsDerivingFromStatusComponentAreConsidered()
		{
			Type[] allActualTypes = (from typeInAssembly in typeof(StatusComponent).Assembly.GetTypes()
									 let statusComponentType = typeof(StatusComponent)
									 where typeInAssembly != statusComponentType && statusComponentType.IsAssignableFrom(typeInAssembly)
									 select typeInAssembly).ToArray();

			Type[] allTypesDeclared = AllStatusComponents.Types();
			
			allTypesDeclared.Should().BeEquivalentTo(allActualTypes);
		}
	}
}