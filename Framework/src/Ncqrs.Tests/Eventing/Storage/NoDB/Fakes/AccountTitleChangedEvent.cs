using System;
using Ncqrs.Eventing.Sourcing;

namespace Ncqrs.Eventing.Storage.NoDB.Tests.Fakes
{
    [Serializable]
    public class AccountTitleChangedEvent : SourcedEntityEvent
    {
        public AccountTitleChangedEvent()
        {
        }

        public AccountTitleChangedEvent(string newTitle)
        {
            NewTitle = newTitle;
        }

        public string NewTitle { get; set; }


        public override bool Equals(object obj)
        {
            var other = obj as AccountTitleChangedEvent;
            if (other == null) return false;
            bool result = EntityId.Equals(other.EntityId) &&
                          NewTitle.Equals(other.NewTitle);
            return result;
        }
    }
}
