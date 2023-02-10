using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR || DEBUG
[CustomPropertyDrawer(typeof(OnInspectorAttribute))]
public class OnInspectorAttributeDrawer : PropertyDrawer
{
    private float bottomOffset = 10;
    const int margin = 5;

    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        Debug.Log("Deneme");
        // Create property container element.
        var container = new VisualElement();

        // Create property fields.
        var field = new PropertyField(property);

        // Add fields to the container.
        container.Add(field);

        return container;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        OnInspectorAttribute attr = fieldInfo.GetCustomAttributes(typeof(OnInspectorAttribute), false).First() as OnInspectorAttribute;

        float result = EditorGUI.GetPropertyHeight(property, label, true);

        if (attr.Comment != null)
        {
            GUI.skin.label.wordWrap = true;
            EditorStyles.label.wordWrap = true;
            CommentScriptableObject scriptableObject = ScriptableObject.CreateInstance<CommentScriptableObject>();
            scriptableObject.Comment = attr.Comment;
            scriptableObject.name = "___GeneratedByComment0x01231111222-abc";
            SerializedObject serializedObject = new SerializedObject(scriptableObject);
            SerializedProperty serializedProperty = serializedObject.FindProperty("Comment");
            GUIContent content = new GUIContent($"☞{attr.Comment}☜");
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.wordWrap = true;
            style.fontSize = 14;
            style.fontStyle = FontStyle.Bold;
            style.normal.textColor = Color.white;
            //Rect rect = EditorGUILayout.GetControlRect(true, result);
            float lineHeight = style.CalcHeight(content, EditorGUIUtility.currentViewWidth - EditorGUIUtility.labelWidth) + bottomOffset + margin;
            //Debug.Log($@"{property.name} {rect.x} {rect.y} {rect.width} {rect.height}");
            result += lineHeight;
        }

        return result;
    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        OnInspectorAttribute attr = fieldInfo.GetCustomAttributes(typeof(OnInspectorAttribute), false).First() as OnInspectorAttribute;

        if (!string.IsNullOrEmpty(attr.Tooltip) && string.IsNullOrEmpty(label.tooltip))
        {
            label.tooltip = attr.Tooltip;
        }

        GUIContent labelCopy = new GUIContent(label);

        if (attr.Comment != null)
        {
            GUI.skin.label.wordWrap = true;
            EditorStyles.label.wordWrap = true;
            CommentScriptableObject scriptableObject = ScriptableObject.CreateInstance<CommentScriptableObject>();
            scriptableObject.Comment = attr.Comment;
            scriptableObject.name = "___GeneratedByComment0x01231111222-abc";
            SerializedObject serializedObject = new SerializedObject(scriptableObject);
            SerializedProperty serializedProperty = serializedObject.FindProperty("Comment");

            GUIContent contentLabel = new GUIContent(labelCopy);
            contentLabel.text = $"☝ Info ➩";

            GUIContent contentComment = new GUIContent(labelCopy);
            //contentComment.text = $"☞{attr.Comment}☜";
            contentComment.text = attr.Comment;

            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.wordWrap = true;
            style.fontSize = 14;
            style.fontStyle = FontStyle.Bold;
            style.normal.textColor = Color.white;
            float lineHeightComment = style.CalcHeight(contentComment, EditorGUIUtility.currentViewWidth - EditorGUIUtility.labelWidth);
            float lineHeightLabel = style.CalcHeight(contentLabel, EditorGUIUtility.labelWidth);


            Rect rect = new Rect(position.x, position.y + EditorGUI.GetPropertyHeight(property, label), EditorGUIUtility.labelWidth, lineHeightLabel);

            //EditorGUI.LabelField(rect, contentLabel, style);

            //rect.x = EditorGUIUtility.labelWidth;
            rect.height = lineHeightComment;
            rect.width = EditorGUIUtility.currentViewWidth - EditorGUIUtility.labelWidth;

            EditorGUI.LabelField(rect, contentComment, style);

            position.height -= lineHeightComment + bottomOffset;
        }

        if (attr.ReadOnly)
        {
            string rest;

            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:
                    rest = property.intValue.ToString();
                    break;
                case SerializedPropertyType.Boolean:

                    EditorGUI.DrawPreviewTexture(new Rect(position.x + EditorGUIUtility.labelWidth - 18, position.y + 2, 13, 13), property.boolValue
                        ? Resources.Load<Texture>("EditorBoolCheck")
                        : Resources.Load<Texture>("EditorBoolUnCheck"));

                    rest = property.boolValue.ToString();
                    break;
                case SerializedPropertyType.Float:
                    rest = property.floatValue.ToString();
                    break;
                case SerializedPropertyType.String:
                    Rect rect = new Rect(position.x + EditorGUIUtility.labelWidth - 45, position.y + 2, 45, 15);
                    GUIStyle style = new GUIStyle(GUI.skin.button);
                    style.fontSize = 9;
                    style.fontStyle = FontStyle.Bold;
                    style.normal.textColor = Color.red;
                    bool buttonClicked = Event.current.rawType == EventType.MouseDown && rect.Contains(Event.current.mousePosition);
                    if (GUI.Button(rect, new GUIContent("copy", "Copy value at right to ClipBoard"), style) || buttonClicked)
                    {
                        GUIUtility.systemCopyBuffer = property.stringValue;
                    }
                    if (property.stringValue is null) rest = "none";
                    else rest = property.stringValue;
                    break;
                case SerializedPropertyType.Color:
                    rest = $@"{property.colorValue.r} {property.colorValue.b} {property.colorValue.g} {property.colorValue.a} (red, blue, green, alpha)";
                    break;
                case SerializedPropertyType.ObjectReference:
                    if (property.objectReferenceValue is null) rest = "none";
                    else rest = property.objectReferenceValue.name;
                    break;
                case SerializedPropertyType.Enum:
                    if (property.enumNames is null || property.enumValueIndex < 0) rest = "none";
                    else rest = property.enumNames[property.enumValueIndex];
                    break;
                case SerializedPropertyType.Vector2:
                    rest = $@"{property.vector2Value.x} {property.vector2Value.y} (x, y)";
                    break;
                case SerializedPropertyType.Vector3:
                    rest = $@"{property.vector3Value.x} {property.vector3Value.y} {property.vector3Value.z} (x, y, z)";
                    break;
                case SerializedPropertyType.Vector4:
                    rest = $@"{property.vector4Value.x} {property.vector4Value.y} {property.vector4Value.z} {property.vector4Value.w} (x, y, z, w)";
                    break;
                case SerializedPropertyType.Rect:
                    rest = $@"{property.rectValue.x} {property.rectValue.y} (x, y)";
                    break;
                case SerializedPropertyType.ArraySize:
                    if (property.arrayElementType is null) rest = "none";
                    else rest = $@"{property.arrayElementType} {property.arraySize} (type, length)";
                    break;
                case SerializedPropertyType.AnimationCurve:
                    if (property.animationCurveValue is null) rest = "none";
                    else rest = property.animationCurveValue.length.ToString();
                    break;
                case SerializedPropertyType.Quaternion:
                    rest = $@"{property.quaternionValue.x} {property.quaternionValue.y} {property.quaternionValue.z} {property.quaternionValue.w} (x, y, z, w)";
                    break;
                case SerializedPropertyType.Vector2Int:
                    rest = $@"{property.vector2IntValue.x} {property.vector2IntValue.y} (x, y)";
                    break;
                case SerializedPropertyType.Vector3Int:
                    rest = $@"{property.vector3IntValue.x} {property.vector3IntValue.y} {property.vector3IntValue.z} (x, y, z)";
                    break;
                case SerializedPropertyType.RectInt:
                    rest = $@"{property.rectIntValue.x} {property.rectIntValue.y} (x, y)";
                    break;
                default:
                    Debug.Log($@"{property.propertyType}");
                    rest = "unsupported";
                    break;
            }

            var content = new GUIContent(labelCopy);
            content.text = rest;

            EditorGUI.LabelField(position, labelCopy, content);
        }
        else
        {
            EditorGUI.PropertyField(position, property, labelCopy);
        }
    }
}
#endif