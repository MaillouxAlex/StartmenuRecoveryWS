using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StartmenuRecoveryWS.Data
{
    public class ShortcutRestored
    {
        [NotMapped]
        public int Id { get; set; }
        [Display(Name = "Nom")]
        [NotMapped]
        public string? LnkName { get; set; }
        [NotMapped]
        [Display(Name = "Path")]
        public string? LnkPath { get; set; }
    }
}
