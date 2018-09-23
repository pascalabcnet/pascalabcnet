type
  
  r1 = record
    
    public X, Y: integer;
    
    public constructor create(X, Y: integer);
    begin
      self.X := X;
      self.Y := Y;
    end;
    
    public function f1 := self.X;
    
    class function GetR1 := new r1(3, 4);
  
  end;

function GetR1 := new r1(1, 2);

procedure test;
function NestedGetR1 := new r1(1, 2);
begin
  assert(NestedGetR1.f1 = 1);
end;

begin
  assert(GetR1.f1 = 1);
  assert(r1.GetR1.f1 = 3);
  test;
end.