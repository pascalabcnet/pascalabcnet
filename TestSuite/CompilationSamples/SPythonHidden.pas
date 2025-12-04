{$HiddenIdents}
unit SPythonHidden;

interface

uses SPythonSystem;

function !Floor(x : real) : integer;

function !FloorDiv(x: real; y: real): integer;

function !FloorMod(x: real; y: real): real;

type !UnknownType = class
end;

// TODO: Перенести в SPythonSystemPys следующие методы
function list<T>(sq: sequence of T): SPythonSystem.list<T>;

function list<K, V>(d : dict<K, V>) : SPythonSystem.list<K>;

function &set<T>(sq: sequence of T): &set<T>;

function &set(): empty_set;

function dict<TKey, TVal>(params pairs: array of (TKey, TVal)): dict<TKey, TVal>;

function dict<TKey, TVal>(seqOfPairs: IEnumerable<System.Tuple<TKey, TVal>>) : dict<TKey, TVal>;

implementation

function list<T>(sq: sequence of T): SPythonSystem.list<T> := new SPythonSystem.list<T>(sq);

function list<K, V>(d : dict<K, V>) : SPythonSystem.list<K> := new SPythonSystem.list<K>(d.keys());

function &set<T>(sq: sequence of T): &set<T>;
begin
  Result := new &set<T>(sq);
end;

function &set(): empty_set := new empty_set;

function dict<TKey, TVal>(params pairs: array of (TKey, TVal)): dict<TKey, TVal> := new dict<TKey, TVal>(pairs);

function dict<TKey, TVal>(seqOfPairs: sequence of (TKey, TVal)): dict<TKey, TVal> := new dict<TKey, TVal>(seqOfPairs);

function !Floor(x : real) : integer := PABCSystem.Floor(x); 

function !FloorDiv(x: real; y: real): integer := PABCSystem.Floor(x / y);

function !FloorMod(x: real; y: real): real := x - PABCSystem.Floor(x / y) * y;

end.