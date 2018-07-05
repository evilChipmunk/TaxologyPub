using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

// This base class comes from Jimmy Bogard, but with support of inheritance
// http://grabbagoft.blogspot.com/2007/06/generic-value-object-equality.html


namespace Shared.Domain
{
    public class BaseEntity
    {
        protected BaseEntity()
        {
            State = SaveState.Saved;
        }

        protected BaseEntity(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Id is required");
            }
            State = SaveState.UnSaved;
        }

        private List<IDomainEvent> events = new List<IDomainEvent>();

        public IReadOnlyList<IDomainEvent> Events
        {
            get { return events.AsReadOnly(); }
        }

        public void AddEvent(IDomainEvent domainEvent)
        {
            this.events.Add(domainEvent);
        }
        public Guid Id { get; set; }

        public SaveState State { get; protected set; }

        public void SetSaved()
        {
            State = SaveState.Saved;
        }
        public static T CreateForDelete<T>(Guid id) where T : BaseEntity, new()
        {
            var deleted = new T();
            deleted.Id = id;
            return deleted;
        }
    }


}
