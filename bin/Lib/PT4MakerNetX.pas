/// ����������� ������� ������� ��� ��������� Programming Taskbook.
/// ������ 1.5 �� 27.03.2017 (�) �. �. �������, 2016-2017.
/// ��� ���������� ������������ ����� ���������� ���� ��� ��������� ������ ������ pt, 
/// ���� ��� ������� ��������� � ���������.
unit PT4MakerNetX;

interface

const
  OptionAllLanguages = 1;       // ������ �������� ��� ���� ������
  OptionPascalLanguages = 2;    // ������ �������� ��� ���� ���������� �������
  OptionNETLanguages = 4;       // ������ �������� ��� ���� NET-������
  OptionUseAddition = 8;        // ������ �������� ������ ��� ������� ����� ����������
  OptionHideExamples = 16;      // �� ���������� ������ � �������� ������� �������

/// ���������, � ������� ������ ���������� ����������� ������ �������.
/// ������ ���������� � ��������� � ������, ������������ � ������ Task.
/// �������� topic ���������� ��� ��������� � �������� ��������������.
/// �������� tasktext �������� ������������ �������; ��������� ������
/// ������������ ������ ����������� ��������� #13, #10 ��� #13#10.
procedure NewTask(topic, tasktext: string);

/// ���������, � ������� ������ ���������� ����������� ������ �������.
/// ������ ���������� � ��������� � ������, ������������ � ������ Task.
/// �������� tasktext �������� ������������ �������; ��������� ������
/// ������������ ������ ����������� ��������� #13, #10 ��� #13#10.
procedure NewTask(tasktext: string);

/// ��������� ����������� � ����� ������ ������� �������� �������.
procedure DataComm(comm: string);

/// ��������� ������ � ����������� � ����� ������ ������� �������� �������.
procedure Data(comm: string; a: object);

/// ��������� ������ � ����������� � ����� ������ ������� �������� �������.
procedure Data(comm1: string; a1: object; comm2: string);

/// ��������� ������ � ����������� � ����� ������ ������� �������� �������.
procedure Data(comm1: string; a1: object; comm2: string; a2: object);

/// ��������� ������ � ����������� � ����� ������ ������� �������� �������.
procedure Data(comm1: string; a1: object; comm2: string; a2: object; comm3: string);

/// ��������� ������ � ����������� � ����� ������ ������� �������� �������.
procedure Data(comm1: string; a1: object; comm2: string; a2: object; comm3: string; a3: object);

/// ��������� ������������������ ���������� ������ � ������ �������� ������.
procedure Data(seq: sequence of boolean);

/// ��������� ������������������ ����� ����� � ������ �������� ������.
procedure Data(seq: sequence of integer);

/// ��������� ������������������ ������������ ����� � ������ �������� ������.
procedure Data(seq: sequence of real);

/// ��������� ������������������ �������� � ������ �������� ������.
procedure Data(seq: sequence of char);

/// ��������� ������������������ ����� � ������ �������� ������.
procedure Data(seq: sequence of string);

/// ��������� ����������� � ����� ������ ������� �����������.
procedure ResComm(comm: string);

/// ��������� ������ � ����������� � ����� ������ ������� �����������.
procedure Res(comm: string; a: object);

/// ��������� ������ � ����������� � ����� ������ ������� �����������.
procedure Res(comm1: string; a1: object; comm2: string);

/// ��������� ������ � ����������� � ����� ������ ������� �����������.
procedure Res(comm1: string; a1: object; comm2: string; a2: object);

/// ��������� ������ � ����������� � ����� ������ ������� �����������.
procedure Res(comm1: string; a1: object; comm2: string; a2: object; comm3: string);

/// ��������� ������ � ����������� � ����� ������ ������� �����������.
procedure Res(comm1: string; a1: object; comm2: string; a2: object; comm3: string; a3: object);

/// ��������� ������������������ ���������� ������ � ������ �����������.
procedure Res(seq: sequence of boolean);

/// ��������� ������������������ ����� ����� � ������ �����������.
procedure Res(seq: sequence of integer);

/// ��������� ������������������ ������������ ����� � ������ �����������.
procedure Res(seq: sequence of real);

/// ��������� ������������������ �������� � ������ �����������.
procedure Res(seq: sequence of char);

/// ��������� ������������������ ����� � ������ �����������.
procedure Res(seq: sequence of string);

/// ������ ����������� ������ ���� ������ ��� �������� ������
/// (����� ������������� �� ������� ���� ���� ������, 
/// �.�. ��� ������������� ����������� ����� ���������).
/// ���� n �� ����� � ��������� 0..10, �� ����� ��������� ������������.
/// �� ��������� ����������� ������ ���� ������ ���������� ������ 0.
procedure SetWidth(n: integer);

/// ������ ������ ����������� ������������ �����: 
/// � ������������� ������ � n �������� ������� ��� n > 0,
/// � ��������� ������ � n �������� ������� ��� n < 0,
/// � ��������� ������ � 6 �������� ������� ��� n = 0.
/// ���� n �� ������ ����������� 10, �� ����� ��������� ������������.
/// �� ��������� ��������������� ������ � ������������� ������
/// � 2 �������� �������.
procedure SetPrecision(n: integer);

/// ������ ���������� �������� ��������, �����������
/// ��� �������� ������������ ��������� (�� 2 �� 10). 
/// �� ��������� ����� �������� �������� ���������� ������ 5.
procedure SetTestCount(n: integer);

/// ������ ���������� ����������� ���������� ��������� 
/// �������� ������, ������� ��������� ������ ��� �����������
/// ������� ������ � ������ �������� ������ �������� ������. 
/// ��� ���������� ��������� ��������������,
/// ��� ��� ����������� ������� ���� ������ ��� �������� ������.
procedure SetRequiredDataCount(n: integer);

/// ���������� ����� �������� ��������� ������� 
/// (������� ���������� �� 1).
function CurrentTest: integer;

/// ���������� ��������� ����� � ��������� �� M �� N ������������.
/// ���� M >= N, �� ���������� M.
function Random(M, N: integer): integer;

/// ���������� ��������� ������������ �� ���������� [A, B).
/// ���� ���������� [A, B) ����, �� ���������� A.
function Random(A, B: real): real;

/// ���������� ��������� ������ ����� len, ���������
/// �� ���� � �������� (�.�. ���������) ��������� ����.
function RandomName(len: integer): string;

/// ������� ����� ������ � ������� ��������� GroupDescription,
/// ����������� �� ������ GroupAuthor � ������� �������������� �����, ������������ ��������� or.
/// ��� ������ ������������ �� ����� ���������� (����� ������������ �������� PT4). 
/// � ������ ���������� �������, ������������ � ����������, ����� ������� ���������� � ������ Task.
/// ��������� NewGroup ������ ���� ������� � ��������� inittaskgroup ��� ����������, �������
/// ���������� ������� � ���������� � ������� ������� (��� ����� � ����� inittaskgroup - ��������).
procedure NewGroup(GroupDescription, GroupAuthor: string; Options: integer := 0);

/// ������������ ����������� ��������� ������ � ����������� ���������.
/// ��������� ActivateNET(S) ������ ���� ������� � ��������� activate(S: string),
/// ������� ���������� ������� � ���������� � ������� ������� (��� ����� � ����� activate - ��������).
procedure ActivateNET(S: string);

/// ����������� � ����������� ������ ������������ �������
/// �� ������ GroupName � ������� TaskNumber. ������ ����������
/// � ��������� � ������, ������������ � ������ Task.
procedure UseTask(GroupName: string; TaskNumber: integer);

/// ���������� ������ �� 116 ������� ����.
function GetWords: array of string;
/// ���������� ������ �� 116 ���������� ����.
function GetEnWords: array of string;
/// ���������� ������ �� 61 �������� �����������.
function GetSentences: array of string;
/// ���������� ������ �� 61 ����������� �����������.
function GetEnSentences: array of string;
/// ���������� ������ �� 85 ������� ������������� �������.
/// ������ ������ ������ ������������ ��������� #13#10.
function GetTexts: array of string;
/// ���������� ������ �� 85 ���������� ������������� �������.
/// ������ ������ ������ ������������ ��������� #13#10.
function GetEnTexts: array of string;

/// ���������� ��������� ������� ����� �� �������, 
/// ��������� � ����������� ������� �������.
function RandomWord: string;
/// ���������� ��������� ���������� ����� �� �������, 
/// ��������� � ����������� ������� �������.
function RandomEnWord: string;
/// ���������� ��������� ������� ����������� �� �������, 
/// ��������� � ����������� ������� �������.
function RandomSentence: string;
/// ���������� ��������� ���������� ����������� �� �������, 
/// ��������� � ����������� ������� �������.
function RandomEnSentence: string;
/// ���������� ��������� ������� ������������� ����� �� �������, 
/// ��������� � ����������� ������� �������.
/// ������ ������ ������ ������������ ��������� #13#10.
function RandomText: string;
/// ���������� ��������� ���������� ������������� ����� �� �������, 
/// ��������� � ����������� ������� �������.
/// ������ ������ ������ ������������ ��������� #13#10.
function RandomEnText: string;


/// ��������� � ������� �������� ���� ����� �����
/// � ������ FileName � ���������� ��� ����������
/// � ������� �������� ������.
procedure DataFileInteger(FileName: string);
/// ��������� � ������� �������� ���� ������������ �����
/// � ������ FileName � ���������� ��� ����������
/// � ������� �������� ������.
procedure DataFileReal(FileName: string);
/// ��������� � ������� �������� ���������� ����
/// � ������ FileName � ���������� ��� ����������
/// � ������� �������� ������. ������� ������
/// ��������� � ����� � ����������� ���������.
procedure DataFileChar(FileName: string);
/// ��������� � ������� �������� ��������� ����
/// � ���������� ���� ShortString � ������ FileName 
/// � ���������� ��� ���������� � ������� �������� ������.
/// ����� ��������� ����� �� ������ ������������ 70 ��������.
/// ������ ������ ��������� � ����� � ����������� ���������.
procedure DataFileString(FileName: string);
/// ��������� � ������� �������� ��������� ����
/// � ������ FileName � ���������� ��� ����������
/// � ������� �������� ������. ����� ������ ������
/// ���������� ����� �� ������ ������������ 70 ��������.
/// ����� ������ ��������� � ����� � ����������� ���������.
procedure DataText(FileName: string; LineCount: integer := 4);

/// ��������� � ������� �������������� ���� ����� �����
/// � ������ FileName � ���������� ��� ���������� � ������� �����������.
procedure ResFileInteger(FileName: string);
/// ��������� � ������� �������������� ���� ������������ �����
/// � ������ FileName � ���������� ��� ���������� � ������� �����������.
procedure ResFileReal(FileName: string);
/// ��������� � ������� �������������� ���������� ����
/// � ������ FileName � ���������� ��� ���������� � ������� �����������.
/// ������� ������ ��������� � ����� � ����������� ���������.
procedure ResFileChar(FileName: string);
/// ��������� � ������� �������������� ��������� ����
/// � ���������� ���� ShortString � ������ FileName 
/// � ���������� ��� ���������� � ������� �����������.
/// ����� ��������� ����� �� ������ ������������ 70 ��������.
/// ������ ������ ��������� � ����� � ����������� ���������.
procedure ResFileString(FileName: string);
/// ��������� � ������� �������������� ��������� ����
/// � ������ FileName � ���������� ��� ���������� � ������� �����������. 
/// ����� ������ ������ ���������� ����� �� ������ ������������ 70 ��������.
/// ����� ������ ��������� � ����� � ����������� ���������.
procedure ResText(FileName: string; LineCount: integer := 5);

type pt = class
  private
   constructor Create;
   begin
   end;
  public
/// �������������� ����� ��� ��������� NewGroup: 
/// ������ �������� ��� ���� ������.  
  class function OptionAllLanguages: integer;    
  begin
    result := PT4MakerNetX.OptionAllLanguages;
  end;  
/// �������������� ����� ��� ��������� NewGroup: 
/// ������ �������� ��� ���� ���������� ����� Pascal.
  class function OptionPascalLanguages: integer;    
  begin
    result := PT4MakerNetX.OptionPascalLanguages;
  end;  
/// �������������� ����� ��� ��������� NewGroup: 
/// ������ �������� ��� ���� NET-������.
  class function OptionNETLanguages: integer;    
  begin
    result := PT4MakerNetX.OptionNETLanguages;
  end;  
/// �������������� ����� ��� ��������� NewGroup: 
/// ������ �������� ������ ��� ������� ���������� � ��� ����� ����������.
  class function OptionUseAddition: integer;    
  begin
    result := PT4MakerNetX.OptionUseAddition;
  end;  
/// �������������� ����� ��� ��������� NewGroup: 
/// � �������� ������ ������ �� ����� ������������ ������ ������� �������.
  class function OptionHideExamples: integer;    
  begin
    result := PT4MakerNetX.OptionHideExamples;
  end;  
/// ���������, � ������� ������ ���������� ����������� ������ �������.
/// ������ ���������� � ��������� � ������, ������������ � ������ Task.
/// �������� topic ���������� ��� ��������� (�������� ��������������).
/// �������� tasktext �������� ������������ �������; ��������� ������
/// ������������ ������ ����������� ��������� #13, #10 ��� #13#10.
class procedure NewTask(topic, tasktext: string);
begin
  PT4MakerNetX.NewTask(topic, tasktext);
end;
/// ���������, � ������� ������ ���������� ����������� ������ �������.
/// ������ ���������� � ��������� � ������, ������������ � ������ Task.
/// �������� tasktext �������� ������������ �������; ��������� ������
/// ������������ ������ ����������� ��������� #13, #10 ��� #13#10.
class procedure NewTask(tasktext: string);
begin
  PT4MakerNetX.NewTask(tasktext);
end;
/// ��������� ����������� � ����� ������ ������� �������� �������.
class procedure DataComm(comm: string);
begin
  PT4MakerNetX.DataComm(comm);
end;
/// ��������� ������ � ����������� � ����� ������ ������� �������� �������.
class procedure Data(comm: string; a: object);
begin
  PT4MakerNetX.Data(comm, a);
end;
/// ��������� ������ � ����������� � ����� ������ ������� �������� �������.
class procedure Data(comm1: string; a1: object; comm2: string);
begin
  PT4MakerNetX.Data(comm1, a1, comm2);
end;
/// ��������� ������ � ����������� � ����� ������ ������� �������� �������.
class procedure Data(comm1: string; a1: object; comm2: string; a2: object);
begin
  PT4MakerNetX.Data(comm1, a1, comm2, a2);
end;
/// ��������� ������ � ����������� � ����� ������ ������� �������� �������.
class procedure Data(comm1: string; a1: object; comm2: string; a2: object; comm3: string);
begin
  PT4MakerNetX.Data(comm1, a1, comm2, a2, comm3);
end;
/// ��������� ������ � ����������� � ����� ������ ������� �������� �������.
class procedure Data(comm1: string; a1: object; comm2: string; a2: object; comm3: string; a3: object);
begin
  PT4MakerNetX.Data(comm1, a1, comm2, a2, comm3, a3);
end;
/// ��������� ������������������ ���������� ������ � ������ �������� ������.
class procedure Data(seq: sequence of boolean);
begin
  PT4MakerNetX.Data(seq);
end;
/// ��������� ������������������ ����� ����� � ������ �������� ������.
class procedure Data(seq: sequence of integer);
begin
  PT4MakerNetX.Data(seq);
end;
/// ��������� ������������������ ������������ ����� � ������ �������� ������.
class procedure Data(seq: sequence of real);
begin
  PT4MakerNetX.Data(seq);
end;
/// ��������� ������������������ �������� � ������ �������� ������.
class procedure Data(seq: sequence of char);
begin
  PT4MakerNetX.Data(seq);
end;
/// ��������� ������������������ ����� � ������ �������� ������.
class procedure Data(seq: sequence of string);
begin
  PT4MakerNetX.Data(seq);
end;

/// ��������� ����������� � ����� ������ ������� �����������.
class procedure ResComm(comm: string);
begin
  PT4MakerNetX.ResComm(comm);
end;
/// ��������� ������ � ����������� � ����� ������ ������� �����������.
class procedure Res(comm: string; a: object);
begin
  PT4MakerNetX.Res(comm, a);
end;
/// ��������� ������ � ����������� � ����� ������ ������� �����������.
class procedure Res(comm1: string; a1: object; comm2: string);
begin
  PT4MakerNetX.Res(comm1, a1, comm2);
end;
/// ��������� ������ � ����������� � ����� ������ ������� �����������.
class procedure Res(comm1: string; a1: object; comm2: string; a2: object);
begin
  PT4MakerNetX.Res(comm1, a1, comm2, a2);
end;
/// ��������� ������ � ����������� � ����� ������ ������� �����������.
class procedure Res(comm1: string; a1: object; comm2: string; a2: object; comm3: string);
begin
  PT4MakerNetX.Res(comm1, a1, comm2, a2, comm3);
end;
/// ��������� ������ � ����������� � ����� ������ ������� �����������.
class procedure Res(comm1: string; a1: object; comm2: string; a2: object; comm3: string; a3: object);
begin
  PT4MakerNetX.Res(comm1, a1, comm2, a2, comm3, a3);
end;
/// ��������� ������������������ ���������� ������ � ������ �����������.
class procedure Res(seq: sequence of boolean);
begin
  PT4MakerNetX.Res(seq);
end;
/// ��������� ������������������ ����� ����� � ������ �����������.
class procedure Res(seq: sequence of integer);
begin
  PT4MakerNetX.Res(seq);
end;
/// ��������� ������������������ ������������ ����� � ������ �����������.
class procedure Res(seq: sequence of real);
begin
  PT4MakerNetX.Res(seq);
end;
/// ��������� ������������������ �������� � ������ �����������.
class procedure Res(seq: sequence of char);
begin
  PT4MakerNetX.Res(seq);
end;
/// ��������� ������������������ ����� � ������ �����������.
class procedure Res(seq: sequence of string);
begin
  PT4MakerNetX.Res(seq);
end;

/// ������ ����������� ������ ���� ������ ��� �������� ������
/// (����� ������������� �� ������� ���� ���� ������, 
/// �.�. ��� ������������� ����������� ����� ���������).
/// ���� n �� ����� � ��������� 0..10, �� ����� ��������� ������������.
/// �� ��������� ����������� ������ ���� ������ ���������� ������ 0.
class procedure SetWidth(n: integer);
begin
  PT4MakerNetX.SetWidth(n);
end;
/// ������ ������ ����������� ������������ �����: 
/// � ������������� ������ � n �������� ������� ��� n > 0,
/// � ��������� ������ � n �������� ������� ��� n < 0,
/// � ��������� ������ � 6 �������� ������� ��� n = 0.
/// ���� n �� ������ ����������� 10, �� ����� ��������� ������������.
/// �� ��������� ��������������� ������ � ������������� ������
/// � 2 �������� �������.
class procedure SetPrecision(n: integer);
begin
  PT4MakerNetX.SetPrecision(n);
end;
/// ������ ���������� �������� ��������, �����������
/// ��� �������� ������������ ��������� (�� 2 �� 10). 
/// �� ��������� ����� �������� �������� ���������� ������ 5.
class procedure SetTestCount(n: integer);
begin
  PT4MakerNetX.SetTestCount(n);
end;
/// ������ ���������� ����������� ���������� ��������� 
/// �������� ������, ������� ��������� ������ ��� �����������
/// ������� ������ � ������ �������� ������ �������� ������. 
/// ��� ���������� ��������� ��������������,
/// ��� ��� ����������� ������� ���� ������ ��� �������� ������.
class procedure SetRequiredDataCount(n: integer);
begin
  PT4MakerNetX.SetRequiredDataCount(n);
end;
/// ���������� ����� �������� ��������� ������� 
/// (������� ���������� �� 1).
class function CurrentTest: integer;
begin
  result := PT4MakerNetX.CurrentTest;
end;

/// ���������� ��������� ����� � ��������� �� M �� N ������������.
/// ���� M >= N, �� ���������� M.
class function Random(M, N: integer): integer;
begin
  result := PT4MakerNetX.Random(M, N);
end;
/// ���������� ��������� ������������ �� ���������� [A, B).
/// ���� ���������� [A, B) ����, �� ���������� A.
class function Random(A, B: real): real;
begin
  result := PT4MakerNetX.Random(A, B);
end;
/// ���������� ��������� ������ ����� len, ���������
/// �� ���� � �������� (�.�. ���������) ��������� ����.
class function RandomName(len: integer): string;
begin
  result := PT4MakerNetX.RandomName(len);
end;

/// ������� ����� ������ � ������� ��������� GroupDescription,
/// ����������� �� ������ GroupAuthor � ������� �������������� �����, ������������ ��������� or.
/// ��� ������ ������������ �� ����� ���������� (����� ������������ �������� PT4). 
/// � ������ ���������� �������, ������������ � ����������, ����� ������� ���������� � ������ Task.
/// ��������� NewGroup ������ ���� ������� � ��������� inittaskgroup ��� ����������, �������
/// ���������� ������� � ���������� � ������� ������� (��� ����� � ����� inittaskgroup - ��������).
class procedure NewGroup(GroupDescription, GroupAuthor: string; Options: integer := 0);
begin
  PT4MakerNetX.NewGroup(GroupDescription, GroupAuthor, Options);
end;
/// ������������ ����������� ��������� ������ � ����������� ���������.
/// ��������� ActivateNET(S) ������ ���� ������� � ��������� activate(S: string),
/// ������� ���������� ������� � ���������� � ������� ������� (��� ����� � ����� activate - ��������).
class procedure ActivateNET(S: string);
begin
  PT4MakerNetX.ActivateNET(S);
end;
/// ����������� � ����������� ������ ������������ �������
/// �� ������ GroupName � ������� TaskNumber. ������ ����������
/// � ��������� � ������, ������������ � ������ Task.
class procedure UseTask(GroupName: string; TaskNumber: integer);
begin
  PT4MakerNetX.UseTask(GroupName, TaskNumber);
end;
/// ���������� ������ �� 116 ������� ����.
class function GetWords: array of string;
begin
  result := PT4MakerNetX.GetWords;
end;
/// ���������� ������ �� 116 ���������� ����.
class function GetEnWords: array of string;
begin
  result := PT4MakerNetX.GetEnWords;
end;
/// ���������� ������ �� 61 �������� �����������.
class function GetSentences: array of string;
begin
  result := PT4MakerNetX.GetSentences;
end;
/// ���������� ������ �� 61 ����������� �����������.
class function GetEnSentences: array of string;
begin
  result := PT4MakerNetX.GetEnSentences;
end;
/// ���������� ������ �� 85 ������� ������������� �������.
/// ������ ������ ������ ������������ ��������� #13#10.
class function GetTexts: array of string;
begin
  result := PT4MakerNetX.GetTexts;
end;
/// ���������� ������ �� 85 ���������� ������������� �������.
/// ������ ������ ������ ������������ ��������� #13#10.
class function GetEnTexts: array of string;
begin
  result := PT4MakerNetX.GetEnTexts;
end;
/// ���������� ��������� ������� ����� �� �������, 
/// ��������� � ����������� ������� �������.
class function RandomWord: string;
begin
  result := PT4MakerNetX.RandomWord;
end;
/// ���������� ��������� ���������� ����� �� �������, 
/// ��������� � ����������� ������� �������.
class function RandomEnWord: string;
begin
  result := PT4MakerNetX.RandomEnWord;
end;
/// ���������� ��������� ������� ����������� �� �������, 
/// ��������� � ����������� ������� �������.
class function RandomSentence: string;
begin
  result := PT4MakerNetX.RandomSentence;
end;
/// ���������� ��������� ���������� ����������� �� �������, 
/// ��������� � ����������� ������� �������.
class function RandomEnSentence: string;
begin
  result := PT4MakerNetX.RandomEnSentence;
end;
/// ���������� ��������� ������� ������������� ����� �� �������, 
/// ��������� � ����������� ������� �������.
/// ������ ������ ������ ������������ ��������� #13#10.
class function RandomText: string;
begin
  result := PT4MakerNetX.RandomText;
end;
/// ���������� ��������� ���������� ������������� ����� �� �������, 
/// ��������� � ����������� ������� �������.
/// ������ ������ ������ ������������ ��������� #13#10.
class function RandomEnText: string;
begin
  result := PT4MakerNetX.RandomEnText;
end;

/// ��������� � ������� �������� ���� ����� �����
/// � ������ FileName � ���������� ��� ����������
/// � ������� �������� ������.
class procedure DataFileInteger(FileName: string);
begin
  PT4MakerNetX.DataFileInteger(FileName);
end;
/// ��������� � ������� �������� ���� ������������ �����
/// � ������ FileName � ���������� ��� ����������
/// � ������� �������� ������.
class procedure DataFileReal(FileName: string);
begin
  PT4MakerNetX.DataFileReal(FileName);
end;
/// ��������� � ������� �������� ���������� ����
/// � ������ FileName � ���������� ��� ����������
/// � ������� �������� ������. ������� ������
/// ��������� � ����� � ����������� ���������.
class procedure DataFileChar(FileName: string);
begin
  PT4MakerNetX.DataFileChar(FileName);
end;
/// ��������� � ������� �������� ��������� ����
/// � ���������� ���� ShortString � ������ FileName 
/// � ���������� ��� ���������� � ������� �������� ������.
/// ����� ��������� ����� �� ������ ������������ 70 ��������.
/// ������ ������ ��������� � ����� � ����������� ���������.
class procedure DataFileString(FileName: string);
begin
  PT4MakerNetX.DataFileString(FileName);
end;
/// ��������� � ������� �������� ��������� ����
/// � ������ FileName � ���������� ��� ����������
/// � ������� �������� ������. ����� ������ ������
/// ���������� ����� �� ������ ������������ 70 ��������.
/// ����� ������ ��������� � ����� � ����������� ���������.
class procedure DataText(FileName: string; LineCount: integer := 4);
begin
  PT4MakerNetX.DataText(FileName);
end;

/// ��������� � ������� �������������� ���� ����� �����
/// � ������ FileName � ���������� ��� ���������� � ������� �����������.
class procedure ResFileInteger(FileName: string);
begin
  PT4MakerNetX.ResFileInteger(FileName);
end;
/// ��������� � ������� �������������� ���� ������������ �����
/// � ������ FileName � ���������� ��� ���������� � ������� �����������.
class procedure ResFileReal(FileName: string);
begin
  PT4MakerNetX.ResFileReal(FileName);
end;
/// ��������� � ������� �������������� ���������� ����
/// � ������ FileName � ���������� ��� ���������� � ������� �����������.
/// ������� ������ ��������� � ����� � ����������� ���������.
class procedure ResFileChar(FileName: string);
begin
  PT4MakerNetX.ResFileChar(FileName);
end;
/// ��������� � ������� �������������� ��������� ����
/// � ���������� ���� ShortString � ������ FileName 
/// � ���������� ��� ���������� � ������� �����������.
/// ����� ��������� ����� �� ������ ������������ 70 ��������.
/// ������ ������ ��������� � ����� � ����������� ���������.
class procedure ResFileString(FileName: string);
begin
  PT4MakerNetX.ResFileString(FileName);
end;
/// ��������� � ������� �������������� ��������� ����
/// � ������ FileName � ���������� ��� ���������� � ������� �����������. 
/// ����� ������ ������ ���������� ����� �� ������ ������������ 70 ��������.
/// ����� ������ ��������� � ����� � ����������� ���������.
class procedure ResText(FileName: string; LineCount: integer := 5);
begin
  PT4MakerNetX.ResText(FileName);
end;
end;


implementation
{$reference system.windows.forms.dll}
uses System.Reflection, PT4TaskMakerNET, System.Windows.Forms, System;

const
  alphabet = '0123456789abcdefghijklmnopqrstuvwxyz';
  ErrMes1 = 'Error: ������ �������� ����� 5 ����� �� ����� ��������� �������� ������.';
  ErrMes2 = 'Error: ��� ������� �������� ������ ������ �� ����� ��������� ����� 5 �����.';
  ErrMes3 = 'Error: ���������� �������� ������ ��������� 200.';
  ErrMes4 = 'Error: ���������� �������������� ������ ��������� 200.';
  ErrMes5 = 'Error: ��� ����������� ������� ������ ������ ���������� ��������� NewTask.';
  ErrMes6 = 'Error: ��� ����������� ������� �� ������� �������� ������.';
  ErrMes7 = 'Error: ��� ����������� ������� �� ������� �������������� ������.';

var
  yd, yr, ye, nd, nr, pr, wd: integer;
  nt, ut, fd, fr: boolean;
  fmt: string;
  tasks := new List<MethodInfo>(100);

procedure Show(s: string);
begin
  MessageBox.Show(s, 'Error', MessageBoxButtons.OK, MessageBoxIcon.Error);
end;

function ErrorMessage(s: string): string;
begin
  result := Copy(s + new string(' ', 100), 1, 78);
end;

procedure ErrorInfo(s: string);
begin
  PT4TaskMakerNET.TaskText('\B'+ErrorMessage(s)+'\b', 0, ye);
  ye := ye + 1;
  if ye > 5 then ye := 0;
end;

function CheckTT: boolean;
begin
  result := ut;
  if not nt then
  begin
    NewTask('');
    ErrorInfo(ErrMes5);
  end;  
end;

function RandomName(len: integer): string;
begin
  result := ArrRandom(len, 1, alphabet.Length)
    .Select(e -> alphabet[e]).JoinIntoString('');
end;

procedure SetPrecision(n: integer);
begin
  if CheckTT then exit;
  if abs(n) > 10 then exit;
  pr := n;
  if n < 0 then
  begin
    fmt := 'e' + IntToStr(-n);
    n := 0;
  end
  else if n = 0 then
    fmt := 'e6'
  else
    fmt := 'f' + IntToStr(n); 
  PT4TaskMakerNET.SetPrecision(n);
end;

procedure SetWidth(n: integer);
begin
  if (n >= 0) and (n <= 20) then
    wd := n;
end;

procedure NewTask(topic, tasktext: string);
begin
  if nt then exit;
  PT4TaskMakerNET.CreateTask(topic);
  PT4TaskMakerNET.TaskText(tasktext);
  nt := true;  // ������� ��������� NewTask
  ut := false; // ���� ���������� ������������ ������� (���������� UseTask)
  ye := 1;     // ������� ����� ������ ��� ������ ��������� �� ������
  yd := 0;     // ������� ����� ������ � ������� �������� ������
  yr := 0;     // ������� ����� ������ � ������� �����������
  nd := 0;     // ���������� ��������� �������� ������
  nr := 0;     // ���������� ��������� �������������� ������
  fd := false; // ������� �������� ������ � ������� �������� ������
  fr := false; // ������� �������� ������ � ������� �����������
  pr := 2;     // ������� �������� ������ ������������ ������
  fmt := 'f2'; // ������� ������ ������ ������������ ������
  wd := 0;     // ������� ������ ���� ������ ��� �����
end;

function wreal(w: integer; x: real): integer;
begin
  result := w;
  if w = 0 then
  begin
    result := Format('{0,0:' + fmt + '}', x).Length;
    if (pr <= 0) and (x >= 0) then
      result := result + 1;
  end;
end;

function winteger(w: integer; x: integer): integer;
begin
  result := w;
  if w = 0 then
    result := IntToStr(x).Length;
end;

procedure NewTask(tasktext: string);
begin
  NewTask('', tasktext);
end;

procedure Data(s: string; a: object; x, y, w: integer);
begin
  if (y > 5) and fd then
  begin
    ErrorInfo(ErrMes2);
    exit;  
  end;
  inc(nd);
  if nd > 200 then
  begin
    ErrorInfo(ErrMes3);
    exit;  
  end;
  if a is boolean then
    DataB(s, boolean(a), x, y)
  else if a is integer then
    DataN(s, integer(a), x, y, winteger(w, integer(a)))
  else if a is real then
    DataR(s, real(a), x, y, wreal(w, real(a)))
  else if a is char then
    DataC(s, char(a), x, y)
  else if a is string then
    DataS(s, string(a), x, y)
  else
    DataComment(Copy(s + '!WrongType:' + a.GetType.Name, 1, 38), x, y);
end;

procedure Res(s: string; a: object; x, y, w: integer);
begin
  if (y > 5) and fr then
  begin
    ErrorInfo(ErrMes2);
    exit;  
  end;
  inc(nr);
  if nr > 200 then
  begin
    ErrorInfo(ErrMes4);
    exit;  
  end;
  if a is boolean then
    ResultB(s, boolean(a), x, y)
  else if a is integer then
    ResultN(s, integer(a), x, y, winteger(w, integer(a)))
  else if a is real then
    ResultR(s, real(a), x, y, wreal(w, real(a)))
  else if a is char then
    ResultC(s, char(a), x, y)
  else if a is string then
    ResultS(s, string(a), x, y)  
  else
    ResultComment(Copy(s + '!WrongType:' + a.GetType.Name, 1, 38), x, y);
end;

procedure DataComm(comm: string);
begin
  if CheckTT then exit;
  inc(yd);
  DataComment(comm, 0, yd);
end;

procedure Data(comm: string; a: object);
begin
  if CheckTT then exit;
  inc(yd);
  Data(comm, a, 0, yd, wd);
end;

procedure Data(comm1: string; a1: object; comm2: string);
begin
  if CheckTT then exit;
  inc(yd);
  Data(comm1, a1, xLeft, yd, wd);
  DataComment(comm2, xRight, yd);
end;

procedure Data(comm1: string; a1: object; comm2: string; a2: object);
begin
  if CheckTT then exit;
  inc(yd);
  Data(comm1, a1, xLeft, yd, wd);
  Data(comm2, a2, xRight, yd, wd);
end;

procedure Data(comm1: string; a1: object; comm2: string; a2: object; comm3: string);
begin
  if CheckTT then exit;
  inc(yd);
  Data(comm1, a1, xLeft, yd, wd);
  Data(comm2, a2, 0, yd, wd);
  DataComment(comm3, xRight, yd);
end;

procedure Data(comm1: string; a1: object; comm2: string; a2: object; comm3: string; a3: object);
begin
  if CheckTT then exit;
  inc(yd);
  Data(comm1, a1, xLeft, yd, wd);
  Data(comm2, a2, 0, yd, wd);
  Data(comm3, a3, xRight, yd, wd);
end;

procedure Data(seq: sequence of boolean);
begin
  if CheckTT then exit;
  var n := seq.Count;
  if n = 0 then exit;
  inc(yd);
  var w := 5;
  var wmax := 80 div (w + 2);
  if n > wmax then
    n := wmax;
  var i := 0;
  foreach var e in seq do
  begin
    inc(i);
    if i > wmax then
    begin
      inc(yd);
      i := 1;
    end;
    Data('', e, Center(i, n, w, 2), yd, w);
  end;  
end;

procedure Data(seq: sequence of integer);
begin
  if CheckTT then exit;
  var n := seq.Count;
  if n = 0 then exit;
  inc(yd);
  var w := wd;
  if w = 0 then
    w := seq.Select(e -> IntToStr(e)).Max(e -> e.Length);
  var wmax := 80 div (w + 2);
  if n > wmax then
    n := wmax;
  var i := 0;
  foreach var e in seq do
  begin
    inc(i);
    if i > wmax then
    begin
      inc(yd);
      i := 1;
    end;
    Data('', e, Center(i, n, w, 2), yd, w);
  end;  
end;

procedure Data(seq: sequence of real);
begin
  if CheckTT then exit;
  var n := seq.Count;
  if n = 0 then exit;
  inc(yd);
  var w := wd;
  if w = 0 then
    w := seq.Select(e -> wreal(0, e)(*Format('{0,0:'+fmt+'}', e)*)).Max;
  var wmax := 80 div (w + 2);
  if n > wmax then
    n := wmax;
  var i := 0;
  foreach var e in seq do
  begin
    inc(i);
    if i > wmax then
    begin
      inc(yd);
      i := 1;
    end;
    Data('', e, Center(i, n, w, 2), yd, w);
  end;  
end;

procedure Data(seq: sequence of char);
begin
  if CheckTT then exit;
  var n := seq.Count;
  if n = 0 then exit;
  inc(yd);
  var w := 3;
  var wmax := 80 div (w + 2);
  if n > wmax then
    n := wmax;
  var i := 0;
  foreach var e in seq do
  begin
    inc(i);
    if i > wmax then
    begin
      inc(yd);
      i := 1;
    end;
    Data('', e, Center(i, n, w, 2), yd, w);
  end;  
end;

procedure Data(seq: sequence of string);
begin
  if CheckTT then exit;
  var n := seq.Count;
  if n = 0 then exit;
  inc(yd);
  var w := seq.Max(e -> e.Length) + 2;
  var wmax := 80 div (w + 2);
  if n > wmax then
    n := wmax;
  var i := 0;
  foreach var e in seq do
  begin
    inc(i);
    if i > wmax then
    begin
      inc(yd);
      i := 1;
    end;
    Data('', e, Center(i, n, w, 2), yd, w);
  end;  
end;

procedure ResComm(comm: string);
begin
  if CheckTT then exit;
  inc(yr);
  ResultComment(comm, 0, yr);
end;

procedure Res(comm: string; a: object);
begin
  if CheckTT then exit;
  inc(yr);
  Res(comm, a, 0, yr, wd);
end;

procedure Res(comm1: string; a1: object; comm2: string);
begin
  if CheckTT then exit;
  inc(yr);
  Res(comm1, a1, xLeft, yr, wd);
  ResultComment(comm2, xRight, yr);
end;

procedure Res(comm1: string; a1: object; comm2: string; a2: object);
begin
  if CheckTT then exit;
  inc(yr);
  Res(comm1, a1, xLeft, yr, wd);
  Res(comm2, a2, xRight, yr, wd);
end;

procedure Res(comm1: string; a1: object; comm2: string; a2: object; comm3: string);
begin
  if CheckTT then exit;
  inc(yr);
  Res(comm1, a1, xLeft, yr, wd);
  Res(comm2, a2, 0, yr, wd);
  ResultComment(comm3, xRight, yr);
end;

procedure Res(comm1: string; a1: object; comm2: string; a2: object; comm3: string; a3: object);
begin
  if CheckTT then exit;
  inc(yr);
  Res(comm1, a1, xLeft, yr, wd);
  Res(comm2, a2, 0, yr, wd);
  Res(comm3, a3, xRight, yr, wd);
end;

procedure Res(seq: sequence of boolean);
begin
  if CheckTT then exit;
  var n := seq.Count;
  if n = 0 then exit;
  inc(yr);
  var w := 5;
  var wmax := 80 div (w + 2);
  if n > wmax then
    n := wmax;
  var i := 0;
  foreach var e in seq do
  begin
    inc(i);
    if i > wmax then
    begin
      inc(yd);
      i := 1;
    end;
    Res('', e, Center(i, n, w, 2), yr, w);
  end;  
end;

procedure Res(seq: sequence of integer);
begin
  if CheckTT then exit;
  var n := seq.Count;
  if n = 0 then exit;
  inc(yr);
  var w := wd;
  if w = 0 then
    w := seq.Select(e -> IntToStr(e)).Max(e -> e.Length);
  var wmax := 80 div (w + 2);
  if n > wmax then
    n := wmax;
  var i := 0;
  foreach var e in seq do
  begin
    inc(i);
    if i > wmax then
    begin
      inc(yd);
      i := 1;
    end;
    Res('', e, Center(i, n, w, 2), yr, w);
  end;  
end;

procedure Res(seq: sequence of real);
begin
  if CheckTT then exit;
  var n := seq.Count;
  if n = 0 then exit;
  inc(yr);
  var w := wd;
  if w = 0 then
    w := seq.Select(e -> wreal(0, e)).Max;
  var wmax := 80 div (w + 2);
  if n > wmax then
    n := wmax;
  var i := 0;
  foreach var e in seq do
  begin
    inc(i);
    if i > wmax then
    begin
      inc(yd);
      i := 1;
    end;
    Res('', e, Center(i, n, w, 2), yr, w);
  end;  
end;

procedure Res(seq: sequence of char);
begin
  if CheckTT then exit;
  var n := seq.Count;
  if n = 0 then exit;
  inc(yr);
  var w := 3;
  var wmax := 80 div (w + 2);
  if n > wmax then
    n := wmax;
  var i := 0;
  foreach var e in seq do
  begin
    inc(i);
    if i > wmax then
    begin
      inc(yd);
      i := 1;
    end;
    Res('', e, Center(i, n, w, 2), yr, w);
  end;  
end;

procedure Res(seq: sequence of string);
begin
  if CheckTT then exit;
  var n := seq.Count;
  if n = 0 then exit;
  inc(yr);
  var w := seq.Max(e -> e.Length) + 2;
  var wmax := 80 div (w + 2);
  if n > wmax then
    n := wmax;
  var i := 0;
  foreach var e in seq do
  begin
    inc(i);
    if i > wmax then
    begin
      inc(yd);
      i := 1;
    end;
    Res('', e, Center(i, n, w, 2), yr, w);
  end;  
end;


procedure ActivateNET(S: string);
begin
  PT4TaskMakerNET.ActivateNET(S);
end;

procedure RunTask(num: integer);
var ut0: boolean;
begin
  try
    try
    if (num > 0) and (num <= tasks.Count) then
      tasks[num - 1].Invoke(nil, nil);
    finally
      nt := false;
      ut0 := ut;
      ut := false;
    end;
  except
    on e: TargetInvocationException do
    begin
      ErrorInfo('Error ' 
          + e.InnerException.GetType.Name + ': '
          + e.InnerException.message);
    end;
  end;
  if ut0 then exit;
  if (nd = 0) and not fd then 
    DataS('\B'+Copy(ErrorMessage(ErrMes6),1,76)+'\b', '', 1, 1);
  if (nr = 0) and not fr then 
    ResultS('\B'+Copy(ErrorMessage(ErrMes7),1,76)+'\b', '', 1, 1);
end;

function GetGroupName(assname: string): string;
begin
  result := copy(assname, 1, pos(',', assname)-1);
  var p := pos('_', result);
  if p > 0 then
    delete(result, p, 100);
  delete(result, 1, 3);
end;

function AcceptedLanguage(opt: integer): boolean;
begin
  var lang := CurrentLanguage;
  result := (lang = lgPascalABCNET) 
    or (opt and OptionAllLanguages = OptionAllLanguages) 
    or (lang and lgNET <> 0) and (opt and OptionNETLanguages = OptionNETLanguages)
    or (lang and lgPascal = lgPascal) and (opt and OptionPascalLanguages = OptionPascalLanguages);
end;


procedure NewGroup(GroupDescription, GroupAuthor: string; Options: integer);

begin
  if not AcceptedLanguage(Options) then exit; // ������������ ������� ����
  var ass := Assembly.GetCallingAssembly;
  var GroupName := GetGroupName(ass.FullName);
  var nm := 'PT4' + GroupName;
  tasks.Clear;
  var GroupKey := 'GK';
  foreach var e in ass.GetType(nm + '.' + nm).GetMethods do
    if e.Name.ToUpper.StartsWith('TASK') then
    begin
      tasks.Add(e);
      GroupKey := GroupKey + Copy(e.Name, 5, 100);
    end;  
  if tasks.Count = 0 then
  begin
    Show('������ ' + GroupName + ' �� �������� �������'#13#10+
    '(����� �������� � ��������� ������ ���������� � ������ "Task").');
    exit;
  end;
  if Options and OptionUseAddition = OptionUseAddition then
    GroupKey := GroupKey + '#UseAddition#';
  if Options and OptionHideExamples = OptionHideExamples then
    GroupKey := GroupKey + '#HideExamples#';
  CreateGroup(GroupName, GroupDescription, GroupAuthor, 
      GroupKey, tasks.Count, RunTask);
end;


procedure SetTestCount(n: integer);
begin
  if CheckTT then exit;
  PT4TaskMakerNET.SetTestCount(n);
end;

procedure SetRequiredDataCount(n: integer);
begin
  if CheckTT then exit;
  PT4TaskMakerNET.SetRequiredDataCount(n);
end;

function Random(M, N: integer): integer;
begin
  result := PT4TaskMakerNET.RandomN(M, N);
end;

function Random(A, B: real): real;
begin
  result := PT4TaskMakerNET.RandomR(A, B);
end;

function CurrentTest: integer;
begin
  if CheckTT then exit;
  result := PT4TaskMakerNET.CurrentTest;
end;

procedure UseTask(GroupName: string; TaskNumber: integer);
begin
  if ut then exit;
  PT4TaskMakerNET.UseTask(GroupName, TaskNumber);
  ut := true;
end;

function GetWords: array of string;
begin
  Result := ArrGen(WordCount, i -> WordSample(i));
end;

function RandomWord: string;
begin
  Result := WordSample(RandomN(0, WordCount - 1));
end;

function GetEnWords: array of string;
begin
  Result := ArrGen(EnWordCount, i -> EnWordSample(i));
end;

function RandomEnWord: string;
begin
  Result := EnWordSample(RandomN(0, EnWordCount - 1));
end;

function GetSentences: array of string;
begin
  Result := ArrGen(SentenceCount, i -> SentenceSample(i));
end;

function RandomSentence: string;
begin
  Result := SentenceSample(RandomN(0, SentenceCount - 1));
end;

function GetEnSentences: array of string;
begin
  Result := ArrGen(EnSentenceCount, i -> EnSentenceSample(i));
end;

function RandomEnSentence: string;
begin
  Result := EnSentenceSample(RandomN(0, EnSentenceCount - 1));
end;

function GetTexts: array of string;
begin
  Result := ArrGen(TextCount, i -> TextSample(i));
end;

function RandomText: string;
begin
  Result := TextSample(RandomN(0, TextCount - 1));
end;

function GetEnTexts: array of string;
begin
  Result := ArrGen(EnTextCount, i -> EnTextSample(i));
end;

function RandomEnText: string;
begin
  Result := EnTextSample(RandomN(0, EnTextCount - 1));
end;

procedure DataFileInteger(FileName: string);
begin
  if CheckTT then exit;
  inc(yd);
  if yd > 5 then
  begin
    DataComm('\B'+ErrorMessage(ErrMes1)+'\b');
    exit;
  end;
  var w := 0;
  try
    var f: file of integer;
    Reset(f, FileName);
    while not EOF(f) do
    begin
      var a: integer;
      read(f, a);
      var s := IntToStr(a);
      if s.Length > w then
        w := s.Length;
    end;
    Close(f);
  except
    on ex: Exception do
    begin
      DataComm('\B'+ErrorMessage('FileError(' + FileName + '): ' + ex.Message)+'\b');
      exit;
    end;  
  end;
  fd := true;
  DataFileN(FileName, yd, w + 2);
end;

procedure DataFileReal(FileName: string);
begin
  if CheckTT then exit;
  inc(yd);
  if yd > 5 then
  begin
    DataComm('\B'+ErrorMessage(ErrMes1)+'\b');
    exit;
  end;
  var w := 0;
  try
    var f: file of real;
    Reset(f, FileName);
    while not EOF(f) do
    begin
      var a: real;
      read(f, a);
      var s := wreal(0, a);
      if s > w then
        w := s;
    end;
    Close(f);
  except
    on ex: Exception do
    begin
      DataComm('\B'+ErrorMessage('FileError(' + FileName + '): ' + ex.Message)+'\b');
      exit;
    end;  
  end;
  fd := true;
  DataFileR(FileName, yd, w + 2);
end;

procedure DataFileChar(FileName: string);
begin
  if CheckTT then exit;
  inc(yd);
  if yd > 5 then
  begin
    DataComm('\B'+ErrorMessage(ErrMes1)+'\b');
    exit;
  end;
  fd := true;
  DataFileC(FileName, yd, 5);
end;

procedure DataFileString(FileName: string);
begin

  if CheckTT then exit;
  inc(yd);
  if yd > 5 then
  begin
    DataComm('\B'+ErrorMessage(ErrMes1)+'\b');
    exit;
  end;
  var w := 0;
  try
    var f: file of ShortString;
    Reset(f, FileName);
    while not EOF(f) do
    begin
      var a: ShortString;
      read(f, a);
      if Length(a) > w then
        w := Length(a);
    end;
    Close(f);
  except
    on ex: Exception do
    begin
      DataComm('\B'+ErrorMessage('FileError(' + FileName + '): ' + ex.Message)+'\b');
      exit;
    end;  
  end;
  fd := true;
  DataFileS(FileName, yd, w + 4);
  
end;

procedure DataText(FileName: string; LineCount: integer);
begin
  if CheckTT then exit;
  inc(yd);
  if yd > 5 then
  begin
    DataComm('\B'+ErrorMessage(ErrMes1)+'\b');
    exit;
  end;
  fd := true;
  var yd2 := yd + LineCount - 1;
  if yd2 > 5 then yd2 := 5;
  DataFileT(FileName, yd, yd2);
  yd := yd2;
end;

procedure ResFileInteger(FileName: string);
begin
  if CheckTT then exit;
  inc(yr);
  if yr > 5 then
  begin
    ResComm('\B'+ErrorMessage(ErrMes1)+'\b');
    exit;
  end;
  var w := 0;
  try
    var f: file of integer;
    Reset(f, FileName);
    while not EOF(f) do
    begin
      var a: integer;
      read(f, a);
      var s := IntToStr(a);
      if s.Length > w then
        w := s.Length;
    end;
    Close(f);
  except
    on ex: Exception do
    begin
      ResComm('\B'+ErrorMessage('FileError(' + FileName + '): ' + ex.Message)+'\b');
      exit;
    end;  
  end;
  fr := true;
  ResultFileN(FileName, yr, w + 2);
end;

procedure ResFileReal(FileName: string);
begin
  if CheckTT then exit;
  ResComm(fmt);
  inc(yr);
  if yr > 5 then
  begin
    ResComm('\B'+ErrorMessage(ErrMes1)+'\b');
    exit;
  end;
  var w := 0;
  try
    var f: file of real;
    Reset(f, FileName);
    while not EOF(f) do
    begin
      var a: real;
      read(f, a);
      var s := wreal(0, a);
      if s > w then
        w := s;
    end;
    Close(f);
  except
    on ex: Exception do
    begin
      ResComm('\B'+ErrorMessage('FileError(' + FileName + '): ' + ex.Message)+'\b');
      exit;
    end;  
  end;
  fr := true;
  ResultFileR(FileName, yr, w + 2);
end;

procedure ResFileChar(FileName: string);
begin
  if CheckTT then exit;
  inc(yr);
  if yr > 5 then
  begin
    ResComm('\B'+ErrorMessage(ErrMes1)+'\b');
    exit;
  end;
  fr := true;
  ResultFileC(FileName, yr, 5);
end;

procedure ResFileString(FileName: string);
begin
  if CheckTT then exit;
  inc(yr);
  if yr > 5 then
  begin
    ResComm('\B'+ErrorMessage(ErrMes1)+'\b');
    exit;
  end;
  var w := 0;
  try
    var f: file of ShortString;
    Reset(f, FileName);
    while not EOF(f) do
    begin
      var a: ShortString;
      read(f, a);
      if Length(a) > w then
        w := Length(a);
    end;
    Close(f);
  except
    on ex: Exception do
    begin
      ResComm('\B'+ErrorMessage('FileError(' + FileName + '): ' + ex.Message)+'\b');
      exit;
    end;  
  end;
  fr := true;
  ResultFileS(FileName, yr, w + 4);
end;

procedure ResText(FileName: string; LineCount: integer);
begin
  if CheckTT then exit;
  inc(yr);
  if yr > 5 then
  begin
    ResComm('\B'+ErrorMessage(ErrMes1)+'\b');
    exit;
  end;
  fr := true;
  var yr2 := yr + LineCount - 1;
  if yr2 > 5 then yr2 := 5;
  ResultFileT(FileName, yr, yr2);
  yr := yr2;
end;

end.