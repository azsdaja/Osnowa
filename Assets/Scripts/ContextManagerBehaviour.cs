using System;
using Osnowa.Osnowa.Context;
using Osnowa.Osnowa.Example;
using UnityEngine;

public class ContextManagerBehaviour : MonoBehaviour, IExampleContextManager
{
	private readonly IOsnowaContextManager _contextManager;

	public ContextManagerBehaviour()
	{
		_contextManager = new OsnowaContextManager();
		_contextManager.ContextReplaced += ContextReplaced;
	}

	public IOsnowaContext Current => _contextManager.Current;

	IExampleContext IExampleContextManager.Current
	{
		get { throw new NotSupportedException(); }
	}

	public bool HasContext => _contextManager.HasContext;
	
	public void LoadContextFromFile(string fileName)
	{
		_contextManager.LoadContextFromFile(fileName);
	}

	public void SaveContext()
	{
		_contextManager.SaveContext();
	}

	public event Action<IOsnowaContext> ContextReplaced;
		
	public void ReplaceContext(IOsnowaContext newContext)
	{
		_contextManager.ReplaceContext(newContext);
	}

	public void CreateAndUseNewContext(int xSize, int ySize)
	{
		_contextManager.CreateAndUseNewContext(xSize, ySize);
	}
}