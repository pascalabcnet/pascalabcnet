var sss: string;

type
  t1 = class
    constructor;
    procedure p0 := exit;
  end;
  
  t2 = class
    constructor(f: ()->()) := f();
  end;
  
constructor t1.Create;
begin
  foreach var i in Arr(0) do
  begin
    var s := 'tt';
    new t2(()->
    begin
      var s2 := s;
      sss := s;
    end);
    
  end;
end;

begin
  t1.Create.p0;
  assert(sss = 'tt');
end.