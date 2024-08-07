using System;
using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Reference of type `bool`. Inherits from `EquatableAtomReference&lt;bool, BoolPair, BoolConstant, BoolVariable, BoolEvent, BoolPairEvent, BoolBoolFunction, BoolVariableInstancer, AtomCollection, AtomList&gt;`.
    /// </summary>
    [Serializable]
    public sealed class BoolReference : EquatableAtomReference<
        bool,
        BoolPair,
        BoolConstant,
        BoolVariable,
        BoolEvent,
        BoolPairEvent,
        BoolBoolFunction,
        BoolVariableInstancer>, IEquatable<BoolReference>
    {
        public BoolReference() { }

        public BoolReference(bool value) : base(value) { }

        public bool Equals(BoolReference other) { return base.Equals(other); }

        public override void SetReferenceValue(object value) { Value = value != null && (bool) value; }
    }
}
