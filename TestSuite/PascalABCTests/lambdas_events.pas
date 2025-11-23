var i: integer;

procedure test;
begin
  i := 1;
end;
type
  t1 = class
    
    public event Elap: procedure;
    
    procedure p1(b:byte);
    begin
      
      var p: procedure := ()->
      begin
        b := 0;
        if self.Elap <> nil then
          self.Elap()
      end;
      p;
    end;
    
  end;

begin 
  var o := new t1;
  o.Elap += test;
  o.p1(2);
  assert(i=1);
end.