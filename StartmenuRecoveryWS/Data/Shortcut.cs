using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace StartmenuRecoveryWS.Data
{
    public class Shortcut
    {
        public Shortcut()
        {
            Approved = false;
            Status = "New";
            Hit = 1;

        }
        public int Id { get; set; }
        [Display(Name = "Nom")]
        public string LnkName { get; set; }
        [Display(Name = "Path")]
        public string LnkPath { get; set; }
        [Display(Name = "Source")]
        public string AppPath { get; set; }
        [Display(Name = "Argument")]
        public string? AppArgs { get; set; }
        public string Status { get; set; }
        public bool Approved { get; set; }
        [Display(Name = "Approved Date")]
        public DateTime? ApprovedTimestamp { get; set; }
        public int Hit { get; set; }
        [NotMapped]
        [Display(Name = "Path")]
        public string LnkPath_Decoded
        {
            get
            {
                return HttpUtility.HtmlDecode(LnkPath);
            }
        
        }
    }

}
