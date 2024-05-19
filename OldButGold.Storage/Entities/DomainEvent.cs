﻿using System.ComponentModel.DataAnnotations;

namespace OldButGold.Forums.Storage.Entities
{
    public class DomainEvent
    {
        [Key]
        public Guid DomainEventId { get; set; }

        public DateTimeOffset EmittedAt { get; set; }

        [Required]
        public byte[] ContentBlob { get; set; }
    }
}
