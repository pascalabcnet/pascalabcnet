module Main where

ff x = x+1

map a [] = []
map a h:t = (a h):(map a t)

main = do {
            (print(map ff [1,3,8,11]));
            (print(map ff []));
          }