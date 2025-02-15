using System;
using UnityEngine;

namespace UnityAtoms.BaseAtoms.Network
{
    /// <summary>
    /// Network sync for reference of type `Vector2`.
    /// </summary>
    [Serializable]
    public sealed class Vector2NetworkSync : AtomVariableNetworkSync<
        Vector2,
        Vector2Pair,
        Vector2Constant,
        Vector2Variable,
        Vector2Event,
        Vector2PairEvent,
        Vector2Vector2Function,
        Vector2VariableInstancer,
        Vector2Reference> { }
}
