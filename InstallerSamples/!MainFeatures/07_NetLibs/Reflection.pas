// Отражение типов. Выводятся все члены типа DateTime
uses System,System.Reflection;

begin
  var bf := BindingFlags.Public or BindingFlags.NonPublic or BindingFlags.Instance or BindingFlags.Static;
  var t: &Type := typeof(DateTime);
  var mi := t.GetMembers(bf);
  foreach var m: MemberInfo in mi do
    Println(m);
end.
  
