module Main where

--ex b 1 = b
--ex b n = b * ex(b n - 1)

--f 0 a = a
--f n a = f(n-1 n*a)

fib x = if (x > 1) then (fib x-1) + (fib x-2) else 1

main = do { 
              --print ex(3 1024);
              --print f(10 1);
              (print(fib 4));
          }