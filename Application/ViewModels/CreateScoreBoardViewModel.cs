using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Application.ViewModels
{
    public class CreateScoreBoardViewModel
    {
        public string Name { get; set; }

        public int GameId { get; set; }

        public List<SelectListItem> Games { get; set; }
    }
}
