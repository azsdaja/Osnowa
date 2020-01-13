using System;
using NUnit.Framework;

namespace Osnowa.Tests.ECS
{
	[TestFixture]
	public class IdTests
	{
		[Test]
		public void CreationIndex()
		{
			var context = new GameContext();
			var entity = context.CreateEntity();
			entity.AddId(Guid.NewGuid());
			Console.WriteLine(entity.creationIndex);
			Console.WriteLine(entity.id.Id);
			var entity2 = context.CreateEntity();
			Console.WriteLine(entity2.creationIndex);
			//entity2.AddId(1);
//			Console.WriteLine(entity2.id.Id);
			Console.WriteLine($"{context.GetEntityWithId(Guid.NewGuid()).creationIndex}");
		}
	}
}