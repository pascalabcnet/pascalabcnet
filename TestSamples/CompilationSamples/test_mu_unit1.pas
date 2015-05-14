unit test_mu_unit1;

interface

uses test_mu_unit3, test_mu_unit2;

procedure proc1;

implementation

uses test_mu_unit4;

procedure proc1;
begin
 writeln('proc1');
 proc2;
end;
end.