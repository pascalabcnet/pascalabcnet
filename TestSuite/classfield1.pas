type
  cls = class
    class i: integer := 3;
    j: integer := 4;
    class obj: object := new object;
  end;
    
begin
  assert(cls.i = 3);
  assert(cls.obj <> nil);
  var obj := new cls;
  assert(obj.j = 4);
end.