using System;

namespace NonogramSolver.Data;

public class NonogramException : Exception
{
    public NonogramException(string message) : base(message) { }
}