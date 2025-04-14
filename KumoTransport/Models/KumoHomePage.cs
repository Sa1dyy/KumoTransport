using Piranha.AttributeBuilder;
using Piranha.Extend;
using Piranha.Extend.Fields;
using Piranha.Models;

[PageType(Title = "Kumo Home Page", UseBlocks = true)]
[ContentTypeRoute(Title = "Default", Route = "/kumohomepage")]
public class KumoHomePage : Page<KumoHomePage>
{
  [Region(Title = "Main Content")]
  public TextField MainContent { get; set; }
}
