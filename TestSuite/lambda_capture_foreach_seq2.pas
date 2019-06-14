type
  t1 = class
    s := Seq&<byte>(2,2,2);
    procedure p1;
    begin
      var p: procedure := ()->
      begin
        foreach var tsk in s do 
          Assert(tsk=2)
      end;
      p;
    end;
  end;

begin 
  t1.Create.p1
end.