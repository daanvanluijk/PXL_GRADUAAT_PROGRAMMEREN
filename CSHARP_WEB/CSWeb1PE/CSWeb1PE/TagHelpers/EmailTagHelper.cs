using Microsoft.AspNetCore.Razor.TagHelpers;

namespace CSWeb1PE.TagHelpers
{
    [HtmlTargetElement("email")]
    public class EmailTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "p";
        }
    }

}
