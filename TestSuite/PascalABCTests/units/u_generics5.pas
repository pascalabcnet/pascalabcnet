unit u_generics5;

function f: byte;
begin
  
  var f: ()->byte := ()->byte(2);
  var t := System.Threading.Tasks.Task&<byte>.Run(f);
  t.Wait;
  Result := t.Result;
end;

end.