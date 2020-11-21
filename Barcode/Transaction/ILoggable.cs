using System;

namespace Barcode
{
    public interface ILoggable<I>
    {
        static event Action<I> LogCommand;
    }
}