using UnityEngine;
using UnityEditor;
using System.Collections;

#if UNITY_EDITOR || DEBUG
namespace ReflectionOfDarknes.Unity
{
    class EditorScrips : EditorWindow
    {

        [System.Obsolete]
        [MenuItem("Go/Root %#&1")]
        public static void FastGo1()
        {
            EditorApplication.SaveScene();
            EditorApplication.OpenScene("Assets/Scenes/Orginals/1-Root/SceneRoot.unity");
        }
        [System.Obsolete]
        [MenuItem("Go/GameMenu %#&2")]
        public static void FastGo2()
        {
            EditorApplication.SaveScene();
            EditorApplication.OpenScene("Assets/Scenes/Orginals/2-GameMenu/SceneGameMenu.unity");
        }
        [System.Obsolete]
        [MenuItem("Go/NewGame %#&3")]
        public static void FastGo3()
        {
            EditorApplication.SaveScene();
            EditorApplication.OpenScene("Assets/Scenes/Orginals/3-NewGame/SceneNewGame.unity");
        }
        [System.Obsolete]
        [MenuItem("Go/JoinGame %#&4")]
        public static void FastGo4()
        {
            EditorApplication.SaveScene();
            EditorApplication.OpenScene("Assets/Scenes/Orginals/4-JoinGame/SceneJoinGame.unity");
        }
        [System.Obsolete]
        [MenuItem("Go/Lobby Mods %#&5")]
        public static void FastGo5()
        {
            EditorApplication.SaveScene();
            EditorApplication.OpenScene("Assets/Scenes/Orginals/5-Lobby/SceneLobby.unity");
        }
        [System.Obsolete]
        [MenuItem("Go/Null %#&6")]
        public static void FastGo6()
        {
            EditorApplication.SaveScene();
            EditorApplication.OpenScene("Assets/Scenes/Orginals/aaaaa/bbbbbb.unity");
        }
        [System.Obsolete]
        [MenuItem("Go/Null %#&7")]
        public static void FastGo7()
        {
            EditorApplication.SaveScene();
            EditorApplication.OpenScene("Assets/Scenes/Orginals/aaaaa/bbbbbb.unity");
        }
        [System.Obsolete]
        [MenuItem("Go/Null 3d %#&8")]
        public static void FastGo8()
        {
            EditorApplication.SaveScene();
            EditorApplication.OpenScene("Assets/Scenes/Orginals/aaaaa/bbbbbb.unity");
        }
        [System.Obsolete]
        [MenuItem("Go/Null 3d %#&9")]
        public static void FastGo9()
        {
            EditorApplication.SaveScene();
            EditorApplication.OpenScene("Assets/Scenes/Orginals/aaaaa/bbbbbb.unity");
        }
        [System.Obsolete]
        [MenuItem("Go/Null 3d %#&0")]
        public static void FastGo0()
        {
            EditorApplication.SaveScene();
            EditorApplication.OpenScene("Assets/Scenes/Orginals/aaaaa/bbbbbb.unity");
        }
        [System.Obsolete]
        [MenuItem("Play/Play Menu %#&p")]
        public static void RunMainScene()
        {
            EditorApplication.SaveScene();
            EditorApplication.OpenScene("Assets/Scenes/Orginals/2-GameMenu/SceneGameMenu.unity");
            EditorApplication.isPlaying = true;
        }
    }
}
#endif