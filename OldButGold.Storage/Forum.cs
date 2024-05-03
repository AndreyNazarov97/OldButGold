using System.ComponentModel.DataAnnotations;

namespace OldButGold.Storage
{
    public class Forum
    {
        [Key]
        public Guid ForumId { get; set; }

        public string Title { get; set; }


    }
}