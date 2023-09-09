// Copyright 2020-2022 Aumoa.lib. All right reserved.

using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEditor;

using UnityEngine;

public partial class GameInstance : MonoBehaviour
{
    [SerializeField]
    private UIManager InstancedUIManager;

    [SerializeField]
    private SceneManager InstancedSceneManager;

    private readonly List<GameInstanceSubsystem> _instancedSubsystems = new();

    public static GameInstance Instance { get; private set; }

    private void Awake()
    {
        // Register singleton instance.
        Debug.Assert(Instance == null);
        Instance = this;
    }

    private IEnumerator Start()
    {
        // Add all components to subsystem collection.
        _instancedSubsystems.Add(InstancedUIManager);
        _instancedSubsystems.Add(InstancedSceneManager);

        // Set game instance as persistent.
        DontDestroyOnLoad(gameObject);

        // Initialize MISC components.
        ThreadingMisc.Initialize();

        // Initialize and waiting.
        bool bCompleted = false;
        Task result = null;
        InitializeSubsystems().ContinueWith(p =>
        {
            bCompleted = true;
            result = p;
        });

        yield return new WaitUntil(() => bCompleted);
        result.GetAwaiter().GetResult();
    }

    private void OnDestroy()
    {
        // Destroy all subsystems.
        DestroySubsystems();

        // Finalize MISC components.
        ThreadingMisc.Shutdown();

        // Remove singleton instance.
        Instance = null;
    }

    private async Task InitializeSubsystems()
    {
        Debug.Log("Initialize subsystems.");

        List<Task> tasks = new();
        foreach (var instance in _instancedSubsystems)
        {
            tasks.Add(instance.InitializeAsync());
        }
        await Task.WhenAll(tasks);
        Debug.Assert(ThreadingMisc.IsInGameThread);

        tasks.Clear();
        foreach (var instance in _instancedSubsystems)
        {
            tasks.Add(instance.PostInitializeAsync());
        }
        await Task.WhenAll(tasks);
        Debug.Assert(ThreadingMisc.IsInGameThread);
    }

    private void DestroySubsystems()
    {
        foreach (var subsystem in _instancedSubsystems)
        {
            subsystem.Shutdown();
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
