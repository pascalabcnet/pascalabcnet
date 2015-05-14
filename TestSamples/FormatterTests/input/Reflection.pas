uses System,System.Reflection;

begin
  var bf := BindingFlags.Public or BindingFlags.NonPublic {or BindingFlags.DeclaredOnly} or BindingFlags.Instance or BindingFlags.Static;
  var t: &Type := typeof(DateTime);
  var mi := t.GetMembers(bf);
  writeln(mi.Length);
  foreach m: MemberInfo in mi do
    writeln(m);
  var mf: MethodInfo;
end.
  
