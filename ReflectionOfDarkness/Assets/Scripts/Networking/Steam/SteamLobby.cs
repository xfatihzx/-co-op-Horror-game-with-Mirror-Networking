using Mirror;
using Steamworks;
using UnityEngine;

namespace ReflectionOfDarknes.Networking.Steam
{
    [RequireComponent(typeof(MirrorNetworking))]
    [RequireComponent(typeof(Mirror.FizzySteam.FizzySteamworks))]
    public class SteamLobby : CacheBehaviour<SteamLobby>
    {
        public CSteamID lobbyId;
        protected Callback<LobbyCreated_t> lobbyCreated;
        protected Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;
        protected Callback<LobbyEnter_t> lobbyEntered;

        private const string HostAddressKey = "HostAddress";

        void Start()
        {
            if (!SteamNetworking.Initialized) return;

            lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
            gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
            lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);

            string name = SteamFriends.GetPersonaName();
            Debug.Log("Steam client name : " + name);
        }

        public void CreateLobby()
        {
            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, MirrorNetworking.instance.maxConnections);
        }

        private void OnLobbyCreated(LobbyCreated_t callback)
        {
            if (callback.m_eResult != EResult.k_EResultOK)
            {
                return;
            }

            MirrorNetworking.instance.StartHost();

            lobbyId = new CSteamID(callback.m_ulSteamIDLobby);

            SteamMatchmaking.SetLobbyData(
                lobbyId,
                HostAddressKey,
                SteamUser.GetSteamID().ToString());

            MirrorNetworking.instance.ServerChangeScene(SceneType.SceneLobby.ToString());
        }

        public void JoinGameTest()
        {
            SteamMatchmaking.JoinLobby(lobbyId);
        }

        private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
        {
            SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
        }

        private void OnLobbyEntered(LobbyEnter_t callback)
        {
            if (NetworkServer.active) { return; }

            LobbyJoined(callback.m_ulSteamIDLobby);
        }

        private void LobbyJoined(ulong m_ulSteamIDLobby)
        {
            string hostAddress = SteamMatchmaking.GetLobbyData(
                new CSteamID(m_ulSteamIDLobby),
                HostAddressKey);

            MirrorNetworking.instance.networkAddress = hostAddress;
            MirrorNetworking.instance.StartClient();
        }
    }
}