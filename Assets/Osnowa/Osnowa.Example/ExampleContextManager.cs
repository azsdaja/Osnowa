namespace Osnowa.Osnowa.Example
{
    using System;
    using System.IO;
    using Context;
    using UnityEngine;

    class ExampleContextManager : OsnowaContextManager, IExampleContextManager
    {
        private IExampleContext _current;

        public new IExampleContext Current
        {
            get
            {
                if (_current == null)
                    throw new NullReferenceException("Trying to access current context which is null.");
                return _current;
            }
            private set => _current = value;
        }

        public void ReplaceContext(IExampleContext newContext)
        {
            Current = newContext;
            base.ReplaceContext(newContext);
        }

        public override void LoadContextFromFile(string fileName)
        {
            string filePath = Path.Combine(Application.dataPath, "contexts", fileName);

            using (StreamReader file = new StreamReader(filePath))
            {
                string jsonContent = file.ReadToEnd();
                var loadedContext = JsonUtility.FromJson<ExampleContext>(jsonContent);
                Current = loadedContext;
                ReplaceContext(loadedContext);
            }
        }

        public override void SaveContext()
        {
            string fileName = DateTime.UtcNow.ToString("s") + ".json";
            string directory = Path.Combine(Application.dataPath, "contexts", fileName);
            string result = JsonUtility.ToJson(Current);
            using (StreamWriter file = new StreamWriter(directory))
            {
                file.Write(result);
            }
        }

        public override void CreateAndUseNewContext(int xSize, int ySize)
        {
            var newContext = new ExampleContext(xSize, ySize);
            Current = newContext;
            ReplaceContext(newContext);
        }
    }
}