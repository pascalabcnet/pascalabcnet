uses GraphWPF;

begin
  Window.IsFixedSize := True;
  Print(Window.IsFixedSize);
  Window.IsFixedSize := False;
  Print(Window.IsFixedSize);
end.