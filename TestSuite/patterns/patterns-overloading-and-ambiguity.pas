type
  My<T> = class
  public 
    _a: T;
    
    procedure Deconstruct(var a: T);
    begin
      a := _a;
    end;
  end;

procedure Deconstruct<T>(self: My<T>; var a: T); extensionmethod;
begin
  Assert(false);
  a := self._a;
end;

procedure Deconstruct<T>(self: My<T>; var a: object); extensionmethod;
begin
  a := self._a;
end;

procedure Deconstruct<T>(self: My<T>; var a: object; var b: object); extensionmethod;
begin
  Assert(false);
  a := self._a;
end;

begin
  var l := new My<List<integer>>; 
  l._a := Arr(1, 2, 3).ToList;
  match l as object with
    My<real>(a: real): Assert(false);
    My<integer>(a: integer): Assert(false);
    My<List<integer>>(a: object): ;
    else Assert(false);
  end;
end.