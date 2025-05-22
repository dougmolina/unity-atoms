#if UNITY_NETCODE
using System;
using UnityEngine;

namespace UnityAtoms.BaseAtoms.Network
{
    /// <summary>
    /// Network sync for reference of type `Vector3`.
    /// </summary>
    [Serializable]
    public sealed class Vector3NetworkSync : AtomVariableNetworkSync<
        Vector3,
        Vector3Pair,
        Vector3Constant,
        Vector3Variable,
        Vector3Event,
        Vector3PairEvent,
        Vector3Vector3Function,
        Vector3VariableInstancer,
        Vector3Reference> { }
}
#endif
