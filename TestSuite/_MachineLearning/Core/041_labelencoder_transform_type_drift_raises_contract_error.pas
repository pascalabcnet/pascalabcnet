uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var trainDf := new DataFrame;
  trainDf.AddStrColumn('City', Arr('Msk', 'Spb', 'Msk'));
  trainDf := trainDf.SetCategorical(['City']);

  var testDf := new DataFrame;
  testDf.AddIntColumn('City', Arr(1, 2, 3));
  testDf := testDf.SetCategorical(['City']);

  var enc := new OrdinalEncoder('City');
  enc.Fit(trainDf);

  CheckRaises(procedure -> begin
    var res := enc.Transform(testDf);
  end, 'LabelEncoder must raise a clear contract error on column type drift');
end.
