using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutomationSystem.Areas.Identity.Data
{
    public class Order
    {
        public int Id { get; set; }

        [ForeignKey("AutomationSystemUser")]
        public string ClientId { get; set; }
        public AutomationSystemUser AutomationSystemUser { get; set; }

        [ForeignKey("Technik")]
        public int TechnikId { get; set; }
        public Technik Technik { get; set; }

        [ForeignKey("FixCategory")]
        public int? FixCategoryId { get; set; }
        public FixCategory FixCategory { get; set; }

        public DateTime data { get; set; }

        public bool Status { get; set; }

        public decimal? Price { get; set; }

        public string? MasterId { get; set; }


    }
}
