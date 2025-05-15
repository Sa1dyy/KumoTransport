using KumoTransport.Models;
using Piranha.AttributeBuilder;
using Piranha.Extend;
using Piranha.Extend.Fields;
using Piranha.Models;

[PageType(Title = "Kumo Home Page", UseBlocks = true)]
[ContentTypeRoute(Title = "Default", Route = "/kumohomepage")]
public class KumoHomePage : Page<KumoHomePage>
{

  [Region(Title = "Odkaz na stránku pro investory")]
  public PageField InvestorsPage { get; set; }


  // === DOOR REGION ===

  [Region(Title = "Door Text", Description = "Hlavní text zobrazovaný po kliknutí na dveře (About Us)")]
  public LocalizedText DoorText { get; set; }

  [Region(Title = "Door Title Label", Description = "Textový nadpis vlevo vedle hlavního textu (např. 'Services')")]
  public LocalizedText DoorTitleLabel { get; set; }

  [Region(Title = "Experience Stat – Value", Description = "Hodnota statistiky zkušeností (např. '10+')")]
  public LocalizedText ExperienceValue { get; set; }

  [Region(Title = "Experience Stat – Label", Description = "Popisek pod hodnotou (např. 'více jak 10 let zkušeností')")]
  public LocalizedText ExperienceLabel { get; set; }

  [Region(Title = "Shipments Stat – Value", Description = "Hodnota statistik zásilek (např. '700 000+')")]
  public LocalizedText ShipmentsValue { get; set; }

  [Region(Title = "Shipments Stat – Label", Description = "Popisek pod hodnotou zásilek")]
  public LocalizedText ShipmentsLabel { get; set; }

  // === TRUNK REGION ===

  [Region(Title = "Trunk Text", Description = "Text zobrazovaný po kliknutí na kufr (Services)")]
  public LocalizedText TrunkText { get; set; }


  [Region(Title = "Trunk Title Label", Description = "Nadpis zobrazovaný nad trunk textem")]
  public LocalizedText TrunkTitle { get; set; }

  // === HOOD REGION ===

  [Region(Title = "Hood Text", Description = "Text zobrazovaný po kliknutí na kapotu (Contacts)")]
  public LocalizedText HoodText { get; set; }

  [Region(Title = "Contact Email", Description = "Emailový kontakt zobrazovaný v části hood")]
  public LocalizedText ContactEmail { get; set; }

  [Region(Title = "Contact Phone", Description = "Telefonní kontakt zobrazovaný v části hood")]
  public LocalizedText ContactPhone { get; set; }

  [Region(Title = "Contact 1 Name", Description = "Jméno prvního kontaktního pracovníka")]
  public LocalizedText Contact1Name { get; set; }

  [Region(Title = "Contact 1 Role", Description = "Role prvního kontaktního pracovníka")]
  public LocalizedText Contact1Role { get; set; }

  [Region(Title = "Contact 2 Name", Description = "Jméno druhého kontaktního pracovníka")]
  public LocalizedText Contact2Name { get; set; }

  [Region(Title = "Contact 2 Role", Description = "Role druhého kontaktního pracovníka")]
  public LocalizedText Contact2Role { get; set; }

  // === MENU / BUTTONS / FOOTER ===

  [Region(Title = "Menu – About Us", Description = "Text pro menu odkazující na About Us (door)")]
  public LocalizedText MenuAbout { get; set; }

  [Region(Title = "Menu – Services", Description = "Text pro menu odkazující na Services (trunk)")]
  public LocalizedText MenuServices { get; set; }

  [Region(Title = "Menu – Contact", Description = "Text pro menu odkazující na Contact (hood)")]
  public LocalizedText MenuContact { get; set; }

  [Region(Title = "Menu – Investors", Description = "Text pro menu odkazující na Investors")]
  public LocalizedText MenuInvestors { get; set; }

  [Region(Title = "Investor Button Text", Description = "Text v tlačítku PRO INVESTORY")]
  public LocalizedText InvestorsButtonText { get; set; }

  [Region(Title = "Back Button Text", Description = "Text pro tlačítko zpět (např. 'Back to depo')")]
  public LocalizedText BackButtonText { get; set; }

  [Region(Title = "Menu Close Text", Description = "Text pro zavření menu (např. 'Close')")]
  public LocalizedText MenuCloseText { get; set; }

  [Region(Title = "Footer – Company Name", Description = "Název společnosti ve footeru")]
  public LocalizedText FooterCompanyName { get; set; }

  [Region(Title = "Footer – Address", Description = "Adresa ve footeru")]
  public LocalizedText FooterAddress { get; set; }

  [Region(Title = "Footer – IČO/DIČ", Description = "IČO a DIČ zobrazené ve footeru")]
  public LocalizedText FooterIdentification { get; set; }

  [Region(Title = "Footer – Email", Description = "Kontaktní email ve footeru")]
  public LocalizedText FooterEmail { get; set; }

  [Region(Title = "Footer – Phone", Description = "Kontaktní telefon ve footeru")]
  public LocalizedText FooterPhone { get; set; }

  [Region(Title = "Footer – On Behalf Of", Description = "Text 'On behalf of' pod logem UPS")]
  public LocalizedText FooterText { get; set; }
}