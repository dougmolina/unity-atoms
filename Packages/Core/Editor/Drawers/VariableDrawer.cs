using UnityEditor;
using UnityEngine;

namespace UnityAtoms.Editor
{
    public abstract class VariableDrawer<T> : AtomDrawer<T>
        where T : ScriptableObject
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.serializedObject.isEditingMultipleObjects || property.objectReferenceValue == null)
            {
                base.OnGUI(position, property, label);
                return;
            }

            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, label);

            SerializedObject inner = new SerializedObject(property.objectReferenceValue);
            SerializedProperty valueProp = inner.FindProperty("_value");
            float width = GetPreviewSpace(valueProp.type);
            Rect previewRect = new(position);
            previewRect.width = GetPreviewSpace(valueProp.type);
            position.xMin = previewRect.xMax;

            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            EditorGUI.BeginDisabledGroup(true);
            EditorGUI.PropertyField(previewRect, valueProp, GUIContent.none, false);
            EditorGUI.EndDisabledGroup();

            position.x = position.x + 6f;
            position.width = position.width - 6f;
            base.OnGUI(position, property, GUIContent.none);

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        private float GetPreviewSpace(string type)
        {
            switch (type)
            {
                case "Vector2":
                case "Vector3":
                    return 128;

                default:
                    return 100;
            }
        }
    }
}
