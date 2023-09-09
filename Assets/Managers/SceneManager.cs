// Copyright 2020-2022 Aumoa.lib. All right reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using UnityEngine;

[Serializable]
public class SceneManager : GameInstanceSubsystem
{
    [SerializeField]
    private string EntrySceneName = "LobbyScene";

    public static void Initialize()
    {
    }

    public override Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public override async Task PostInitializeAsync()
    {
        var baseType = typeof(SceneInstance);

        var sceneMap = baseType.Assembly.GetTypes()
            .Where(p => baseType.IsAssignableFrom(p) && p != baseType)
            .ToDictionary(p => p.Name, p => p);

        if (sceneMap.TryGetValue(EntrySceneName, out Type sceneType) == false)
        {
            throw new KeyNotFoundException("EntrySceneName is not valid.");
        }

        await LoadSceneAsync(sceneType);
    }

    public override void Shutdown()
    {
    }

    private SceneInstance _currentScene = null;

    public Task<T> LoadSceneAsync<T>() where T : SceneInstance, new()
    {
        return LoadSceneAsync(typeof(T)).ContinueWith(p => (T)p.Result);
    }

    public async Task<SceneInstance> LoadSceneAsync(Type sceneType)
    {
        Debug.Assert(sceneType.IsAssignableFrom(typeof(SceneInstance)));

        if (_currentScene != null)
        {
            _currentScene.EndPlay();
        }

        var ctor = sceneType.GetConstructor(Array.Empty<Type>());
        Debug.Assert(ctor != null);

        var scene = (SceneInstance)ctor.Invoke(Array.Empty<object>());
        await scene.LoadSceneAsync();
        Debug.Assert(ThreadingMisc.IsInGameThread);

        if (_currentScene != null)
        {
            _currentScene.UnloadScene();
        }
        _currentScene = scene;
        await UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scene.SceneAssetPath);
        scene.BeginPlay();

        return scene;
    }
}
