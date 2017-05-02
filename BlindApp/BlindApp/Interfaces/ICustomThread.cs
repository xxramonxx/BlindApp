using System;
namespace BlindApp.Interfaces
{
    public interface ICustomThread
    {
        Action Thread { get; set; }
    }
}
