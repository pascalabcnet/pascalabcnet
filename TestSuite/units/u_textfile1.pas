unit u_textfile1;
procedure Test;
var f : Text;
    s : string;
begin
Assign(f,'test.txt');
Rewrite(f);
Writeln(f,'abc');
s := 'ggg';
Writeln(f,s);
Close(f);
Reset(f);
Reset(f);
Readln(f,s);
assert(s='abc');
Readln(f,s);
assert(s='ggg');
Close(f);
end;

procedure Test2;
var f : Text;
    s : string;
procedure Nested;
begin
Assign(f,'test.txt');
Rewrite(f);
assert(s='ggg');
Writeln(f,'abc');
s := 'ggg';
Writeln(f,s);
Close(f);
Reset(f);
Reset(f);
Readln(f,s);
assert(s='abc');
Readln(f,s);
assert(s='ggg');
Close(f);
end;

begin
Assign(f,'test.txt');
Rewrite(f);
Writeln(f,'abc');
s := 'ggg';
assert(s='ggg');
Writeln(f,s);
Close(f);
Reset(f);
Reset(f);
Readln(f,s);
assert(s='abc');
Readln(f,s);
assert(s='ggg');
Close(f);
Nested;
end;

var f : Text;
    s : string;
    
begin
Assign(f,'test.txt');
Rewrite(f);
Writeln(f,'abc');
s := 'ggg';
Writeln(f,s);
Close(f);
Reset(f);
Reset(f);
Readln(f,s);
assert(s='abc');
Readln(f,s);
assert(s='ggg');
Close(f);
Test;
Test2;
end.