﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OldButGold.Forums.Storage.Entities
{
    public class Forum
    {
        [Key]
        public Guid ForumId { get; set; }

        [MaxLength(100)]
        public string Title { get; set; }

        [InverseProperty(nameof(Topic.Forum))]
        public ICollection<Topic> Topics { get; set; }

    }
}