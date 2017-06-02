///- ������ �����������
/// ��������� ��� ������������, ������� ����� ��������� �������
unit �����������;

type
  ///!#
  ����� = integer;
  ������������ = real;
  ���������� = boolean;
  ��������� = string;

///- ��������� �������(��������)
/// ������� ������ ��������
procedure �������(params args: array of object) := Println(args);
/// ������� ������ ������
procedure ������������������� := Println;

///- ����� �����.�������
/// ������� �����
procedure �������(Self: �����); extensionmethod := Println(Self);
///- ����� ������������.�������
/// ������� ������������
procedure �������(Self: real); extensionmethod := Println(Self);
///- ����� ������.�������
/// ������� ������
procedure �������(Self: string); extensionmethod := Println(Self);

type
  ///!#
  Seq<T> = interface(IEnumerable<T>)
  
  end;
  
  ///!#
  ���������� = class
  public
    ///- ����� �����.������(���������)
    /// ������� ������, �������� �� ���������
    procedure ������(params args: array of object) := Println(args);
    ///- ����� �����.������������
    /// ������� ������ ������
    procedure ������������ := Println;
  end;
  
  ����������������������� = class
    
  end;

// ��������� ������

function DeleteEnd(Self: string; s: string): string; extensionmethod;
begin
  if Self.EndsWith(s) then
  begin
    var i := Self.LastIndexOf(s);
    if (i>=0) and (i<Self.Length) then
      Result := Self.Remove(i)
    else Result := Self;  
  end
  else Result := Self;
end;
 
Procedure PrintAllMethods(o: Object);
begin
  WritelnFormat('������ ����������� {0}:',o.GetType.Name.DeleteEnd('�����'));
  o.GetType.GetMethods(System.Reflection.BindingFlags.Public or
            System.Reflection.BindingFlags.Instance or 
            System.Reflection.BindingFlags.DeclaredOnly)
    .Select(s->s.ToString.Replace('Void ','')
    .Replace('Int32','�����')
    .Replace('Boolean','����������')
    .Replace('System.String','���������')
    .Replace('Double','������������'))
    .Select(s->'  '+s.DeleteEnd('()'))
    .Where(s->not s.ToString.Contains('$Init$'))
    .Println(NewLine);
end;

// ����������
type
  ///!#
  �������������� = class
  private
    s := new SortedSet<integer>;
  public
    constructor;
    begin end;
    ///- ����� ���������.��������(�������: �����)
    /// ��������� ������� � ���������. ���� ����� ������� ����, �� ������ �� ����������
    procedure ��������(params a: array of integer);
    begin
      a.ForEach(x->begin s.Add(x) end);
      //s.Add(�������);
    end;   
    ///- ����� ���������.�������(�������: �����)
    /// ������� ������� �� ���������. ���� ������ �������� ���, �� ������ �� ����������
    procedure �������(�������: �����);
    begin
      s.Remove(�������);
    end;
    ///- ����� ���������.�������
    /// ������� �������� ���������
    procedure �������;
    begin
      s.Println;
    end;
    ///- ����� ���������.��������(�������: �����): ����������
    /// ���������, ���� �� ������� �� ���������
    function ��������(�������: �����): ����������;
    begin
      Result := s.Contains(�������)
    end;
    ///- ����� ���������.����������������
    /// ������� ��� ������ ����������� ���������
    procedure ����������������;
    begin
      if Random(2)=1 then
        PrintAllMethods(Self)
      else 
      begin
        WritelnFormat('������ ����������� {0}:',Self.GetType.Name.DeleteEnd('�����'));
        Writeln('  ��������(�������: �����)');
        Writeln('  �������(�������: �����)');
        Writeln('  �������');
        Writeln('  ��������(�������: �����): ����������');
        Writeln('  ����������������');
      end;
    end;
    ///- ����� ���������.��������
    /// ������ ��������� ������
    procedure ��������;
    begin
      s.Clear
    end;
    ///- ����� ���������.�����
    /// ������� ����� ���������
    function �����(params a: array of integer): ��������������;
    begin
      Result := new ��������������();
      Result.��������(a)
    end;
    ///- ����� ���������.�����������(���������1)
    /// ���������� ����������� ��������   
    function �����������(s1: ��������������): ��������������;
    begin
      Result := new ��������������();
      var ss := SSet(s.AsEnumerable&<integer>);
      ss.IntersectWith(s1.s);
      Result.s := ss;
    end;
  end;
  
type  
  ///!#
  ������������������� = class
  private
    s := new SortedSet<string>;
  public
    constructor;
    begin end;
    ///- ����� ���������.��������(�������: ���������)
    /// ��������� ������� � ���������. ���� ����� ������� ����, �� ������ �� ����������
    procedure ��������(�������: string);
    begin
      s.Add(�������);
    end;
    ///- ����� ���������.�������(�������: ���������)
    /// ������� ������� �� ���������. ���� ������ �������� ���, �� ������ �� ����������
    procedure �������(�������: string);
    begin
      s.Remove(�������);
    end;
    ///- ����� ���������.�������
    /// ������� �������� ���������
    procedure �������;
    begin
      s.Println;
    end;
    ///- ����� ���������.��������(�������: ���������): ����������
    /// ���������, ���� �� ������� �� ���������
    function ��������(�������: string): ����������;
    begin
      Result := s.Contains(�������)
    end;
    ///- ����� ���������.����������������
    /// ������� ��� ������ ����������� ���������
    procedure ����������������;
    begin
      if Random(2)=1 then
        PrintAllMethods(Self)
      else 
      begin
        WritelnFormat('������ ����������� {0}:',Self.GetType.Name.DeleteEnd('�����'));
        Writeln('  ��������(�������: �����)');
        Writeln('  �������(�������: �����)');
        Writeln('  �������');
        Writeln('  ��������(�������: �����): ����������');
        Writeln('  ����������������');
      end;
    end;
    ///- ����� ���������.��������
    /// ������ ��������� ������
    procedure ��������;
    begin
      s.Clear
    end;
    ///- ����� ���������.�����
    /// ������� ����� ���������
    function ����� := new �������������������;
    ///- ����� ���������.�����������(���������1)
    /// ������� ���� ��������� �� ���� ��������
    function �����������(��1: �������������������): �������������������;
    begin
      var ss: SortedSet<string>;
      ss := (��1 as �������������������).s;
      
      var m := new �������������������;
      m.s := s.ZipTuple(ss).Select(x -> x.ToString()).ToSortedSet;
      Result := m
    end;
  end;
  
type
///!#
  ���������������� = class
  public
  ///- ����� �����������.�������������������(a,b,c: ������������)
  /// ������� ��� ������� ����������� ���������
    procedure �������������������������(a,b,c: real);
    begin
      writelnFormat('���������� ���������: {0}*x*x+{1}*x+{2}=0',a,b,c);
      var D := b*b-4*a*c;
      if D<0 then
        writeln('������� ���')
      else
      begin
        var x1 := (-b-sqrt(D))/2/a;
        var x2 := (-b+sqrt(D))/2/a;
        writelnFormat('�������: x1={0} x2={1}',x1,x2)
      end;
    end;
  ///- ����� �����������.������������������������(a0,d: �����)
  /// ������� �������������� ����������
    procedure �������������������������������(a0,d: integer);
    begin
      writelnFormat('�������������� ����������: a0={0} d={1}',a0,d);
      SeqGen(10,a0,x->x+d).Println; // ! ������ ���� ��������� ����������� �����
    end;
    procedure ����������������;
    begin
      PrintAllMethods(Self);
    end;
  end;

  FileState = (Closed,OpenedForRead,OpenedForWrite);
  ///!#
  ��������� = class
  private
    f: Text;
    State := FileState.Closed;
  public
    constructor ;
    begin
    end;
  ///- ����� ����.���������������(���)
  /// ��������� ���� �� ������
    procedure ���������������(���: ���������);
    begin
      if State<>FileState.Closed then
        f.Close;
      f := OpenRead(���);
      State := FileState.OpenedForRead
    end;
  ///- ����� ����.���������������(���)
  /// ��������� ���� �� ������
    procedure ���������������(���: ���������);
    begin
      if State<>FileState.Closed then
        f.Close;
      f := OpenWrite(���);
      State := FileState.OpenedForWrite
    end;
  ///- ����� ����.�������
  /// ��������� ����
    procedure �������;
    begin
      if State=FileState.Closed then
        Println('������: ���� ��� ������')
      else f.Close;
      State := FileState.Closed;
    end;
  ///- ����� ����.��������(������)
  /// ���������� ������ � ����
    procedure ��������(������: ���������);
    begin
      if State=FileState.Closed then
        Println('������: ����� ������� ���� ������� �������')
      else f.Writeln(������)
    end;
  ///- ����� ����.���������������
  /// ���������� ������, ��������� �� �����
    function ���������������: ���������;
    begin
      if State=FileState.Closed then
      begin
        Println('������: ����� ������� ���� ������� �������');
        Result := '';
      end
      else 
      begin
        Result := f.ReadlnString;
        Println(Result);
      end;  
    end;
  ///- ����� ����.��������������
  /// ���������� �����, ��������� �� �����
    function ��������������: �����;
    begin
      if State=FileState.Closed then
      begin
        Println('������: ����� ������� ���� ���� �������');
        Result := 0;
      end
      else 
      begin
        Result := f.ReadInteger;
        Print(Result);
      end;  
    end;
  ///- ����� ����.���������������������
  /// ���������� ������������, ��������� �� �����
    function ���������������������: ������������;
    begin
      if State=FileState.Closed then
      begin
        Println('������: ����� ������� ���� ���� �������');
        Result := 0;
      end
      else 
      begin
        Result := f.ReadReal;
        Print(Result);
      end;  
    end;
  ///- ����� ����.����������
  /// ����������, ��������� �� ����� �����
    function ����������: ����������;
    begin
      Result := f.Eof;
    end;
  ///- ����� ����.��������
  /// ���������� ��� �����
    function ��������: ���������;
    begin
      if State=FileState.Closed then
      begin
        Println('������: ���� ������, ������� �� �� ����� �����');
        Result := '';
      end
      else 
      begin
        Println('��� �����: ',f.Name);
        Result := f.Name;
      end
    end;
  ///- ����� ����.�����������������
  /// ������� ���������� �����
    procedure �����������������(��������: ���������);
    begin
      if (State<>FileState.Closed) and (f.Name.ToLower=��������.ToLower) then
        Println('������: ���������� ����� ������� ������ � ��������� �����')
      else 
      begin
        WritelnFormat('���������� ����� {0}:',��������);
        try
          ReadLines(��������).Println(NewLine);
        except
          WritelnFormat('���� {0}: ����������� �� �����',��������);
        end;
      end;  
        
    end;
  ///- ����� ����.����������������
  /// ������� ��� ������ ����������� ����
    procedure ����������������;
    begin
      PrintAllMethods(Self);
    end;
  ///- ����� ����.�����
  /// ������� � ���������� ������ ����������� ����
    function ����� := new ���������;
  ///- ����� ����.���������
  /// ���������� ��� ������ ����� � ���� ������������������
    function ���������(��������: ���������): sequence of ���������;
    begin
      if (State<>FileState.Closed) and (f.Name.ToLower=��������.ToLower) then
      begin
        Println('������: ������ ����� �������� ������ � ��������� �����');
        Result := nil;
        exit;
      end;
      Result := ReadLines(��������).ToArray;
    end;
  end;

const dbname = 'countries.db';

var coun: array of string := nil;

function ������������: sequence of string;
begin
  if coun = nil then
    coun := ReadLines(dbname).ToArray();
  Result := coun;  
end;

///- ����� ������������������.�������
/// ������� ��� �������� ������������������, �������� �� ���������
function �������<T>(Self: sequence of T): sequence of T; extensionmethod;
begin
  Self.Println;
  Result := Self;
end;

///- ����� ������������������.����������������
/// ������� ��� �������� ������������������ - ������ ������� � ����� ������
function ����������������<T>(Self: sequence of T): sequence of T; extensionmethod;
begin
  Self.Println(NewLine);
  Result := Self;
end;

///- ����� ������������������.�������(�������)
/// �������� ��� �������� ������������������, ��������������� ���������� �������
function �������<T>(Self: sequence of T; cond: T -> boolean): sequence of T; extensionmethod;
begin
  Result := Self.Where(cond);  
end;

///- ����� ������������������.�����(n)
/// ���������� ������ n ��������� ������������������
function �����<T>(Self: sequence of T; n: integer): sequence of T; extensionmethod;
begin
  Result := Self.Take(n);  
end;

///- ����� ������������������.����������(�������)
/// ���������� ���������� ��������� ������������������, ��������������� ���������� �������
function ����������<T>(Self: sequence of T; cond: T -> boolean := nil): �����; extensionmethod;
begin
  if cond = nil then
    Result := Self.Count()
  else Result := Self.Count(cond)
end;

///- ����� ������������������.�����
/// ���������� ����� ��������� ������������������
function �����(Self: sequence of integer): integer; extensionmethod;
begin
  Result := Self.Sum();  
end;  

///- ����� ������������������.�������
/// ���������� ������� ��������� ������������������
function �������(Self: sequence of integer): real; extensionmethod;
begin
  Result := Self.Average;  
end;  

///- ����� ������������������.�������
/// ���������� ����������� ������� ������������������
function �������(Self: sequence of integer): integer; extensionmethod;
begin
  Result := Self.Min;  
end;  

///- ����� ������������������.��������
/// ���������� ������������ ������� ������������������
function ��������(Self: sequence of integer): integer; extensionmethod;
begin
  Result := Self.Max;
end;  

///- ����� ������������������.�������������(������� ��������������)
/// ����������� �������� ������������������ � ������� ������� � ���������� ����� ������������������
function �������������<T,Key>(Self: sequence of T; conv: T -> Key): sequence of Key; extensionmethod;
begin
  Result := Self.Select(conv);  
end;

///- ����� ������������������.���������������(�������� �������� �� ����)
/// ��������� �������� ������������������ �� ����������� �����
function ���������������<T,Key>(Self: sequence of T; cond: T -> Key): sequence of T; extensionmethod;
begin
  Result := Self.OrderBy(cond);  
end;

///- ����� ������������������.���������������(�������� �������� �� ����)
/// ��������� �������� ������������������ �� �����
function �������������<T>(Self: sequence of T): sequence of T; extensionmethod;
begin
  Result := Self.OrderBy(x->x);  
end;

///- ����� ������������������.�����������������������(�������� �������� �� ����)
/// ��������� �������� ������������������ �� �������� �����
function �����������������������<T,Key>(Self: sequence of T; cond: T -> Key): sequence of T; extensionmethod;
begin
  Result := Self.OrderByDescending(cond);  
end;

///- ����� ������������������.�������(��������)
/// ��������� ��������� �������� ��� ������� �������� ������������������
procedure �������<T>(Self: sequence of T; act: T -> ()); extensionmethod;
begin
  Self.Foreach(act);  
end;

function �������(c: char): string -> boolean;
begin
  Result := ������ -> ������[1] = c;
end;

function �������(s: string): string -> boolean;
begin
  Result := ������ -> ������[1] = s[1];
end;

function ������������(Self: string; s: string): boolean; extensionmethod;
begin
  Result := Self.StartsWith(s);  
end;

///- ������(��������)
/// ����������, �������� �� �������� ������
function ������(x: integer): boolean;
begin
  Result := x mod 2 = 0;
end;

///- ��������(��������)
/// ����������, �������� �� �������� ��������
function ��������(x: integer): boolean;
begin
  Result := x mod 2 <> 0;
end;

type 
///!#
  ������������������� = class
public
  ///- ����������� ��������� 
  /// ������������� ������ ��� ��������� �����
  ���������: ��������������;
  ///- ����������� ��������������
  /// ������������� ������ ��� ��������� �����
  ��������������: �������������������;
  ///- ����������� �����������
  /// ������������� ��� ������� �� ������� ����������
  �����������: ����������������;
  ///- ����������� ����
  /// ������������� ��� ������� ��� ������ � ������� �� �����
  ����: ���������;
  ///- ����������� �����
  /// ������������� ������ ��� ������ ������
  �����: ����������;
  ///- ����� �������
  /// ������� ���� ������������
  procedure �������;
  begin
    Println('���������');
    Println('��������������');
    Println('�����������');
    Println('����');
    Println('�����');
  end;
end;

type 
  ///!#
  Country = auto class
    nm,cap: string;
    inh: integer;
    cont: string;
  public  
    property ��������: string read nm;
    property �������: string read cap;
    property ���������: integer read inh;
    property ���������: string read cont;
  end;
  
var ������: sequence of Country;  

procedure InitCountries();
begin
  ������ := ReadLines('������.csv')
    .Select(s->s.ToWords(';'))
    .Select(w->new Country(w[0],w[1],w[2].ToInteger,w[3])).ToArray;
end;

// ������������������

///- ������������������������(������,���,����������)
/// ���������� �������������� ���������� � ��������� ������ ���������, ����� � �����������
function ������������������������(a,d: integer; n: integer := 20): sequence of integer;
begin
  Result := SeqGen(n,a,a->a+d)
end;

///- ������������������������(������,���,����������)
/// ���������� �������������� ���������� � ��������� ������ ���������, ����� � �����������
function ������������������������(a,d: real; n: integer := 20): sequence of real;
begin
  Result := SeqGen(n,a,a->a+d)
end;

///- �����������������������(������,���,����������)
/// ���������� ������������� ���������� � ��������� ������ ���������, ����� � �����������
function �����������������������(a,d: integer; n: integer := 10): sequence of integer;
begin
  Result := SeqGen(n,a,a->a*d)
end;

///- �����������������������(������,���,����������)
/// ���������� ������������� ���������� � ��������� ������ ���������, ����� � �����������
function �����������������������(a,d: real; n: integer := 10): sequence of real;
begin
  Result := SeqGen(n,a,a->a*d)
end;

///- ���������������������������(����������,��,��)
/// ���������� ��������� ������������������ ��������� � ��������� [��, ��] 
function ���������������������������(n: integer := 10; a: integer := 0; b: integer := 10): sequence of integer;
begin
  Result := ArrRandom(n,a,b)
end;

function ��������� := new ���������;

var 
  ///- ����������� ���������. ������������� ������ ��� ��������� �����
  ��������� := new ��������������;
  ///- ����������� ��������������. ������������� ������ ��� ��������� �����
  �������������� := new �������������������;
  ///- ����������� �����������. ������������� ��� ������� �� ������� ����������
  ����������� := new ����������������;
  ///- ����������� ����. ������������� ��� ������� ��� ������ � ������� �� �����
  ���� := new ���������;
  ///- ����������� �����. ������������� ������ ��� ������ ������
  ����� := new ����������;
  ///- ����������� ���������������. �������� ���� ������������ 
  ��������������� := new �������������������;
begin  
  ���������������.��������� := ���������;
  ���������������.�������������� := ��������������;
  ���������������.����������� := �����������;
  ���������������.���� := ����;
  ���������������.����� := �����;
  InitCountries;
end.  
  