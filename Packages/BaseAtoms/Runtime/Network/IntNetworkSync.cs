#if UNITY_NETCODE
using System;

namespace UnityAtoms.BaseAtoms.Network
{
    /// <summary>
    /// Network sync for reference of type `int`.
    /// </summary>
    [Serializable]
    public sealed class IntNetworkSync : AtomVariableNetworkSync<
        int,
        IntPair,
        IntConstant,
        IntVariable,
        IntEvent,
        IntPairEvent,
        IntIntFunction,
        IntVariableInstancer,
        IntReference> { }
}
#endif
