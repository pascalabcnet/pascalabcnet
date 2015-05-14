module Main where

fib = \x -> if (x > 1) then (fib x-1) + (fib x-2) else 1

main = do (print(fib 0))
          (print(fib 1))
          (print(fib 2))
          (print(fib 3))
          (print(fib 4))
          (print(fib 5))
          (print(fib 6))
          (print(fib 7))