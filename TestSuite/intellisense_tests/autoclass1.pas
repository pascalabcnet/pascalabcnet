type
  MyClass = class
    a, b, c: integer;
    obj := new class(d := 'r', a, b{@property class.b: integer;@}, c);
  end;
Begin
  var x := new MyClass;
  x.obj.b{@property class.b: integer;@} := 2;
  var x2{@var x2: class;@} := new class(x);
  x2.x{@property class.x: MyClass;@} := nil;
End.