begin
  var p: procedure := procedure()->exit;
  var s := $'{Seq(0).Select(i->i).First}';
  assert(s = '0');
end.