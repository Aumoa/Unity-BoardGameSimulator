// Copyright 2020-2022 Aumoa.lib. All right reserved.

using System.Threading.Tasks;

using UnityEngine;

public abstract class SceneInstance
{
    public abstract string SceneAssetPath { get; }

    public virtual Task LoadSceneAsync()
    {
        return Task.CompletedTask;
    }

    public virtual void UnloadScene()
    {
    }

    public GameObject ScenePersistentObject { get; private set; }

    public virtual void BeginPlay()
    {
        ScenePersistentObject = new GameObject("ScenePersistentObject");
    }

    public virtual void EndPlay()
    {
        if (ScenePersistentObject != null)
        {
            Object.Destroy(ScenePersistentObject);
            ScenePersistentObject = null;
        }
    }
}
