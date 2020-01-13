#if UNITY_EDITOR
namespace Libraries.EditorCoroutines.Scripts
{
	using System.Collections;
	using UnityEditor;
	using UnityEngine;

	public class CoroutineWindowExample : EditorWindow
	{
		[MenuItem("Window/Coroutine Example")]
		public static void ShowWindow()
		{
			EditorWindow.GetWindow(typeof(CoroutineWindowExample));
		}

		void OnGUI()
		{
			if (GUILayout.Button("Start"))
			{
				this.StartCoroutine(Example());
			}

			if (GUILayout.Button("Start WWW"))
			{
				this.StartCoroutine(ExampleWWW());
			}

			if (GUILayout.Button("Start Nested"))
			{
				this.StartCoroutine(ExampleNested());
			}

			if (GUILayout.Button("Stop"))
			{
				this.StopCoroutine("Example");
			}
			if (GUILayout.Button("Stop all"))
			{
				this.StopAllCoroutines();
			}

			if (GUILayout.Button("Also"))
			{
				this.StopAllCoroutines();
			}
		}

		IEnumerator Example()
		{
			while (true)
			{
				Debug.Log("Hello EditorCoroutine!");
				yield return new WaitForSeconds(2f);
			}
		}

		IEnumerator ExampleWWW()
		{
			while (true)
			{
#pragma warning disable 618
				var www = new WWW("https://unity3d.com/");
#pragma warning restore 618
				yield return www;
				Debug.Log("Hello EditorCoroutine!" + www.text);
				yield return new WaitForSeconds(2f);
			}
		}

		IEnumerator ExampleNested()
		{
			while (true)
			{
				yield return new WaitForSeconds(2f);
				Debug.Log("I'm not nested");
				yield return this.StartCoroutine(ExampleNestedOneLayer());
			}
		}

		IEnumerator ExampleNestedOneLayer()
		{
			yield return new WaitForSeconds(2f);
			Debug.Log("I'm one layer nested");
			yield return this.StartCoroutine(ExampleNestedTwoLayers());
		}

		IEnumerator ExampleNestedTwoLayers()
		{
			yield return new WaitForSeconds(2f);
			Debug.Log("I'm two layers nested");
		}


		class NonEditorClass
		{
			public void DoSomething(bool start, bool stop, bool stopAll)
			{
				if (start)
				{
					EditorCoroutines.StartCoroutine(Example(), this);
				}
				if (stop)
				{
					EditorCoroutines.StopCoroutine("Example", this);
				}
				if (stopAll)
				{
					EditorCoroutines.StopAllCoroutines(this);
				}
			}

			IEnumerator Example()
			{
				while (true)
				{
					Debug.Log("Hello EditorCoroutine!");
					yield return new WaitForSeconds(2f);
				}
			}
		}
	}
}
#endif
