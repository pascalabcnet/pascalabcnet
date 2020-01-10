//nopabcrtl
{$includenamespace 'namespaces/nsyield1.pas'}

uses nsyield1;
begin
  assert(T1.Seq().ToArray()[0] = 1);
end.