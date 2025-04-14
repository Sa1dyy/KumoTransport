using Piranha.Extend;
using Piranha.Extend.Fields;

[BlockType(Name = "Car Animation", Category = "Content", Icon = "fas fa-car", Component = "car-animation-block")]
public class CarAnimationBlock : Block
{
  public ImageField CarModel { get; set; }
  public TextField AltText { get; set; }
}
