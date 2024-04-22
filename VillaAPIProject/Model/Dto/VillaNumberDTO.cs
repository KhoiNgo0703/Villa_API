using System.ComponentModel.DataAnnotations;

namespace VillaAPIProject.Model.Dto
{
    public class VillaNumberDTO
    {
        [Required]
        public int VillaNo { get; set; }
        public string SpecialDetails { get; set; }
        [Required]
        public int VillaID { get; set; }

        //implement Villa Prop
        public VillaDTO Villa { get; set; }
    }
}
