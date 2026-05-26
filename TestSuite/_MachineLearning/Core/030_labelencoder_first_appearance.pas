uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var df := new DataFrame;
  df.AddStrColumn('Region', Arr('South', 'North', 'South', 'East'));
  df := df.SetCategorical(['Region']);

  var enc := new OrdinalEncoder('Region');
  enc.Fit(df);

  var res := enc.Transform(df);
  var labels := res.GetIntColumn('Region');

  Check(labels[0] = 0, 'First appearance of South must get code 0');
  Check(labels[1] = 1, 'First appearance of North must get code 1');
  Check(labels[2] = 0, 'Repeated South must keep code 0');
  Check(labels[3] = 2, 'First appearance of East must get code 2');
end.
