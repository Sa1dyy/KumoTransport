using Piranha.AttributeBuilder;
using Piranha.Extend;
using Piranha.Extend.Fields;
using Piranha.Models;

namespace KumoTransport.Models
{
  [PageType(Title = "Investors Page")]
  [ContentTypeRoute(Title = "Investors", Route = "/investors")]
  public class InvestorPage : Page<InvestorPage>
  {
    // Každá sekce má jen název a technický identifikátor (slug)
    [Region(Title = "Sekce", ListTitle = "Title")]
    public IList<SectionItem> Sections { get; set; }

    // Dokumenty s referencí na sekci přes její ID (string)
    [Region(Title = "Dokumenty", ListTitle = "Title")]
    public IList<DocumentItem> Documents { get; set; }

    [Region(Title = "Program Nadpis")]
    public LocalizedText ProgramHeader { get; set; }

    [Region(Title = "Program Popis")]
    public LocalizedText ProgramDescription { get; set; }

    [Region(Title = "CTA Nadpis")]
    public LocalizedText CtaHeader { get; set; }

    [Region(Title = "CTA Text")]
    public LocalizedText CtaText { get; set; }

    [Region(Title = "CTA Kontakt 1 – Jméno")]
    public LocalizedText CtaContact1Name { get; set; }

    [Region(Title = "CTA Kontakt 1 – Role")]
    public LocalizedText CtaContact1Role { get; set; }

    [Region(Title = "CTA Kontakt 2 – Jméno")]
    public LocalizedText CtaContact2Name { get; set; }

    [Region(Title = "CTA Kontakt 2 – Role")]
    public LocalizedText CtaContact2Role { get; set; }

    [Region(Title = "Zpět tlačítko")]
    public LocalizedText BackButtonText { get; set; }

    [Region(Title = "Nadpis stránky")]
    public LocalizedText PageTitle { get; set; }
  }

  [BlockType(Name = "Sekce", Category = "Obsah")]
  public class SectionItem : Block
  {
    [Field(Title = "Kód sekce", Description = "Unikátní ID sekce, např. 'finance'")]
    public StringField SectionKey { get; set; }

    [Field(Title = "Název sekce")]
    public StringField Title { get; set; }
  }

  [BlockType(Name = "Dokument", Category = "Obsah")]
  public class DocumentItem : Block
  {
    [Field(Title = "Název dokumentu")]
    public StringField Title { get; set; }

    [Field(Title = "Soubor")]
    public DocumentField File { get; set; }

    [Field(Title = "Sekce (ID)", Description = "Zadej ID sekce, do které dokument patří")]
    public StringField SectionId { get; set; }
  }
}