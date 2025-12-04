type MyEnum = (red,green);
begin
  var l: (A,B,C);
  l := l.A{@@};
  MyEnum.green{@const green: (red,green) = green;@};
end.