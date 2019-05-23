type
  TSome = auto class
  public 
    auto property A: byte;
    auto property B: object;
  end;

begin
  var node := new TSome(1, nil);
  assert(node.A = 1);
  assert(node.B = nil);
end.