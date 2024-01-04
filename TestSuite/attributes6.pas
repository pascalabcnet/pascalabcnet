type
  [System.AttributeUsage(System.AttributeTargets.ReturnValue)]
  TestAttribute = sealed class(System.Attribute)
    
  end;

procedure dummy := exit;
  
[Result: Test]
function f1 := 0;

begin
  var mi := System.Type.GetType('attributes6.Program').GetMethod('f1');
  var attrs := mi.ReturnTypeCustomAttributes.GetCustomAttributes(true);
  assert((attrs.Length = 1) and (attrs[0].GetType().FullName = 'attributes6.TestAttribute')); 
end.