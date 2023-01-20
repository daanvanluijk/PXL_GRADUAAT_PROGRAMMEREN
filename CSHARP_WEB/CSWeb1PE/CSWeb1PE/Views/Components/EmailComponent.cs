using Microsoft.AspNetCore.Mvc;

namespace CSWeb1PE.Views.Components
{
    [ViewComponent]
    public class EmailComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
