using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class SceneField
{
    [SerializeField]
    private Object _sceneAsset;

    [SerializeField]
    private string _sceneName = "";

    public string SceneName => _sceneName;

    public static implicit operator string(SceneField sceneField)
    {
        return sceneField.SceneName;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(SceneField))]
public class SceneFieldPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        
        SerializedProperty sceneAssetProp = property.FindPropertyRelative("_sceneAsset");
        SerializedProperty sceneNameProp = property.FindPropertyRelative("_sceneName");

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        if (sceneAssetProp != null)
        {
            EditorGUI.BeginChangeCheck();
            
            Object value = EditorGUI.ObjectField(position, sceneAssetProp.objectReferenceValue, typeof(SceneAsset), false);
            
            if (EditorGUI.EndChangeCheck())
            {
                sceneAssetProp.objectReferenceValue = value;
                
                if (sceneAssetProp.objectReferenceValue != null)
                {
                    sceneNameProp.stringValue = (sceneAssetProp.objectReferenceValue as SceneAsset).name;
                }
                else
                {
                    sceneNameProp.stringValue = "";
                }
            }
        }
        
        EditorGUI.EndProperty();
    }
}
#endif