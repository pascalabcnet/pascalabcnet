module Main where

sum 1 = 1
sum i = (sum i-1) + i

factor 1 = 1
factor i = (factor i-1)*i

main = do { 
              (print(sum 5));
              (print(factor 20));
          }