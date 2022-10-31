using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using UnityEngine.SceneManagement;

public class PlayerObjectController : NetworkBehaviour
{
    //Player Data
    [SyncVar] public int ConnectionID;
    [SyncVar] public int PlayerID;
    [SyncVar] public ulong PlayerSteamID;
    [SyncVar(hook = nameof(PlayerNameUpdate))] public string PlayerName;
    [SyncVar(hook = nameof(PlayerReadyUpdate))] public bool IsReady;

    private CustomNetworkManager manager;

    private CustomNetworkManager Manager
    {
        get
        {
            if (manager != null) return manager;
            return manager = CustomNetworkManager.singleton as CustomNetworkManager;
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public override void OnStartAuthority()
    {
        CmdSetPlayerName(SteamFriends.GetPersonaName().ToString());
        gameObject.name = "LocalGamePlayer";
        LobbyController.Instance.FindLocalPlayer();
        LobbyController.Instance.UpdaateLobbyName();
    }

    public override void OnStartClient()
    {
        Manager.GamePlayers.Add(this);
        LobbyController.Instance.UpdaateLobbyName();
        LobbyController.Instance.UpdatePlayerList();
    }

    public override void OnStopClient()
    {
        Manager.GamePlayers.Remove(this);
        LobbyController.Instance.UpdatePlayerList();
    }

    [Command]
    private void CmdSetPlayerName(string PlayerName)
    {
        this.PlayerNameUpdate(this.PlayerName, PlayerName);
    }
    private void CmdSetPlayerReady()
    {
        this.PlayerReadyUpdate(this.IsReady, !this.IsReady);
    }

    public void ChangeReady()
    {
        if (hasAuthority)
        {
            CmdSetPlayerReady();
        }
    }

    public void PlayerNameUpdate(string OldVal, string NewVal)
    {
        if (isServer) //Host
        {
            this.PlayerName = NewVal;
        }
        if (isClient) //Client
        {
            LobbyController.Instance.UpdatePlayerList();
        }
    }

    private void PlayerReadyUpdate(bool oldval, bool newval)
    {
        if (isServer)
        {
            this.IsReady = newval;
        }
        if (isClient)
        {
            LobbyController.Instance.UpdatePlayerList();
        }
    }

    //Start Game
    public void CanStartGame(string SceneName)
    {
        if (hasAuthority)
        {
            CmdCanStartGame(SceneName);
        }
    }

    [Command]
    public void CmdCanStartGame(string SceneName)
    {
        manager.StartGame(SceneName);
    }
}
