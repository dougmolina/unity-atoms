using System;
using UnityEngine;
using UnityAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// A Collection / Dictionary of Atom Variables (`AtomBaseVariable`).
    /// </summary>
    [CreateAssetMenu(menuName = "Unity Atoms/Collections/Collection", fileName = "Collection")]
    [EditorIcon("atom-icon-kingsyellow")]
    public class AtomCollection : AtomBaseVariable<StringReferenceAtomBaseVariableDictionary>, IGetValue<IAtomCollection>, IWithCollectionEvents
    {
        /// <summary>
        /// Get value as an `IAtomCollection`. Needed in order to inject Collection into the Variable Instancer class.
        /// </summary>
        /// <returns>The value as an `IAtomCollection`.</returns>
        public IAtomCollection GetValue() => this.Value;

        /// <summary>
        /// Event for when and item is added to the collection.
        /// </summary>
        public AtomBaseVariableEvent Added { get => _added; set => _added = value; }

        /// <summary>
        /// Event for when and item is removed from the collection.
        /// </summary>
        public AtomBaseVariableEvent Removed { get => _removed; set => _removed = value; }

        /// <summary>
        /// Event for when the collection is cleared.
        /// </summary>
        public AtomEventBase Cleared { get => _cleared; set => _cleared = value; }

        [SerializeField]
        private AtomBaseVariableEvent _added;

        [SerializeField]
        private AtomBaseVariableEvent _removed;

        [SerializeField]
        private AtomEventBase _cleared;

        void OnEnable()
        {
            if (Value == null) return;

            Value.Added += PropogateAdded;
            Value.Removed += PropogateRemoved;
            Value.Cleared += PropogateCleared;
        }

        void OnDisable()
        {
            if (Value == null) return;
			
			//Calling OnAfterDeserialize again here, as Collection may use StringConstant or StringVariable as key, and Unity doesnt ensures execution order in Scriptable Objects
            // As such is possible that the Collection is called before its keys are ready to be used.
            // On OnEnable, all atoms are supposed to already have been initialized, and calling OnAfterDeserialize, will fix any issue with not ready keys
            Value.OnAfterDeserialize();

            Value.Added -= PropogateAdded;
            Value.Removed -= PropogateRemoved;
            Value.Cleared -= PropogateCleared;
        }

        #region Observable
        /// <summary>
        /// Make the add event into an `IObservable&lt;T&gt;`. Makes Collection's add Event compatible with for example UniRx.
        /// </summary>
        /// <returns>The add Event as an `IObservable&lt;T&gt;`.</returns>
        public IObservable<AtomBaseVariable> ObserveAdd()
        {
            if (Added == null)
            {
                throw new Exception("You must assign an Added event in order to observe when adding to the collection.");
            }

            return new ObservableEvent<AtomBaseVariable>(Added.Register, Added.Unregister);
        }

        /// <summary>
        /// Make the remove event into an `IObservable&lt;T&gt;`. Makes Collection's remove Event compatible with for example UniRx.
        /// </summary>
        /// <returns>The remove Event as an `IObservable&lt;T&gt;`.</returns>
        public IObservable<AtomBaseVariable> ObserveRemove()
        {
            if (Removed == null)
            {
                throw new Exception("You must assign a Removed event in order to observe when removing from the collection.");
            }

            return new ObservableEvent<AtomBaseVariable>(Removed.Register, Removed.Unregister);
        }

        /// <summary>
        /// Make the clear event into an `IObservable&lt;Void&gt;`. Makes Collection's clear Event compatible with for example UniRx.
        /// </summary>
        /// <returns>The clear Event as an `IObservable&lt;Void&gt;`.</returns>
        public IObservable<Void> ObserveClear()
        {
            if (Cleared == null)
            {
                throw new Exception("You must assign a Cleared event in order to observe when clearing the collection.");
            }

            return new ObservableVoidEvent(Cleared.Register, Cleared.Unregister);
        }

        #endregion // Observable

        void PropogateAdded(AtomBaseVariable baseVariable)
        {
            if (_added == null) return;

            _added.Raise(baseVariable);
        }

        void PropogateRemoved(AtomBaseVariable baseVariable)
        {
            if (_removed == null) return;

            _removed.Raise(baseVariable);
        }

        void PropogateCleared()
        {
            if (_cleared == null) return;

            _cleared.Raise();
        }
    }
}