using System;
using NServiceBus;
using Ncqrs.Eventing.ServiceModel.Bus;

namespace Ncqrs.NServiceBus
{
    /// <summary>
    /// Wraps Ncqrs event to be transportable by NServiceBus.
    /// </summary>
    /// <typeparam name="T">Type of transported event.</typeparam>
    [Serializable]
    public class EventMessage<T> : EventMessage, IPublishedEvent<T>
    {
		public EventMessage() { }

		public EventMessage(IPublishableEvent evnt) : base(evnt)
    	{
    	}
        /// <summary>
        /// Gets or sets transported event.
        /// </summary>
		public new T Payload { get { return (T)base.Payload; } set { base.Payload = value; } }
    }

	[Serializable]
	public class EventMessage : IMessage, IPublishableEvent
	{
		public EventMessage() {}

		public EventMessage(IPublishableEvent evnt)
		{
			EventIdentifier = evnt.EventIdentifier;
			EventTimeStamp = evnt.EventTimeStamp;
			EventVersion = evnt.EventVersion;
			EventSourceId = evnt.EventSourceId;
			Payload = evnt.Payload;
		}

		public Guid EventIdentifier { get; set; }
		public DateTime EventTimeStamp { get; set; }
		public Version EventVersion { get; set; }
		public Guid EventSourceId { get; set; }
		public long EventSequence { get; set; }
		public Guid CommitId { get; set; }
		public object Payload { get; set; }
	}
}