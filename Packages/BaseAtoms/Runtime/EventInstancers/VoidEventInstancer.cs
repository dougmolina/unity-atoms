using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Instancer of type `Void`. Inherits from `AtomEventInstancer&lt;Void, VoidEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-sign-blue")]
    [AddComponentMenu("Unity Atoms/Event Instancers/Void Event Instancer")]
    public class VoidEventInstancer : AtomBaseEventInstancer
    {
        public override AtomEventBase EventNoValue { get => Event; }

        /// <summary>
        /// Getter for retrieving the in memory runtime Event.
        /// </summary>
        public AtomEventBase Event { get => _inMemoryCopy; }

        /// <summary>
        /// Getter for retrieving the base Event.
        /// </summary>
        public AtomEventBase Base { get => _base; }

        [SerializeField]
        [ReadOnly]
        private AtomEventBase _inMemoryCopy;

        /// <summary>
        /// The Event that the in memory copy will be based on when created at runtime.
        /// </summary>
        [SerializeField]
        private AtomEventBase _base = null;

        private void OnEnable()
        {
            if (_base == null)
            {
                _inMemoryCopy = ScriptableObject.CreateInstance<AtomEventBase>();
            }
            else if (_inMemoryCopy == null)
            {
                _inMemoryCopy = Instantiate(_base);
            }
        }

        /// <summary>
        /// Raises the instanced Event.
        /// </summary>
        public override void Raise()
        {
            Event.Raise();
        }

        /// <summary>
        /// Raises the base Event.
        /// </summary>
        public override void RaiseBase()
        {
            Base.Raise();
        }
    }
}
