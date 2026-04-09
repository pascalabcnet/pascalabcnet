/// Вспомогательные функции для ML.
///
/// Содержит утилиты, используемые в нескольких модулях,
/// не привязанные к DataFrame или конкретным моделям.
unit MLUtilsABC;

interface

uses LinearAlgebraML;

/// Преобразует вектор меток классов в массив целых чисел.
/// Используется при визуализации и других задачах,
///   где метки должны быть представлены как 0,1,2,...
/// Значения округляются функцией Round, чтобы устранить
///   возможные небольшие численные ошибки 
function LabelsToInts(y: Vector): array of integer;

/// Преобразует массив целых меток в Vector.
function IntsToLabels(a: array of integer): Vector;

implementation

uses MLExceptions;

const
  ER_LABELS_NULL =
    'y не может быть nil!!y cannot be nil';
  ER_LABELS_ARRAY_NULL =
    'labels не может быть nil!!labels cannot be nil';

function LabelsToInts(y: Vector): array of integer;
begin
  if y = nil then
    ArgumentNullError(ER_LABELS_NULL, 'y');

  Result := new integer[y.Length];

  for var i := 0 to y.Length - 1 do
    Result[i] := Round(y[i]);
end;

function IntsToLabels(a: array of integer): Vector;
begin
  if a = nil then
    ArgumentNullError(ER_LABELS_ARRAY_NULL, 'a');

  Result := new Vector(a);
end;

end.