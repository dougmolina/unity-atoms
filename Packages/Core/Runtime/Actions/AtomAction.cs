namespace UnityAtoms
{
    /// <summary>
    /// Base abstract class for Actions. Inherits from `BaseAtom`.
    /// </summary>
    public abstract class AtomAction : BaseAtom, ICallable
    {
        /// <summary>
        /// Perform the Action.
        /// </summary>
        public virtual void Do() { }
        public void Call() => Do();
    }

    /// <summary>
    /// Generic abstract base class for Actions. Inherits from `AtomAction`.
    /// </summary>
    /// <typeparam name="T1">The type for this Action.</typeparam>
    public abstract class AtomAction<T1> : AtomAction, IValueCallable
    {
        /// <summary>
        /// Perform the Action.
        /// </summary>
        /// <param name="value">The first parameter.</param>
        public virtual void Do(T1 value) => base.Do();
        public void CallWithValue(object value) => Do((T1) value);
    }
}
