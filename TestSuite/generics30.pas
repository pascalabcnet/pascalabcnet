type
  TErr<T> = class;
  
  TBase = abstract class
    public static procedure p1<T>(temp: TErr<T>);
    
  end;
  
  TErr<T> = class(TBase) end;
  
static procedure TBase.p1<T>(temp: TErr<T>);
begin
  var q: TErr<T> := new TErr<T>;
  var a1: TBase := q;
  var a2: TBase := q as TBase;
  assert(a2 <> nil);
  q := a1 as TErr<T>;
  assert(q <> nil);
end;

begin 
  TBase.p1&<integer>(nil);
end.