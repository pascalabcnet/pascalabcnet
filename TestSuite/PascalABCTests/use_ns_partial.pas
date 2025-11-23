//nopabcrtl
{$includenamespace 'namespaces/nspartial1.pas'}
{$includenamespace 'namespaces/nspartial2.pas'}

uses nspartial;
begin
  var o := new T;
  o.Test1;
  o.Test2;
  assert(o.i = 2);
end.