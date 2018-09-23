begin
  assert(typeof(System.Collections.Generic.Dictionary<,>).FullName = 'System.Collections.Generic.Dictionary`2');
  assert(typeof(List<>).FullName = 'System.Collections.Generic.List`1');
  assert(typeof(System.Collections.Generic.Dictionary<,{}>).FullName = 'System.Collections.Generic.Dictionary`2');
  assert(typeof(List<{}>).FullName = 'System.Collections.Generic.List`1');
  assert(typeof(List<integer>).FullName.StartsWith('System.Collections.Generic.List`1[[System.Int32'));
end.