using System;

namespace Simpl.Mobile.Mechanic.Core.Exceptions;

public class DatabaseException : Exception
{
  public DatabaseException(string message)
      : base(message)
  {
  }

  public DatabaseException(string message, Exception inner)
      : base(message, inner)
  {
  }
}
