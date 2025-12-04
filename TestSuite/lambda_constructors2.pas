type TBase = class
  o: object;
  constructor;
  begin
    o := new object;
  end;
end;

TDer = class(TBase)
  constructor;
  begin
    var obj := o;
    var f: ()->() := ()->assert(obj <> nil);
    f();
  end;
  constructor (i: integer);
  begin
    var obj := o;
    var f: ()->() := ()->assert(obj <> nil);
    f();
  end;
end;

begin
  new TDer;
  new TDer(2);
end.