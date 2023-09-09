// Copyright 2020-2022 Aumoa.lib. All right reserved.

using UnityEngine;
using UnityEngine.UI;

public class LobbyRootWidget : MonoBehaviour
{
    [SerializeField]
    private Button Button_Exit;

    private void Awake()
    {
        Button_Exit.onClick.AddListener(OnClicked_Exit);
    }

    protected void OnClicked_Exit()
    {
        GameInstance.Instance.ExitGame();
    }
}
