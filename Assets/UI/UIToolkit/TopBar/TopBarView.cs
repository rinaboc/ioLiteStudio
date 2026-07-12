using System;
using Core.Domain;
using UnityEngine.UIElements;

namespace Core.Features
{
    public class TopBarView : View<TopBarUIState>
    {
        private Button _undoButton;
        private Button _redoButton;
        private Button _fileButton;

        public TopBarView(IViewModel<TopBarUIState> viewModel, VisualElement topBarElement) : base(viewModel)
        {
            _undoButton = topBarElement.Q<Button>("UndoBtn");
            _redoButton = topBarElement.Q<Button>("RedoBtn");
            _fileButton = topBarElement.Q<Button>("FileBtn");

            _undoButton.clicked += OnUndoClicked;
            _redoButton.clicked += OnRedoClicked;
            _fileButton.clicked += OnFileClicked;
        }

        public override void Dispose()
        {
            base.Dispose();
            _undoButton.clicked -= OnUndoClicked;
            _redoButton.clicked -= OnRedoClicked;
            _fileButton.clicked -= OnFileClicked;
        }

        private void OnFileClicked()
        {
            _viewModel.Dispatch(new OpenWindowIntent(WindowType.File));
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
