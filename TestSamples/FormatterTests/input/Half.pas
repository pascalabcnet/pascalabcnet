unit Half;

interface

uses Halfs, GraphABC;

type TClass = class
procedure Test;
end;

procedure CutHalf(var s: string);

implementation

uses Halfs;

procedure TClass.Test;
begin
Line('aaa');  
Line(0,0,100,100);
//Test(nil);
end;

procedure CutHalf(var s: string);
begin
  //Assert(s.Length <> 0);
  Line(2,2,10,100);
  SetLength(s, s.Length div 2);
end;

end.