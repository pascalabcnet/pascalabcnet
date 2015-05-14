module Main where

g z |z > 0 = 1
g z |z < 0 = -1
g z | otherwise = 0

main = do { 
              (print(g 5));
              (print(g -5));
              (print(g 0));
          }