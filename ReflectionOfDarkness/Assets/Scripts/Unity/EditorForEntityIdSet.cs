using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR || DEBUG
namespace ReflectionOfDarknes.Unity
{
    [CustomEditor(typeof(Entity), true)]
    public class EditorEntity : Editor
    {
        public override void OnInspectorGUI()
        {
            Entity obj = (Entity)target;

            if (GUILayout.Button("Set New [guid]"))
            {
                if (EditorUtility.DisplayDialogComplex("Hmmmmmm..... Emin misin ?", "Guid'i Değiştirmek istediğine emin misin ?", "Evet Değiştir [Guid 'in değişmesini istiyorum]", "Hayır Değiştirme", null) == 0)
                {
                    obj.SetGuid();
                }
            }
            DrawDefaultInspector();
        }
    }
}
#endif