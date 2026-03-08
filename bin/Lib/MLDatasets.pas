unit MLDatasets;

interface

uses DataFrameABC, LinearAlgebraML;

type
  Datasets = static class
  public
    // --- synthetic datasets (Matrix + Vector)
    
    /// Генерирует k кластеров (blobs) для задач классификации и кластеризации
    static function MakeBlobs(
      n: integer := 300;
      centers: integer := 3;
      seed: integer := 1): (Matrix, Vector);
    
    /// Генерирует датасет "две луны" для демонстрации нелинейной классификации и кластеризации
    static function MakeMoons(
      n: integer := 300;
      noise: real := 0.05;
      seed: integer := 1): (Matrix, Vector);
    
    /// Генерирует синтетический датасет для задач регрессии
    static function MakeRegression(
      n: integer := 300;
      noise: real := 0.1;
      seed: integer := 1): (Matrix, Vector);
    
    /// Генерирует концентрические круги (сложная нелинейная классификация)
    static function MakeCircles(
      n: integer := 300;
      noise: real := 0.05;
      seed: integer := 1): (Matrix, Vector);
    
    /// Генерирует спиральный датасет (очень сложная нелинейная классификация)
    static function MakeSpiral(
      n: integer := 300;
      classes: integer := 2;
      seed: integer := 1): (Matrix, Vector);
    
    
    // --- DataFrame datasets (реалистичные таблицы)
    
    /// Датасет цен на квартиры (задача регрессии)
    static function RussianHousing: DataFrame;
    
    /// Датасет результатов экзамена студентов (классификация)
    static function StudentExam: DataFrame;
    
    /// Датасет банковских клиентов (классификация одобрения кредита)
    static function BankClients: DataFrame;
    
    /// Датасет поездок такси (регрессия стоимости поездки)
    static function TaxiTrips: DataFrame;
    
    /// Датасет транспортной активности пассажиров (кластеризация)
    static function MoscowTransport: DataFrame;
    
    /// Датасет интернет-покупок пользователей (классификация покупки)
    static function OnlineShopping: DataFrame;
  
  end;

implementation

static function Datasets.MakeBlobs(
      n: integer ;
      centers: integer ;
      seed: integer): (Matrix, Vector);
begin
  Result := nil;
end;

static function Datasets.MakeMoons(
      n: integer ;
      noise: real ;
      seed: integer): (Matrix, Vector);
begin
  Result := nil;
end;

static function Datasets.MakeRegression(
      n: integer ;
      noise: real ;
      seed: integer): (Matrix, Vector);
begin
  Result := nil;
end;

static function Datasets.MakeCircles(
      n: integer ;
      noise: real ;
      seed: integer): (Matrix, Vector);
begin
  Result := nil;
end;

static function Datasets.MakeSpiral(
      n: integer ;
      classes: integer ;
      seed: integer): (Matrix, Vector);
begin
  Result := nil;
end;

static function Datasets.RussianHousing: DataFrame;
begin
  Result := nil;
end;

static function Datasets.StudentExam: DataFrame;
begin
  Result := nil;
end;

static function Datasets.BankClients: DataFrame;
begin
  Result := nil;
end;

static function Datasets.TaxiTrips: DataFrame;
begin
  Result := nil;
end;

static function Datasets.MoscowTransport: DataFrame;
begin
  Result := nil;
end;

static function Datasets.OnlineShopping: DataFrame;
begin
  Result := nil;
end;


end.