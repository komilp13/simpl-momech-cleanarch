using System;

namespace ApplicationTests;

public class TestDataWrapper<T, TExp>
{
    public T? Value { get; set; }
    public TExp? Expected { get; set; }
}
