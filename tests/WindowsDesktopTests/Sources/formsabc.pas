uses FormsABC;
begin
  var b := new Button('Test');
  System.Windows.Forms.Application.Idle += (s,e) -> begin System.Windows.Forms.Application.Exit; end;
  System.Console.WriteLine('DESKTOP_OK');
end.
