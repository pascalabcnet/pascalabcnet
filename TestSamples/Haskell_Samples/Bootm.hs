module Main where

f 1 _ = 1
f _ a = a

lenght []  = 0
lenght _:t = (lenght t) + 1

main = do {
            (print(f 1 2));
            (print(f 5 2));
            (print(lenght [1,2,3,4,5,7,8,9]));
          }