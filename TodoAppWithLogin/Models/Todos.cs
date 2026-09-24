using System.ComponentModel.DataAnnotations;

namespace TodoAppWithLogin.Models
{
    public class Todos
    {
        [Key]
        public int Id { get; set; }

        public string Description { get; set; }

        public bool IsComplete { get; set; } = false;

        // Foreign key
        public string UserId { get; set; }

        //Navigation properties
        public Users User { get; set; }

        public DateTime? DueDate { get; set; }   // nullable — due date is optional

        public bool ReminderSent { get; set; } = false;   // To know if the reminder email has been sent for this todo item
    }
}
