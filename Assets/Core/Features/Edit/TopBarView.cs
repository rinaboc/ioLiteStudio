using System;
using Core.Domain;
using UnityEngine.UIElements;

namespace Core.Features
{
    public class TopBarView : View<TopBarUIState>
    {
        private Button _undoButton;
        private Button _redoButton;

        public TopBarView(IViewModel<TopBarUIState> viewModel, VisualElement topBarElement) : base(viewModel)
        {
            _undoButton = topBarElement.Q<Button>("UndoBtn");
            _redoButton = topBarElement.Q<Button>("RedoBtn");

            _undoButton.clicked += OnUndoClicked;
            _redoButton.clicked += OnRedoClicked;
        }

        public override void Dispose()
        {
            base.Dispose();
            _undoButton.clicked -= OnUndoClicked;
            _redoButton.clicked -= OnRedoClicked;
        }

        private void OnRedoClicked()
        {
            _viewModel.Dispatch(new RedoIntent());
        }

        private void OnUndoClicked()
        {
            _viewModel.Dispatch(new UndoIntent());
        }

        public override void Render(TopBarUIState state)
        {
            _undoButton.SetEnabled(state.CanUndo);
            _redoButton.SetEnabled(state.CanRedo);
        }
    }
}
