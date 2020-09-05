unit NUnitABC;

{$reference nunit.framework.dll}
uses NUnit.Framework;

procedure InitPABCSystem;
begin
  __InitPABCSystem;
end;

type 
  SetUpAttribute = NUnit.Framework.SetUpAttribute;
  TestAttribute = NUnit.Framework.TestAttribute;
  TestCaseAttribute = NUnit.Framework.TestCaseAttribute;
  Assert = NUnit.Framework.Assert;

begin
  
end.