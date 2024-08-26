using UnityEngine;

namespace UnityAtoms
{
    /// <summary>
    /// An Event Instancer is a MonoBehaviour that takes an Event as a base and creates an in memory copy of it on OnEnable.
    /// This is handy when you want to use Events for prefabs that are instantiated at runtime.
    /// </summary>
    [EditorIcon("atom-icon-sign-blue")]
    [DefaultExecutionOrder(Runtime.ExecutionOrder.VARIABLE_INSTANCER)]
    public abstract class AtomBaseEventInstancer : MonoBehaviour, ICallable, IAtomInstancer
    {
        /// <summary>
        /// Getter for retrieving the in memory runtime Event.
        /// </summary>
        public abstract AtomEventBase EventNoValue
        {
            get;
        }

        /// <summary>
        /// Raises the instanced Event.
        /// </summary>
        public abstract void Raise();

        /// <summary>
        /// Raises the base instanced Event.
        /// </summary>
        public abstract void RaiseBase();

        public void Call()
        {
            Raise();
        }
    }
}
