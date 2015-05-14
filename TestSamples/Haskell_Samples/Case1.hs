module Main where

f a b = case (a, b) of (1, 2) -> 3 
                     | (a, b) -> a+b
                        
main = do { 
              (print(f 1 2));
              (print(f 5 3));
          }