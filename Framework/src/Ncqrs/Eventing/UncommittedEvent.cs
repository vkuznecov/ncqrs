﻿using System;

namespace Ncqrs.Eventing
{
    /// <summary>
    /// Represents an event which has not been yet persisted.
    /// </summary>
    public class UncommittedEvent : IEvent
    {
        private readonly object _payload;
        private readonly long _eventSequence;
        private readonly Guid _eventIdentifier;
        private readonly DateTime _eventTimeStamp;
        private readonly Guid _eventSourceId;
        private readonly long _initialVersionOfEventSource;
        private readonly Version _eventVersion;

        /// <summary>
        /// Gets the initial version of event source (the version it was just after creating/retrieving from the store)
        /// </summary>
        public long InitialVersionOfEventSource
        {
            get { return _initialVersionOfEventSource; }
        }

        /// <summary>
        /// Gets the payload of the event.
        /// </summary>
        public object Payload
        {
            get { return _payload; }
        }

        /// <summary>
        /// Gets the unique identifier for this event.
        /// </summary>
        public Guid EventIdentifier
        {
            get { return _eventIdentifier; }
        }

        /// <summary>
        /// Gets the time stamp for this event.
        /// </summary>
        /// <value>a <see cref="DateTime"/> UTC value that represents the point
        /// in time where this event occurred.</value>
        public DateTime EventTimeStamp
        {
            get { return _eventTimeStamp; }
        }

        /// <summary>
        /// Gets the id of the event source that caused the event.
        /// </summary>
        /// <value>The id of the event source that caused the event.</value>
        public Guid EventSourceId
        {
            get { return _eventSourceId; }
        }

        /// <summary>
        /// Gets the event sequence number.
        /// </summary>
        /// <remarks>
        /// An sequence of events always starts with <c>1</c>. So the first event in a sequence has the <see cref="EventSequence"/> value of <c>1</c>.
        /// </remarks>
        /// <value>A number that represents the order of where this events occurred in the sequence.</value>
        public long EventSequence
        {
            get { return _eventSequence; }
        }

        public UncommittedEvent(Guid eventIdentifier, Guid eventSourceId, long eventSequence, long initialVersionOfEventSource, DateTime eventTimeStamp, object payload, Version eventVersion)            
        {
            _payload = payload;
            _eventVersion = eventVersion;
            _initialVersionOfEventSource = initialVersionOfEventSource;
            _eventSourceId = eventSourceId;
            _eventSequence = eventSequence;
            _eventIdentifier = eventIdentifier;
            _eventTimeStamp = eventTimeStamp;
        }

        public Version EventVersion
        {
            get { return _eventVersion; }
        }
    }
}