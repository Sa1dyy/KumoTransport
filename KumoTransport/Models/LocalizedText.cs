using Piranha.Extend;
using Piranha.Extend.Fields;

namespace KumoTransport.Models
{
  public class LocalizedText
  {
    [Field(Title = "Česky")]
    public TextField Cs { get; set; }

    [Field(Title = "Deutsch")]
    public TextField De { get; set; }
  }
}
