/// Модуль содержит шаблоны классов
///   Stack — стек
///   Queue — очередь
///   DynArray — динамический массив
///   SimpleSet — простое множество на основе динамического массива
///   AssocArray — простой ассоциативный массив на основе динамического массива пар
///   LinkedList — двусвязный список
unit Collections;

//------------------------------------------------------------------------------
// Модуль упрощенных классов коллекций для обучения
// Версия 1.0
// Copyright (c) 2009 Белякова Ю., Михалкович С.С., Саатчи А.
//------------------------------------------------------------------------------

interface

// -------------------------------- SingleNode ---------------------------------
type
  /// Узел с одним полем связи
  SingleNode<T> = class
  private 
    /// Значение, содержащееся в узле
    fData: T;
    /// Ссылка на следующий элемент
    fNext: SingleNode<T>;
  /// <summary>
  /// Конструктор
  /// </summary>
  /// <param name="pData">Значение в узле</param>
  /// <param name="pNext">Ссылка на следующий элемент</param>
  public 
    constructor Create(pData: T; pNext: SingleNode<T>);
    begin
      fData := pData;
      fNext := pNext;
    end;
    
    property Data: T read fData write fData;
    property Next: SingleNode<T> read fNext write fNext;
  end; 

// ----------------------------------- Stack -----------------------------------
type
  /// Шаблон класса Stack
  Stack<T> = class
  private 
    /// Вершина стека
    fTop: SingleNode<T> := nil; 
  public 
    /// Создает пустой стек
    constructor Create;
    /// <summary>
    /// Кладет элемент на вершину стека
    /// </summary>
    /// <param name="x">Новый элемент</param>
    procedure Push(x: T);
    /// Возвращает значение элемента на вершине, снимая его со стека
    function Pop: T;
    /// Возвращает значение элемента на вершине стека, не снимая его
    function Top: T;
    /// Возвращает истину, если стек пуст
    function IsEmpty: boolean;
    /// Преобразует содержимое стека в строку
    function ToString: string; override;
    /// Выводит содержимое стека на консоль
    procedure Print;
    /// Выводит содержимое стека на консоль с переходом на новую строку
    procedure Println;
  end;

// ----------------------------------- Queue -----------------------------------
type
  /// Шаблон класса Queue
  Queue<T> = class
  private 
    /// Голова очереди
    head: SingleNode<T>;
    /// Хвост очереди
    tail: SingleNode<T>;
  public 
    /// Создает пустую очередь
    constructor Create;
    /// <summary>
    /// Добавляет элемент в хвост очереди
    /// </summary>
    /// <param name="x">Добавляемый элемент</param>
    procedure Enqueue(x: T);
    /// Возвращает значение элемента в голове, удаляя его из очереди
    function Dequeue: T;
    /// Возвращает значение элемента в голове очереди, не удаляя его
    function Top: T;
    /// Возвращает истину, если очередь пуста
    function IsEmpty: boolean;
    /// Преобразует содержимое очереди в строку
    function ToString: string; override;
    /// Выводит содержимое очереди на консоль
    procedure Print;
    /// Выводит содержимое очереди на консоль с переходом на новую строку
    procedure Println;
  end;

// --------------------------------- DynArray ----------------------------------
const
  /// Минимальная емкость, устанавливаемая при создании массива
  MIN_CAP = 4;
  /// Коэффициент увеличения емкости массива при её нехватке
  INC_CAP_FACTOR = 2;

type
  /// Шаблон класса DynArray [Динамический массив с автоконтролем памяти]
  DynArray<T> = class
  private 
    /// Встроенный динамический массив, содержащий данные
    fData: array of T;
    /// Размер массива
    fSize: integer;
    /// Емкость массива
    fCap: integer;
    /// Устанавливает элемент с индексом ind равным x
    procedure SetElem(index: integer; x: T);
    /// Возвращает элемент массива с индексом ind
    function GetElem(index: integer): T;
  public 
    /// Создает массив размера 0
    constructor Create;
    /// Создает массив размера size
    constructor Create(psize: integer);
    /// <summary>
    /// Выделяет новую память. Емкость увеличивается.
    /// (Если newCap меньше текущей емкости, ничего не происходит) 
    /// </summary>
    /// <param name="newCap">Новая емкость массива</param>
    procedure Reserve(newCap: integer);
    /// <summary>
    /// Устанавливает новый размер массива 
    /// </summary>
    /// <param name="newSize">Новый размер массива</param>
    procedure Resize(newSize: integer);
    /// <summary>
    /// Добавляет элемент в конец массива 
    /// </summary>
    /// <param name="x">Добавляемый элемент</param>
    procedure Add(x: T);
    /// <summary>
    /// Вставляет элемент в указанную позицию
    /// </summary>
    /// <param name="pos">Позиция, в которую вставляется элемент</param>
    /// <param name="x">Вставляемый элемент</param>
    procedure Insert(pos: integer; x: T);
    /// <summary>
    /// Удаляет элемент массива из указанной позиции
    /// </summary>
    /// <param name="pos">Позиция, из которой удаляется элемент</param>
    procedure Remove(pos: integer);
    /// <summary>
    /// Возвращает индекс первого элемента массива равного искомому
    /// или -1, если такого элемента нет
    /// </summary>
    /// <param name="x">Искомый элемент</param>
    function Find(x: T): integer;
    /// Количество элементов (размер) массива
    property Count: integer read fSize write Resize;
    /// Емкость массива 
    property Capacity: integer read fCap write Reserve;
    /// Позволяет обращаться к элементам массива по индексу 
    property Elem[index: integer]: T read GetElem write SetElem; default;
    /// Преобразует содержимое массива в строку
    function ToString: string; override;
    /// Выводит содержимое массива на консоль
    procedure Print;
    /// Выводит содержимое массива на консоль с переходом на новую строку
    procedure Println;
  end;

// -------------------------------- SimpleSet ----------------------------------
type
  /// Шаблон класса SimpleSet
  SimpleSet<T> = class 
  private 
    /// Элементы множества
    data: DynArray<T>;
  public 
    /// Создает множество
    constructor Create;
    /// <summary>
    /// Добавляет элемент во множество, если его там еще нет
    /// </summary>
    /// <param name="x">Добавляемый элемент</param>    
    procedure Add(x: T);
    /// <summary>
    /// Удаляет элемент из множества, если он там есть
    /// </summary>
    /// <param name="x">Удаляемый элемент</param>
    procedure Remove(x: T);
    /// <summary>
    /// Возвращает истину, если множество содержит элемент
    /// </summary>
    /// <param name="x">Искомый элемент</param>          
    function Contains(x: T): boolean;
    /// Преобразует содержимое массива в строку
    function ToString: string; override;
    /// Выводит содержимое массива на консоль
    procedure Print;
    /// Выводит содержимое множества на консоль с переходом на новую строку
    procedure Println;
  end;

// -------------------------------- AssocArray ---------------------------------
type
  /// Шаблон класса AssocArray 
  AssocArray<KeyType, ValueType> = class
  private 
    /// Ключи
    keys: DynArray<KeyType>;
    /// Значения, соответствующие ключам
    values: DynArray<ValueType>;
    
    /// Устанавливает значение элемента с ключом key равным value
    procedure SetElem(key: KeyType; value: ValueType);
    /// Возвращает значение элемента с ключом key
    function GetElem(key: KeyType): ValueType;
  public 
    /// Создает ассоциативный массив
    constructor Create;
    /// Позволяет обращаться к элементам массива по ключу
    property Elem[key: KeyType]: ValueType read GetElem write SetElem; default;
    /// Преобразует содержимое ассоциативного массива в строку
    function ToString: string; override;
    /// Выводит содержимое ассоциативного массива на консоль
    procedure Print;
    /// Выводит содержимое ассоциативного массива на консоль с переходом на новую строку
    procedure Println;
  end;

// ----------------------------- LinkedListNode --------------------------------
type
  /// Узел с двумя полями связи
  LinkedListNode<T> = class
  private 
    /// Значение, содержащееся в узле
    fData: T;
    /// Ссылка на предыдущий элемент
    fPrev: LinkedListNode<T>;
    /// Ссылка на следующий элемент
    fNext: LinkedListNode<T>;
  public 
    /// <summary>
    /// Создает новый узел
    /// </summary>
    /// <param name="T">Значение узла</param>
    /// <param name="Prev">Ссылка на предыдущий элемент</param>
    /// <param name="Next">Ссылка на следующий элемент</param>
    constructor Create(data: T; prev, next: LinkedListNode<T>);
    begin
      fData := data;
      fNext := next;
      fPrev := prev;
    end;
    /// Значение, содержащееся в узле
    property Value: T read fData write fData;
    /// Ссылка на предыдущий элемент — только на чтение
    property Prev: LinkedListNode<T> read fPrev;
    /// Ссылка на следующий элемент — только на чтение
    property Next: LinkedListNode<T> read fNext;
  end;


// -------------------------------- LinkedList ---------------------------------
type
  /// Двусвязный линейный список
  LinkedList<T> = class
  private 
    /// Первый элемент (голова)
    fFirst: LinkedListNode<T>;
    /// Последний элемент (хвост)
    fLast: LinkedListNode<T>;
  public 
    /// Создает пустой список
    constructor Create;
    /// Первый элемент (голова) — только на чтение
    property First: LinkedListNode<T> read fFirst;
    /// Последний элемент (хвост) — только на чтение
    property Last: LinkedListNode<T> read fLast;
    /// <summary>
    /// Добавляет элемент x в начало списка
    /// </summary>
    /// <param name="x">Добавляемый элемент</param>
    procedure AddFirst(x: T);
    /// <summary>
    /// Добавляет элемент x в конец списка
    /// </summary>
    /// <param name="x">Добавляемый элемент</param>
    procedure AddLast(x: T);
    /// Удаляет первый элемент списка
    procedure RemoveFirst();
    /// Удаляет последний элемент списка
    procedure RemoveLast();
    /// <summary>
    /// Добавляет элемент x перед node
    /// </summary>
    /// <param name="node">Ссылка на элемент, перед которым нужно добавить новый</param>
    /// <param name="x">Добавляемый элемент</param>
    procedure AddBefore(node: LinkedListNode<T>; x: T);
    /// <summary>
    /// Добавляет элемент x после node
    /// </summary>
    /// <param name="node">Ссылка на элемент, после которого нужно добавить новый</param>
    /// <param name="x">Добавляемый элемент</param>
    procedure AddAfter(node: LinkedListNode<T>; x: T);
    /// <summary>
    /// Удаляет элемент node
    /// </summary>
    /// <param name="node">Ссылка на элемент, который нужно удалить</param>
    procedure Remove(node: LinkedListNode<T>);
    /// Преобразует содержимое списка в строку
    function ToString: string; override;
    /// Выводит содержимое списка на консоль
    procedure Print;
    /// Выводит содержимое списка на консоль с переходом на новую строку
    procedure Println;
  end;

//==============================================================================
implementation

//Сообщения исключений
const
  PopNilStackExceptionMessage =				'Попытка снятия элемента с пустого стека';
  TopNilStackExceptionMessage =				'Попытка получения элемента с пустого стека';
  
  DequeueNilQueueExceptionMessage =		'Попытка удаления элемента из пустой очереди';
  TopNilQueueExceptionMessage =				'Попытка получения элемента из пустой очереди';
  
  NegativeArraySizeExceptionMessage =	'Попытка присвоить размеру массива отрицательное значение ';
  InsOutOfArrayBoundExceptionMessage = 'Попытка вставки за границей массива в позицию ';
  RemOutOfArrayBoundExceptionMessage = 'Попытка удаления за границей массива из позиции ';
  SetElemOutOfBoundExceptionMessage =	'Попытка присвоить значение элементу за границей массива с индексом ';
  GetElemOutOfBoundExceptionMessage =	'Попытка получить значение элемента за границей массива с индексом ';
  
  RemoveFromNilListExceptionMessage =	'Попытка удаления из пустого списка';
  AddNilNodeExceptionMessage =				'Параметр node является нулевой ссылкой';
  RemoveNilNodeExceptionMessage =			'Параметр node является нулевой ссылкой';

// ----------------------------------- Stack -----------------------------------
constructor Stack<T>.Create;
begin
  fTop := nil;
end;

procedure Stack<T>.Push(x: T);
begin
  fTop := new SingleNode<T>(x, fTop);
end;

function Stack<T>.Pop: T;
begin
  if IsEmpty then 
    raise new Exception(PopNilStackExceptionMessage);
  
  Result := fTop.data;
  fTop := fTop.next;  
end;

function Stack<T>.Top: T;
begin
  if IsEmpty then 
    raise new Exception(TopNilStackExceptionMessage);
  
  Result := fTop.data;
end;

function Stack<T>.IsEmpty: boolean;
begin
  Result := (fTop = nil);
end;

function Stack<T>.ToString: string;
begin
  Result := '';
  var curElem := fTop;
  while curElem <> nil do
  begin
    Result += curElem.data.ToString + ' ';
    curElem := curElem.next;
  end;
end;

procedure Stack<T>.Print;
begin
  writeln(ToString);
end;

procedure Stack<T>.Println;
begin
  writeln(ToString);
end;

// ----------------------------------- Queue -----------------------------------
constructor Queue<T>.Create;
begin
  head := nil;
  tail := nil;
end;

procedure Queue<T>.Enqueue(x: T);
begin
  if IsEmpty then
  begin
    head := new SingleNode<T>(x, nil);
    tail := head;
  end
  else
  begin
    tail.next := new SingleNode<T>(x, nil);
    tail := tail.next;  // элемент удаляется из хвоста очереди (т.е. хвостом становится следующий элемент)
  end;
end;

function Queue<T>.Dequeue: T;
begin
  if IsEmpty then 
    raise new Exception(DequeueNilQueueExceptionMessage);
  
  Result := head.data;
  head := head.next;  // элемент удаляется из головы очереди (т.е. головой становится следующий элемент)
  if head = nil then
    tail := nil;
end;

function Queue<T>.Top: T;
begin
  if IsEmpty then 
    raise new Exception(TopNilQueueExceptionMessage);
  Result := head.data;
end;

function Queue<T>.IsEmpty: boolean;
begin
  Result := (head = nil);
  if Result then
    Assert(tail = nil);
end;

function Queue<T>.ToString: string;
begin
  Result := '';
  var curElem := head;
  while curElem <> nil do
  begin
    Result += curElem.data + ' ';
    curElem := curElem.next;
  end;
end;

procedure Queue<T>.Print;
begin
  write(ToString);
end;

procedure Queue<T>.Println;
begin
  writeln(ToString);
end;

// ---------------------------------- DynArray ---------------------------------
constructor DynArray<T>.Create;
begin
  Create(0);
end;

constructor DynArray<T>.Create(pSize: integer);
begin
  if pSize < 0 then
    raise new Exception(NegativeArraySizeExceptionMessage + pSize.ToString);
  
  fSize := pSize;
  fCap := INC_CAP_FACTOR * pSize + MIN_CAP; // Устанавливаем емкость "с запасом"
  SetLength(fData, fCap);
end;

procedure DynArray<T>.Reserve(newCap: integer);
begin
  if newCap > fCap then 
  begin
    SetLength(fData, newCap);
    fCap := newCap;
  end;
end;

procedure DynArray<T>.Resize(newSize: integer);
begin
  if newSize < 0 then
    raise new Exception(NegativeArraySizeExceptionMessage + newSize.ToString);
  
  if newSize > fCap then
  begin
    Reserve(INC_CAP_FACTOR * newSize);
    for var i := fSize to newSize - 1 do // явным образом заполняем новые элементы
      fData[i] := default(T);
  end;
  fSize := newSize;
end;

procedure DynArray<T>.Add(x: T);
begin
  Resize(fSize + 1);
  fData[fSize - 1] := x;
end;

procedure DynArray<T>.Insert(pos: integer; x: T);
begin
  if (pos < 0) or (pos > fSize - 1) then 
    raise new Exception(InsOutOfArrayBoundExceptionMessage + pos.ToString);
  
  Resize(fSize + 1);
  for var i := fSize - 2 downto pos do
    fData[i + 1] := fData[i];
  fData[pos] := x;
end;

procedure DynArray<T>.Remove(pos: integer);
begin
  if (pos < 0) or (pos > fSize - 1) then 
    raise new Exception(RemOutOfArrayBoundExceptionMessage + pos.ToString);
  
  for var i := pos to fSize - 2 do // сдвиг элементов влево на 1, начиная с позиции pos
    fData[i] := fData[i + 1];
  Resize(fSize - 1);
end;

function DynArray<T>.Find(x: T): integer;
begin
  Result := -1;
  for var i := 0 to fSize - 1 do
    if fData[i] = x then
    begin
      Result := i;
      exit;
    end;
end;

procedure DynArray<T>.SetElem(index: integer; x: T);
begin
  if (index < 0) or (index > fSize - 1) then 
    raise new Exception(SetElemOutOfBoundExceptionMessage + index.ToString);
  
  fData[index] := x;
end;

{Возвращает элемент массива с индексом ind}
function DynArray<T>.GetElem(index: integer): T;
begin
  if (index < 0) or (index > fSize - 1) then 
    raise new Exception(GetElemOutOfBoundExceptionMessage + index.ToString);
  
  Result := fData[index];
end;

function DynArray<T>.ToString: string;
begin
  Result := '';
  for var i := 0 to fSize - 1 do
    Result += fData[i].ToString + ' '; 
end;

procedure DynArray<T>.Print;
begin
  write(ToString);
end;

procedure DynArray<T>.Println;
begin
  writeln(ToString);
end;

// -------------------------------- SimpleSet ----------------------------------
constructor SimpleSet<T>.Create;
begin
  data := new DynArray<T>;
end;

procedure SimpleSet<T>.Add(x: T);
begin
  if data.Find(x) = -1 then 
    data.Add(x);
end;

procedure SimpleSet<T>.Remove(x: T);
begin
  var xPos := data.Find(x);
  if xPos <> -1 then
    data.Remove(xPos);
end;

function SimpleSet<T>.Contains(x: T): boolean;
begin
  Result := (data.Find(x) <> -1);
end;

function SimpleSet<T>.ToString: string;
begin
  Result := data.ToString;
end;

procedure SimpleSet<T>.Print;
begin
  write(ToString);
end;

procedure SimpleSet<T>.Println;
begin
  writeln(ToString);
end;

// -------------------------------- AssocArray ---------------------------------
constructor AssocArray<KeyType, ValueType>.Create;
begin
  keys := new DynArray<KeyType>;
  values := new DynArray<ValueType>;
end;

procedure AssocArray<KeyType, ValueType>.SetElem(key: KeyType; value: ValueType);
begin
  var ind := Keys.Find(key);
  if ind <> -1 then
    Values[ind] := value
  else
  begin
    Keys.Add(key);
    Values.Add(value);
  end;
end;

function AssocArray<KeyType, ValueType>.GetElem(key: KeyType): ValueType;
begin
  var ind := Keys.Find(key);
  if ind <> -1 then
    Result := Values[ind]
  else Result := default(ValueType);
end;

function AssocArray<KeyType, ValueType>.ToString: string;
const
  NewLine = #13#10;
begin
  Result := '';
  for var i := 0 to keys.Count - 1 do
    Result += keys[i].ToString + ' ' + values[i].ToString + NewLine;
end;

procedure AssocArray<KeyType, ValueType>.Print;
begin
  write(ToString);
end;

procedure AssocArray<KeyType, ValueType>.Println;
begin
  write(ToString);
end;

// -------------------------------- LinkedList ---------------------------------
constructor LinkedList<T>.Create;
begin
end;

procedure LinkedList<T>.AddFirst(x: T);
begin
  var val := new LinkedListNode<T>(x, nil, fFirst);
  if fFirst <> nil then
    fFirst.fPrev := val;
  
  fFirst := val;
  if fLast = nil then
    fLast := fFirst;
end;

procedure LinkedList<T>.AddLast(x: T);
begin
  var val := new LinkedListNode<T>(x, fLast, nil);
  if fLast <> nil then
    fLast.fNext := val;
  
  fLast := val;
  if fFirst = nil then
    fFirst := fLast;
end;

procedure LinkedList<T>.RemoveFirst();
begin
  if fFirst = nil then
    raise new Exception(RemoveFromNilListExceptionMessage);
  
  fFirst := fFirst.fNext;
  if fFirst = nil then
    fLast := nil
  else
    fFirst.fPrev := nil;
end;

procedure LinkedList<T>.RemoveLast();
begin
  if fLast = nil then
    raise new Exception(RemoveFromNilListExceptionMessage);
  
  fLast := fLast.fPrev;
  if fLast = nil then
    fFirst := nil
  else
    fLast.fNext := nil;
end;

procedure LinkedList<T>.AddBefore(node: LinkedListNode<T>; x: T);
begin
  if node = nil then
    raise new Exception(AddNilNodeExceptionMessage);
  
  if node = fFirst then
    AddFirst(x)
  else
  begin
    var val := new LinkedListNode<T>(x, node.fPrev, node);
    node.fPrev.fNext := val;
    node.fPrev := val;
  end;
end;

procedure LinkedList<T>.AddAfter(node: LinkedListNode<T>; x: T);
begin
  if node = nil then
    raise new Exception(AddNilNodeExceptionMessage);
  
  if node = fLast then
    AddLast(x)
  else
  begin
    var val := new LinkedListNode<T>(x, node, node.fNext);
    node.fNext.fPrev := val;
    node.fNext := val;
  end;
end;

procedure LinkedList<T>.Remove(node: LinkedListNode<T>);
begin
  if node = nil then
    raise new Exception(RemoveNilNodeExceptionMessage);
  
  if node = fFirst then
    RemoveFirst
  else if node = fLast then
    RemoveLast
  else
  begin
    node.fPrev.fNext := node.fNext;
    node.fNext.fPrev := node.fPrev;
  end;
end;

function LinkedList<T>.ToString: string;
begin
  Result := '';
  var cur := fFirst;
  while cur <> nil do
  begin
    Result := cur.Value.ToString + ' ';
    cur := cur.Next;
  end;
end;

procedure LinkedList<T>.Print;
begin
  write(ToString);
end;

procedure LinkedList<T>.Println;
begin
  writeln(ToString);
end;

end.