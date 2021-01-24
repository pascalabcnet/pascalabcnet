unit u;

{$savepcu false}

{$resource '1.txt'}
{$reference 'uui3_lib.dll'}

begin
  Assert('abc' = System.IO.StreamReader.Create(GetResourceStream('1.txt')).ReadToEnd);
end.