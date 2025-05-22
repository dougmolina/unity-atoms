#if UNITY_NETCODE
using System;
using UnityAtoms.BaseAtoms.Network;

namespace UnityAtoms.BaseAtoms.Network
{
    /// <summary>
    /// Network sync for reference of type `bool`.
    /// </summary>
    [Serializable]
    public sealed class BoolNetworkSync : AtomVariableNetworkSync<
        bool,
        BoolPair,
        BoolConstant,
        BoolVariable,
        BoolEvent,
        BoolPairEvent,
        BoolBoolFunction,
        BoolVariableInstancer,
        BoolReference> { }
}
#endif
