using System;
using System.Collections;
using GameLogic;
using Osnowa.Osnowa.Example;
using Osnowa.Osnowa.Grid;
using Osnowa.Osnowa.RNG;
using PCG;
using TMPro;
using UnityEngine;
using Zenject;

public class MenuMapGenerator : MonoBehaviour
{
	public MapGenerator MapGenerator;

	public TMP_InputField SeedInput;

	public GameObject GeneratingOverlayPanel; 
	public GameObject StartingOverlayPanel; 
	public TextMeshProUGUI GeneratingText;
	public TMP_Dropdown Difficulty;

	public GameObject StartScreen;
	public GameObject[] ToEnableWhenStarting;
	public GameObject[] ToEnableAfterGenerating;

	private IRandomNumberGenerator _rng;
	private TilemapGenerator _tilemapGenerator;
	private IWorldActorFiller _worldActorFiller;
	private IPositionFlagsResolver _positionFlagsResolver;
	private IExampleContextManager _contextManager;
	private IUiFacade _uiFacade;

	[Inject]
	public void Init(IRandomNumberGenerator rng, TilemapGenerator tilemapGenerator, IWorldActorFiller worldActorFiller, 
		IPositionFlagsResolver positionFlagsResolver, IExampleContextManager contextManager, IUiFacade uiFacade)
	{
		_rng = rng;
		_tilemapGenerator = tilemapGenerator;
		_worldActorFiller = worldActorFiller;
		_positionFlagsResolver = positionFlagsResolver;
		_contextManager = contextManager;
		_uiFacade = uiFacade;
	}

	void Start()
	{
		//		SeedInput.text += new System.Random().Next(1000).ToString();
		ResetRngWithSeedFromInput();
	}

	public void Generate()
	{
		_contextManager.CreateAndUseNewContext(MapGenerator.WorldGeneratorConfig.XSize, MapGenerator.WorldGeneratorConfig.YSize);
		ResetRngWithSeedFromInput();
		StartCoroutine(GeneratingPostPreview());
	}

	public void StartGame()
	{
		StartCoroutine(StartingGame());
	}

	public IEnumerator StartingGame()
	{
		StartingOverlayPanel.SetActive(true);

		// done on start because otherwise there would be problem with retained Entitas entities (solvable of course)
		GeneratingText.text += Environment.NewLine + "...";
		yield return new WaitForSeconds(0.3f);

		if (Difficulty.value == 0) // easy
		{
		}

		float enemyCountRate = Difficulty.value == 0 ? 0.9f : Difficulty.value == 1 ? 1f : 1.1f;
		_worldActorFiller.FillWithActors(enemyCountRate);

		yield return new WaitForSeconds(0.3f);
		_tilemapGenerator.Clear();
		yield return new WaitForSeconds(0.3f);
		_tilemapGenerator.Generate();

		yield return new WaitForSeconds(0.1f);

		foreach (GameObject gameObjectToEnable in ToEnableWhenStarting)
		{
			gameObjectToEnable.SetActive(true);
			yield return new WaitForSeconds(0.05f); // without this objects may start in wrong order, causing data to be corrupted
		}

		StartingOverlayPanel.SetActive(false);
		_uiFacade.AddLogEntry("<color=#adf>Welcome!</color> \nHave fun working with Osnowa.");
		_uiFacade.AddLogEntry("Feel free to play around with the engine.");

		gameObject.SetActive(false);
	}

	private IEnumerator GeneratingPostPreview()
	{
		GeneratingOverlayPanel.SetActive(true);
		GeneratingText.text = "Island generation:" + Environment.NewLine;

		yield return new WaitForSeconds(0.05f);
		ResetRngWithSeedFromInput();
		yield return new WaitForSeconds(0.03f);
		IEnumerator generatingInBackground = MapGenerator.GeneratingInBackground(AppendProgressToGenerationLog);
		while (generatingInBackground.MoveNext())
		{
			yield return new WaitForSeconds(0.03f);
			GeneratingText.text += ".";
		}

		GeneratingText.text += Environment.NewLine + "...";
		yield return new WaitForSeconds(0.3f);
		_positionFlagsResolver.InitializePositionFlags();
		yield return new WaitForSeconds(0.3f);

		GeneratingText.text += Environment.NewLine + Environment.NewLine + "Finished!";

		GeneratingOverlayPanel.SetActive(false);

		foreach (GameObject gameObjectToEnable in ToEnableAfterGenerating)
		{
			gameObjectToEnable.SetActive(true);
		}
	}

	private void ResetRngWithSeedFromInput()
	{
		int seed = SeedInput.text.ToLower().GetHashCode();
		Debug.Log("Resetting RNG with seed: " + seed);
		_rng.ResetWithSeed(seed);
	}

	private void AppendProgressToGenerationLog(string text)
	{
		GeneratingText.text += Environment.NewLine + "...";
	}
}