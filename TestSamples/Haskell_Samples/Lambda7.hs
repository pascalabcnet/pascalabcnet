module Main where

p a x y = (a x y)+y

pp b x = (p b (b x x))

f x y = x*y

main = do {
            (print(pp f 2 3));
          }