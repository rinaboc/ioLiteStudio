using System;
using Core.Domain;
using Core.Features;
using UnityEngine.UIElements;

namespace UI.UIToolkit
{
    public abstract class InspectorView<TState> : View<TState> where TState : InspectorUIState
    {
        protected Button _addButton, _deleteButton;
        private Toggle _inspectorToggle;
        protected VisualElement _listElement;

        protected abstract string InspectorLabelText { get; }
        protected abstract Widget InspectorWidget { get; }

        private VisualElement _inspectorCommands;
        private VisualElement _container;

        protected InspectorView(IViewModel<TState> viewModel, VisualElement inspectorRoot) : base(viewModel)
        {
            inspectorRoot.Q<Label>("InspectorTitleLabel").text = InspectorLabelText;

            _addButton = inspectorRoot.Q<Button>("AddBtn");
            _deleteButton = inspectorRoot.Q<Button>("DeleteBtn");

            _listElement = inspectorRoot.Q<VisualElement>("List");

            _inspectorToggle = inspectorRoot.Q<Toggle>("InspectorToggle");

            _inspectorCommands = inspectorRoot.Q<VisualElement>("InspectorCommands");
            _container = inspectorRoot.Q<VisualElement>("Container");

            _addButton.clicked += OnAddClicked;
            _deleteButton.clicked += OnDeleteClicked;
            _inspectorToggle.RegisterValueChangedCallback(OnInspectorToggled);
        }

        private void OnInspectorToggled(ChangeEvent<bool> evt)
        {
            _viewModel?.Dispatch(new WidgetToggleIntent(InspectorWidget, evt.newValue));
        }

        public override void Render(TState state)
        {
            if (state.IsInspectorOpen)
            {
                _inspectorCommands.RemoveFromClassList("hide");
                _container.RemoveFromClassList("hide");
            }
            else
            {
                _inspectorCommands.AddToClassList("hide");
                _container.AddToClassList("hide");
            }
        }

        protected abstract void OnAddClicked();
        protected abstract void OnDeleteClicked();

        public override void Dispose()
        {
            base.Dispose();
            _addButton.clicked -= OnAddClicked;
            _deleteButton.clicked -= OnDeleteClicked;
        }

    }
}
