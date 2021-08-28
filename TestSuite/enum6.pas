begin
  var a := System.Globalization.UnicodeCategory.Format;
  assert(a.GetType().FullName = 'System.Globalization.UnicodeCategory');
  assert(System.Globalization.UnicodeCategory.Format(typeof(System.Globalization.UnicodeCategory), a, 'd') = '15');
end.