unit program68;

begin
foreach c : char in 'abcd' do
begin
if c = 'c' then continue;
writeln(c);
end;

end.