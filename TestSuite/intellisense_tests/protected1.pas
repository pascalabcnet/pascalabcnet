{$reference System.Windows.Forms.dll}
type MyButton = class(System.Windows.Forms.Button)
constructor;
begin
  self.WndProc{@procedure Button.WndProc(var m: Message); virtual;@}();
end;
end;

begin
  var b: System.Windows.Forms.Button;
  b.WndProc{@@}();
end.