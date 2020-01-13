using System;

namespace Osnowa.Osnowa.Context
{
	public interface IOsnowaContextManager
	{
		IOsnowaContext Current { get; }
		bool HasContext { get; }
		void LoadContextFromFile(string fileName);
		void SaveContext();
		event Action<IOsnowaContext> ContextReplaced;
		void ReplaceContext(IOsnowaContext newContext);
		void CreateAndUseNewContext(int xSize, int ySize);
	}
}