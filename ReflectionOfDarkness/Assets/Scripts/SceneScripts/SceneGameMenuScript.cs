using ReflectionOfDarknes.Networking.Steam;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneGameMenuScript : MonoBehaviour
{
    public void ButtonOnClickAsStart()
    {
        //Root.instance.steam.CreateLobby();
        //Root.instance.LoadSceneSinglePlayer(SceneType.SceneNewGame,
        //                            LoadSceneMode.Single,
        //                            LocalPhysicsMode.Physics2D
        //    );
        SteamLobby.singleton.CreateLobby();
    }
    public void ButtonOnClickAsJoin()
    {
        //Root.instance.LoadSceneSinglePlayer(SceneType.SceneJoinGame,
        //                        LoadSceneMode.Single,
        //                        LocalPhysicsMode.Physics2D
        //);
        SteamLobby.singleton.JoinGameTest();
    }
    public void ButtonOnClickAsExit()
    {
        Application.Quit();
    }
}