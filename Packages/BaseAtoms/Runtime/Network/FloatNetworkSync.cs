#if UNITY_NETCODE
using System;

namespace UnityAtoms.BaseAtoms.Network
{
    /// <summary>
    /// Network sync for reference of type `float`.
    /// </summary>
    [Serializable]
    public sealed class FloatNetworkSync : AtomVariableNetworkSync<
        float,
        FloatPair,
        FloatConstant,
        FloatVariable,
        FloatEvent,
        FloatPairEvent,
        FloatFloatFunction,
        FloatVariableInstancer,
        FloatReference> { }
}
#endif
