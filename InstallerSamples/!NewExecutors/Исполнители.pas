///- ������ �����������
/// ��������� ��� ������������, ������� ����� ��������� �������
unit �����������;

///- ��������� �����(��������)
/// ������� ������ ��������
procedure �����(params args: array of object) := Println(args);
/// ������� ������ ������
procedure ����������������� := Println;

type
  ����� = integer;
  ������������ = real;
  ���������� = boolean;
  ��������� = string;
  
  ������������ = interface
    ///- ����� ���������.��������(�������: �����)
    /// ��������� ������� � ���������. ���� ����� ������� ����, �� ������ �� ����������
    procedure ��������(�������: �����);
    ///- ����� ���������.�������(�������: �����)
    /// ������� ������� �� ���������. ���� ������ �������� ���, �� ������ �� ����������
    procedure �������(�������: �����);
    ///- ����� ���������.�������
    /// ������� �������� ���������
    procedure �������;
    ///- ����� ���������.��������(�������: �����): ����������
    /// ���������, ���� �� ������� �� ���������
    function ��������(�������: �����): ����������;
    ///- ����� ���������.����������������
    /// ������� ��� ������ ����������� ���������
    procedure ����������������;
  end;
  
  �������������� = interface
  ///- ����� �����������.�������������������(a,b,c: ������������)
  /// ������� ��� ������� ����������� ���������
    procedure �������������������(a,b,c: ������������);
  ///- ����� �����������.������������������������(a0,d: �����)
  /// ������� �������������� ����������
    procedure ������������������������(a0,d: integer);
    procedure ����������������;
  end;
  
  ������� = interface
    procedure ���������������(���: ���������);
    procedure ���������������(���: ���������);
    procedure �������;
    procedure ��������(������: ���������);
    function ���������������: ���������;
    function ��������������: �����;
    function ����������: ����������;
    function ��������: ���������;
    procedure �����������������(��������: ���������);
    procedure ����������������;
  end;


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
  �������������� = class(������������)
    s := new SortedSet<integer>;
  public
    procedure ��������(�������: �����);
    begin
      s.Add(�������);
    end;
    procedure �������(�������: �����);
    begin
      s.Remove(�������);
    end;
    procedure �������;
    begin
      s.Println;
    end;
    function ��������(�������: �����): ����������;
    begin
      Result := s.Contains(�������)
    end;
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
  end;
  
type
  ���������������� = class(��������������)
  public
    procedure �������������������(a,b,c: real);
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
    procedure ������������������������(a0,d: integer);
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
  ��������� = class(�������)
    f: Text;
    State := FileState.Closed;
  public
    procedure ���������������(���: ���������);
    begin
      if State<>FileState.Closed then
        f.Close;
      f := OpenRead(���);
      State := FileState.OpenedForRead
    end;
    procedure ���������������(���: ���������);
    begin
      if State<>FileState.Closed then
        f.Close;
      f := OpenWrite(���);
      State := FileState.OpenedForWrite
    end;
    procedure �������;
    begin
      if State=FileState.Closed then
        Println('���� ��� ������')
      else f.Close;
      State := FileState.Closed;
    end;
    procedure ��������(������: ���������);
    begin
      if State=FileState.Closed then
        Println('����� ������� ���� ������� �������')
      else f.Writeln(������)
    end;
    function ���������������: ���������;
    begin
      if State=FileState.Closed then
      begin
        Println('����� ������� ���� ������� �������');
        Result := '';
      end
      else 
      begin
        Result := f.ReadlnString;
        Println(Result);
      end;  
    end;
    function ��������������: �����;
    begin
      if State=FileState.Closed then
      begin
        Println('����� ������� ���� ���� �������');
        Result := 0;
      end
      else 
      begin
        Result := f.ReadInteger;
        Print(Result);
      end;  
    end;
    function ����������: ����������;
    begin
      Result := f.Eof;
    end;
    function ��������: ���������;
    begin
      if State=FileState.Closed then
      begin
        Println('���� ������, ������� �� �� ����� �����');
        Result := '';
      end
      else 
      begin
        Println('��� �����: ',f.Name);
        Result := f.Name;
      end
    end;
    procedure �����������������(��������: ���������);
    begin
      if (State<>FileState.Closed) and (f.Name.ToLower=��������.ToLower) then
        Println('���������� ����� ������� ������ � ��������� �����')
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
    procedure ����������������;
    begin
      PrintAllMethods(Self);
    end;
  end;

const dbname = 'countries.db';

var coun: array of string := nil;

function ������: sequence of string;
begin
  if coun = nil then
    coun := ReadLines(dbname).ToArray();
  Result := coun;  
end;

function �������<T>(Self: sequence of T; cond: T -> boolean): sequence of T; extensionmethod;
begin
  Result := Self.Where(cond);  
end;

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

var 
  ///- ��������� - ��� ����� ��������
  ���������: ������������ := new ��������������;
  ///- ��������� - ��� ����� ��������
  ���������1: ������������ := new ��������������;
  ///- ����������� - �����������, ������������ �������������� ����������
  �����������: �������������� := new ����������������;
  ///- ���� - �����������, ����������� �� � ������������ � ���� �� �����
  ����: ������� := new ���������;
end.  
  