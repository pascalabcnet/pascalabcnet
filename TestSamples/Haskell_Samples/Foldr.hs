module Main where

f x y = x+y

foldr a x [] = x
foldr a x h:t = (a h (foldr a x t))

main = do {
            (print(foldr f 2 []));
            (print(foldr f 1 [1,2,3]));
            (print(foldr f 2 [1..10]));
            (print(foldr f 2 [1,3..10]));
          }