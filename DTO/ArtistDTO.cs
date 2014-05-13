using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JMTI_Test.DTO
{
    public class ArtistDTO
    {

        public int ArtistId { get; set; }

        [Display(Name = "Artist Name")]
        [Required(ErrorMessage = "This field cannot be empty")]
        public string ArtistName { get; set; }

        [Display(Name = "Package Name")]
        [Required(ErrorMessage = "This field cannot be empty")]
        public string PackageName { get; set; }

        [Display(Name = "Image")]
        public HttpPostedFileBase Image { get; set; }

        [Display(Name = "Release Date")]
        [Required(ErrorMessage = "This field cannot be empty")]
        public System.DateTime ReleaseDate { get; set; }

        [Required(ErrorMessage = "This field cannot be empty")]
        [Range(1, 9999999999, ErrorMessage = "Must be number")]
        public decimal Price { get; set; }

        [Display(Name = "Sample Music")]
        public HttpPostedFileBase Sample { get; set; }
    }
}