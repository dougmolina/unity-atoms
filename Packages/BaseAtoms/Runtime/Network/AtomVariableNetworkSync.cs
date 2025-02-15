using System;
using Unity.Netcode;

namespace UnityAtoms.BaseAtoms.Network
{
    /// <summary>
    /// Network sync for atom references.
    /// </summary>
    [Serializable]
    public class AtomVariableNetworkSync<T, P, C, V, E1, E2, F, VI, R> : NetworkBehaviour
        where R : AtomReference<T, P, C, V, E1, E2, F, VI>
        where P : struct, IPair<T>
        where C : AtomBaseVariable<T>
        where V : AtomVariable<T, P, E1, E2, F>
        where E1 : AtomEvent<T>
        where E2 : AtomEvent<P>
        where F : AtomFunction<T, T>
        where VI : AtomVariableInstancer<V, P, T, E1, E2, F>
    {
        public BoolReference ServerIsOwner;
        public R Reference;

        private NetworkVariable<T> _networkVariable = new();

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (!ServerIsOwner || IsServer)
            {
                Reference.GetEvent<E1>().Register(OnLocalReferenceChanged);
                OnLocalReferenceChanged(Reference.Value);
            }

            if (!IsServer)
            {
                Reference.Value = _networkVariable.Value;
                _networkVariable.OnValueChanged += OnNetworkValueChanged;
            }
        }

        public override void OnNetworkDespawn()
        {
            Reference.GetEvent<E1>().Unregister(OnLocalReferenceChanged);
            _networkVariable.OnValueChanged -= OnNetworkValueChanged;
            base.OnNetworkDespawn();
        }

        private void OnLocalReferenceChanged(T value)
        {
            if (IsServer)
            {
                _networkVariable.Value = value;
            }
            else
            {
                SendValueChangeToServerRpc(value);
            }
        }

        [Rpc(SendTo.Server)]
        private void SendValueChangeToServerRpc(T value)
        {
            T previousValue = _networkVariable.Value;
            _networkVariable.Value = value;
            OnNetworkValueChanged(previousValue, value);
        }

        private void OnNetworkValueChanged(T previousValue, T newValue)
        {
            Reference.GetEvent<E1>().Unregister(OnLocalReferenceChanged);
            Reference.Value = newValue;
            Reference.GetEvent<E1>().Register(OnLocalReferenceChanged);
        }
    }
}
