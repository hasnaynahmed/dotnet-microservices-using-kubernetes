using System.ComponentModel.DataAnnotations;

namespace CommandsService.Dto
{
    public class CommandCreateDto
    {
        [Required]
        public string HowTo { get; set; }
        [Required]
        public string CommandLine { get; set; }
    }
}