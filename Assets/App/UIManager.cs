using Core.Features;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;

namespace Core.Services
{
    public interface IUIManager
    {
        void InitializeViews();
    }

    public class UIManager : MonoBehaviour, IUIManager
    {
        [SerializeField] private PanelRenderer _panelRenderer;

        public void InitializeViews()
        {
            _panelRenderer.RegisterUIReloadCallback(OnUIReload);
        }

        void Awake()
        {
            Assert.NotNull(_panelRenderer);
        }

        private void OnUIReload(PanelRenderer panelRenderer, VisualElement rootElement)
        {
            var viewModelManager = ServiceLocator.Get<IViewModelManager>();

            if (viewModelManager.TryGet<TopBarUIState>(out var topBarViewmodel))
            {
                var viewModel = topBarViewmodel[0];
                viewModel.Bind(new TopBarView(viewModel, rootElement.Q<VisualElement>("TopBar")));
            }
        }
    }
}
