namespace System
{
    /// <summary>
    /// From <see cref="Maxsys.Core"/>.<para/>
    /// Provides data for an event with a Guid.
    /// </summary>
    public class GuidEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets a value a Guid id.
        /// </summary>
        public Guid Value { get; }

        /// <summary>
        /// Initializes a new instance of the GuidEventArgs class with
        /// the Value property set to the given value.
        /// </summary>
        /// <param name="value"></param>
        public GuidEventArgs(Guid value)
        {
            Value = value;
        }
    }
}