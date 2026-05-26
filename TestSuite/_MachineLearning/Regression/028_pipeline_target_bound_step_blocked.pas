uses MLABC, PreprocessorABC, MLCoreABC;
uses TestHelpers in '..\TestHelpers.pas';

type
  BadTargetStep = class(IPreprocessor, IColumnBoundStep)
  private
    fColumn: string;
  public
    constructor Create(column: string);
    function Fit(df: DataFrame): IPreprocessor;
    function Transform(df: DataFrame): DataFrame;
    function FitTransform(df: DataFrame): DataFrame;
    function Clone: IPreprocessor;
    property ColumnName: string read fColumn;
  end;

constructor BadTargetStep.Create(column: string);
begin
  fColumn := column;
end;

function BadTargetStep.Fit(df: DataFrame): IPreprocessor;
begin
  Result := Self;
end;

function BadTargetStep.Transform(df: DataFrame): DataFrame;
begin
  Result := df;
end;

function BadTargetStep.FitTransform(df: DataFrame): DataFrame;
begin
  Result := df;
end;

function BadTargetStep.Clone: IPreprocessor;
begin
  Result := new BadTargetStep(fColumn);
end;

begin
  CheckRaises(procedure -> begin
    var pipe := DataPipeline.Build(
      TaskKind.tkClassification,
      'Target',
      Arr($'X'),
      new BadTargetStep('Target'),
      new LogisticRegression
    );
  end,
  'DataPipeline must reject a bound preprocessor that targets the target column');
end.
