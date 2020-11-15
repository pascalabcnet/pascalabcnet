function operator or<T2,T1>(f: byte -> T2; g: real -> T1); extensionmethod := 0;
function operator or<T1,T2>(f: byte -> T2; g: real -> T1); extensionmethod := 0;

begin
  var f: byte->word;
  var g: real->byte;
  var fg := f or g;
end.