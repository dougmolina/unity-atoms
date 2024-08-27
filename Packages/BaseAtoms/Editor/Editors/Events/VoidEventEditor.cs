#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityEngine.UIElements;
using UnityAtoms.Editor;
using UnityEngine;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `Void`. Inherits from `AtomEventEditor&lt;Void, VoidEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomEditor(typeof(VoidEvent))]
    public sealed class VoidEventEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            IMGUIContainer defaultInspector = new IMGUIContainer(() => DrawDefaultInspector());
            root.Add(defaultInspector);

            var atomEvent = target as AtomEventBase;

            var runtimeWrapper = new VisualElement();
            runtimeWrapper.SetEnabled(Application.isPlaying);
            runtimeWrapper.Add(new Button(() =>
            {
                atomEvent.Raise();
            })
            {
                text = "Raise"
            });
            root.Add(runtimeWrapper);

#if !UNITY_ATOMS_GENERATE_DOCS
            StackTraceEditor.RenderStackTrace(root, atomEvent.GetInstanceID());
#endif

            return root;
        }
    }
}
#endif
