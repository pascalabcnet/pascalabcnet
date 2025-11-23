uses System.Drawing, GraphABC;
var br : BrushStyleType;
begin
  assert(defaultWindowWidth = 640);
  assert(defaultWindowHeight = 480);
  assert(clAquamarine = Color.Aquamarine);
  assert(GraphABC.clYellow = Color.Yellow);
  assert(VK_NumPad9 = 105);
  assert(bhVertical = HatchStyle.Vertical);
  br := bsClear;
  assert(br = BrushStyleType.bsClear);
  //InitWindow(10,20,400,300);
  Line(0,0,200,100);
  DrawCircle(100,100,20);
  
end.