type
  My<T> = class
  public 
    _a: T;
  
  private 
    procedure Deconstruct(var a: T);
    begin
      Println('Instance');
      a := _a;
    end;
  end;

procedure Deconstruct<T>(self: My<T>; var a: T);
begin
  Println('Ext');
  a := self._a;
end;

begin
  var l := new My<List<integer>>;
  l._a := Arr(1, 2, 3).ToList;
  match l with
    My<real>(a): Assert(false);
    My<integer>(a): Assert(false);
    My<List<integer>>(a): Assert(a[0] = 1);
  end;
end.