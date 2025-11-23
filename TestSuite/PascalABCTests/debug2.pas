{$ifdef asd}
{$resource 'ddddd'}
{$endif asd}

{$define asd}
{$ifdef asd}
{$ifdef asd2}
{$resource 'ddddd'}
{$endif asd2}
{$endif asd}

{$define asd3}
{$undef asd3}
{$ifdef asd3}
{$resource 'НеСуществующийФайл'}
{$endif asd3}

begin
  
end.