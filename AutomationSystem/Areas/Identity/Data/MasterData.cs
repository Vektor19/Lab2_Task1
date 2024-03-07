using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AutomationSystem.Data;
using AutomationSystem.Observers;
namespace AutomationSystem.Areas.Identity.Data
{
    public class MasterData : IOrderObserver
    {
        public string Id { get; set; }
        public bool isNewOrdersChecked { get; set; }
        public void Update()
        {
            this.isNewOrdersChecked = false;
        }
    }
}
