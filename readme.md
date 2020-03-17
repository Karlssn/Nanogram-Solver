# Nanogram solver
## Background

So recently, my girlfriend and I have started to play Nanogram on our phones which lead me to want to create a version and a nanogram solver. Instead of starting with the Nanogram project itself, I started with the solver. 

The nongrams I've played are either 10x10 or 15x15 so I have not taken account for non square nonograms. Although, one could add either 0 columns or 0 rows to make the nonogram square to use this solution.

## How it works
I used [this guide](https://coolbutuseless.github.io/2018/09/28/writing-a-nonogram-solver-in-r) to get me going. This program generates all possible patterns that are possible with the current row/ column. It then looks if the patterns overlap with filled blocks or crossed. 


## TODO:
* Use a joblist instead of whiles
* Refactoring
* Unittests
* Better way to input data
* Web application around this project...