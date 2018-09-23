type
  t1=class
    class v: byte;
    class property a: byte read v
    write 
    begin
      v := value;
    end;
  end;

begin
  t1.a := 1;
  Assert(t1.a=1);
end.