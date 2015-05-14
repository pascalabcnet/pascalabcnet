module Main where

m x y = x*y

main = do {
             (print        ((\x y -> x*y) 4 7)          );
          }