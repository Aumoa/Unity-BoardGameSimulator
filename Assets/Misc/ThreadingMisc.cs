// Copyright 2020-2022 Aumoa.lib. All right reserved.

using System.Threading;

using UnityEngine;

public static class ThreadingMisc
{
    private static int _gameThreadId;

    public static void Initialize()
    {
        Debug.Assert(_gameThreadId == 0);
        _gameThreadId = Thread.CurrentThread.ManagedThreadId;
    }

    public static void Shutdown()
    {
        _gameThreadId = 0;
    }

    public static bool IsInGameThread => _gameThreadId == Thread.CurrentThread.ManagedThreadId;
}
