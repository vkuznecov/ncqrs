using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Newtonsoft.Json.Linq;

namespace Ncqrs.Eventing.Storage.Serialization
{
    /// <summary>
    /// Aggregates multiple type specific event converters into a single
    /// <see cref="IEventConverter"/>
    /// </summary>
    /// <seealso cref="NullEventConverter"/>
    public class EventConverter : IEventConverter
    {
		private readonly Dictionary<Type, IEventConverter> _converters;
        private readonly IEventTypeResolver _typeResolver;


        /// <summary>
        /// Initializes a new instance of the <see cref="EventConverter"/> class with a given type resolver.
        /// </summary>
        /// <param name="typeResolver">The type resolver to use when looking up event names for a specific event type.</param>
        /// <exception cref="ArgumentNullException"><paramref name="typeResolver"/> is <value>null</value>.</exception>
        public EventConverter(IEventTypeResolver typeResolver)
        {
            Contract.Requires<ArgumentNullException>(typeResolver != null, "typeResolver");

			_converters = new Dictionary<Type, IEventConverter>();
            _typeResolver = typeResolver;
        }


        /// <summary>
        /// Upgrades an event to the latest version.
        /// </summary>
        /// <remarks>
        /// <para>This checks to see if an <see cref="IEventConverter"/> has been added for <see cref="StoredEvent{T}.EventName"/>.</para>
        /// <para>If there is an event converter for this event then that converter will be used to upgrade the event.</para>
        /// <para>If no converter has been added for this event then the event will be left un-modified.</para>
        /// </remarks>
        /// <param name="theEvent">The event to be upgraded.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="theEvent"/> is <value>null</value>.</exception>
        /// <seealso cref="AddConverter"/>
        public void Upgrade(StoredEvent<JObject> theEvent)
        {
            IEventConverter converter;
            Type eventType = _typeResolver.ResolveType(theEvent.EventName);
			if (_converters.TryGetValue(eventType, out converter))
                converter.Upgrade(theEvent);
        }


        /// <summary>
        /// Adds a converter for the specified event to be used when upgrading events.
        /// </summary>
        /// <remarks>
        /// <para>This uses the type resolver to look up the name of the event.</para>
        /// <para>You do not need to add a converter for every event, only those that require upgrading.
        /// If an event has no converter added it will be left un-modified.</para>
        /// </remarks>
        /// <param name="eventType">The event type the <paramref name="converter"/> handles.</param>
        /// <param name="converter">The converter for the event specified by <paramref name="eventType"/>.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="eventType"/> or <paramref name="converter"/> is <value>null</value>.</exception>
        /// <exception cref="ArgumentException">If a converter for <paramref name="eventType"/> has already been added.</exception>
        public void AddConverter(Type eventType, IEventConverter converter)
        {
            Contract.Requires<ArgumentNullException>(eventType != null, "eventType");
            Contract.Requires<ArgumentNullException>(converter != null, "converter");

			ThrowIfEventExists(eventType);
			_converters.Add(eventType, converter);
        }


        private void ThrowIfEventExists(Type eventType)
		{
			if (_converters.ContainsKey(eventType))
			{
				string message = string.Format("There is already a converter for event '{0}'.", eventType.AssemblyQualifiedName);
				throw new ArgumentException(message, "eventType");
             }
         }
    }
}
