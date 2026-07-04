using System;
using Core.Domain;

namespace Core.Services
{
    public interface IUndoService
    {
        void PushStep(HistoryStep step);
        HistoryStep LastStep { get; }
        HistoryStep NextStep { get; }
        bool Undo(out Action revert);
        bool Redo(out Action mutation);
        int UndoCount { get; }
        int RedoCount { get; }
        void Clear();
    }
}
