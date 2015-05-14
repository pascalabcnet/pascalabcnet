module Main where

f = do d <- getChar 
       (print d+"456")

main = do     d <- getChar
              (print d+"456")
              f