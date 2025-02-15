using System;
using UnityEngine;

namespace UnityAtoms.BaseAtoms.Network
{
    /// <summary>
    /// Network sync for reference of type `Color`.
    /// </summary>
    [Serializable]
    public sealed class ColorNetworkSync : AtomVariableNetworkSync<
        Color,
        ColorPair,
        ColorConstant,
        ColorVariable,
        ColorEvent,
        ColorPairEvent,
        ColorColorFunction,
        ColorVariableInstancer,
        ColorReference> { }
}
