// Copyright 2020-2022 Aumoa.lib. All right reserved.

using System;
using System.Threading.Tasks;

[Serializable]
public abstract class GameInstanceSubsystem
{
    public abstract Task InitializeAsync();

    public abstract Task PostInitializeAsync();

    public abstract void Shutdown();
}
