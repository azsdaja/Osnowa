using System;
using System.IO;
using UnityEngine;

namespace Osnowa.Osnowa.Context
{
	public class OsnowaContextManager : IOsnowaContextManager
	{
		private IOsnowaContext _current;

		public bool HasContext => _current != null;
		
	    public IOsnowaContext Current
		{
			get
			{
				if(_current == null)
					throw new NullReferenceException("Trying to access current context which is null.");
				return _current;
			}
			private set => _current = value;
        }

		public void ReplaceContext(IOsnowaContext newContext)
		{
			Current = newContext;
			ContextReplaced?.Invoke(newContext);
		}

		public virtual void LoadContextFromFile(string fileName)
		{
			string filePath = Path.Combine(Application.dataPath, "contexts", fileName);

			using (StreamReader file = new StreamReader(filePath))
			{
				string jsonContent = file.ReadToEnd();
				var loadedContext = JsonUtility.FromJson<OsnowaContext>(jsonContent);
				Current = loadedContext;
				ContextReplaced?.Invoke(Current);
			}
		}

		public virtual void SaveContext()
		{
			string fileName = DateTime.UtcNow.ToString("s") + ".json";
			string directory = Path.Combine(Application.dataPath, "contexts", fileName);
			string result = JsonUtility.ToJson(Current);
			using (StreamWriter file = new StreamWriter(directory))
			{
				file.Write(result);
			}
		}

		public virtual void CreateAndUseNewContext(int xSize, int ySize)
		{
		    var context = new OsnowaContext(xSize, ySize);
			Current = context;
			ContextReplaced?.Invoke(Current);
		}

		public event Action<IOsnowaContext> ContextReplaced;
	}
}