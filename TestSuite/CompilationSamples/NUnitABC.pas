unit NUnitABC;

{$reference nunit.framework.dll}
uses NUnit.Framework;

procedure InitPABCSystem;
begin
  __InitPABCSystem;
  System.Console.OutputEncoding := Encoding.GetEncoding(866);
end;

type 
  CombinatorialAttribute = NUnit.Framework.CombinatorialAttribute;
  PairwiseAttribute = NUnit.Framework.PairwiseAttribute;
  IgnoreAttribute = NUnit.Framework.IgnoreAttribute;
  OrderAttribute = NUnit.Framework.OrderAttribute;
  SetUpAttribute = NUnit.Framework.SetUpAttribute;
  TearDownAttribute = NUnit.Framework.TearDownAttribute;
  TestAttribute = NUnit.Framework.TestAttribute;
  TestCaseAttribute = NUnit.Framework.TestCaseAttribute;
  TimeoutAttribute = NUnit.Framework.TimeoutAttribute;
  RandomAttribute = NUnit.Framework.RandomAttribute;
  RangeAttribute = NUnit.Framework.RangeAttribute;
  RepeatAttribute = NUnit.Framework.RepeatAttribute;
  TestFixtureAttribute = NUnit.Framework.TestFixtureAttribute;
  ValuesAttribute = NUnit.Framework.ValuesAttribute;

  Assert = NUnit.Framework.Assert;
  Assume = NUnit.Framework.Assume;
  Does = NUnit.Framework.Does;
  Has = NUnit.Framework.Has;
  &Is = NUnit.Framework.Is;
  StringAssert = NUnit.Framework.StringAssert;
  Warn = NUnit.Framework.Warn;

  AllOperator = NUnit.Framework.Constraints.AllOperator;
  AndOperator = NUnit.Framework.Constraints.AndOperator;
  BinaryOperator = NUnit.Framework.Constraints.BinaryOperator;
  CollectionOperator = NUnit.Framework.Constraints.CollectionOperator;
  Interval = NUnit.Framework.Constraints.Interval;
  OrOperator = NUnit.Framework.Constraints.OrOperator;
  WithOperator = NUnit.Framework.Constraints.WithOperator;

  Tolerance = NUnit.Framework.Constraints.Tolerance;

  Constraint = NUnit.Framework.Constraints.Constraint;

  AndConstraint = NUnit.Framework.Constraints.AndConstraint;
  AnyOfConstraint = NUnit.Framework.Constraints.AnyOfConstraint;
  BinaryConstraint = NUnit.Framework.Constraints.BinaryConstraint;
  CollectionConstraint = NUnit.Framework.Constraints.CollectionConstraint;
  EndsWithConstraint = NUnit.Framework.Constraints.EndsWithConstraint;
  EqualConstraint = NUnit.Framework.Constraints.EqualConstraint;
  FalseConstraint = NUnit.Framework.Constraints.FalseConstraint;
  GreaterThanConstraint = NUnit.Framework.Constraints.GreaterThanConstraint;
  GreaterThanOrEqualConstraint = NUnit.Framework.Constraints.GreaterThanOrEqualConstraint;
  LessThanConstraint = NUnit.Framework.Constraints.LessThanConstraint;
  LessThanOrEqualConstraint = NUnit.Framework.Constraints.LessThanOrEqualConstraint;
  NotConstraint = NUnit.Framework.Constraints.NotConstraint;
  OrConstraint = NUnit.Framework.Constraints.OrConstraint;
  RangeConstraint = NUnit.Framework.Constraints.RangeConstraint;
  StartsWithConstraint = NUnit.Framework.Constraints.StartsWithConstraint;
  StringConstraint = NUnit.Framework.Constraints.StringConstraint;
  SubStringConstraint = NUnit.Framework.Constraints.SubStringConstraint;
  TrueConstraint = NUnit.Framework.Constraints.TrueConstraint;
   
  AssertionException = NUnit.Framework.AssertionException;

begin
  
end.