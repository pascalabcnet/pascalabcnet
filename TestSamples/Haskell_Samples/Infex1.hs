module Main where

a `plus` b = a+b 

f = ((3 `plus` 2) `plus` 5)

g = f

main = do { 
              (print g);
          }