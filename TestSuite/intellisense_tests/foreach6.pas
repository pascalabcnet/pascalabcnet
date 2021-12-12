//!semantic
{$reference system.windows.forms.dll}
begin
  var coll: system.windows.forms.ListBox.SelectedIndexCollection;
  foreach var a in coll do
    a{@var a: Object;@}.IsEven.print;

end.