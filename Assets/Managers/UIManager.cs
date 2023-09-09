// Copyright 2020-2022 Aumoa.lib. All right reserved.

using System;
using System.Threading.Tasks;

[Serializable]
public class UIManager : GameInstanceSubsystem
{
    public static void Initialize()
    {
    }

    public override Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public override Task PostInitializeAsync()
    {
        return Task.CompletedTask;
    }

    public override void Shutdown()
    {
    }
}
