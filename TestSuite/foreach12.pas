var c := 0;

type
  t1 = class(IEnumerable<byte>, IEnumerator<byte>)
    
    public function GetEnumerator: IEnumerator<byte> := self;
    function System.Collections.IEnumerable.GetEnumerator: System.Collections.IEnumerator := self;
    
    public function MoveNext := false;
    
    public property Current: byte read 0;
    property System.Collections.IEnumerator.Current: object read 0;
    
    public procedure Reset := exit;
    
    public procedure Dispose := c += 1;
    
  end;
  
function f1: sequence of byte;
begin
  yield sequence new t1;
  foreach var x in new t1 do
    yield x;
  foreach var x in new t1 do ;
end;

begin
  foreach var x in new t1 do ;
  Assert(c = 1);
end.