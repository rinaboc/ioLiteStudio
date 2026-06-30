using System;
using System.Collections.Generic;
using Core.Domain;

namespace Core.Services
{
    public class UndoService : IUndoService
    {
        private readonly DropOutStack<HistoryStep> _undoStack;
        private readonly Stack<HistoryStep> _redoStack = new();

        public UndoService(int undoCapacity)
        {
            _undoStack = new(undoCapacity);
        }

        public HistoryStep LastStep => _undoStack.Peek();
        public HistoryStep NextStep => _redoStack.Peek();

        public int UndoCount => _undoStack.Count;
        public int RedoCount => _redoStack.Count;

        public void PushStep(HistoryStep step)
        {
            _undoStack.Push(step);
            _redoStack.Clear();
        }

        public bool Redo(out Action mutation)
        {
            mutation = null;
            if (!_redoStack.TryPop(out var step)) return false;

            _undoStack.Push(step);
            mutation = step.CommandResult.Mutation;
            return true;
        }

        public bool Undo(out Action revert)
        {
            revert = null;
            if (!_undoStack.TryPop(out var step)) return false;

            _redoStack.Push(step);
            revert = step.CommandResult.Revert;
            return true;
        }
    }
}
