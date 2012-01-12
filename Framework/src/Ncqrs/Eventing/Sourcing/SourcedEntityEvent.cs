using System;

namespace Ncqrs.Eventing.Sourcing
{
	[Serializable]
	public abstract class SourcedEntityEvent : SourcedEvent, IEntitySourcedEvent
    {
        public static Guid UndefinedEntityId = Guid.Empty;

        /// <summary>
        /// Gets or sets the id of the entity that causes this event.
        /// </summary>
        public Guid EntityId { get; set; }

		public void ClaimEvent(Guid aggregateId, Guid entityId, long sequence = 0)
		{
			ClaimEvent(aggregateId, sequence);
			EntityId = entityId;
		}
    }
}