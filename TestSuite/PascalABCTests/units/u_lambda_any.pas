unit u_lambda_any;
{$reference WindowsBase.dll}

function f: integer;
begin
  var d := System.Windows.Threading.Dispatcher.CurrentDispatcher;
  var res := d.Invoke(()->
  begin
    Result := 1;
  end);
  Result := res;
end;

end.