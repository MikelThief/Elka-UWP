using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Usos.Services;
using Prism.Mvvm;
using Prism.Navigation;

namespace ElkaUWP.Modularity.GradesModule.ViewModels
{
    public class TestViewModel : BindableBase, INavigationAware
    {
        private TestsService _testsService;

        public TestViewModel(TestsService testsService)
        {
            _testsService = testsService;
        }
        /// <inheritdoc />
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }

        /// <inheritdoc />
        public void OnNavigatedTo(INavigationParameters parameters)
        {
            
        }

        /// <inheritdoc />
        public void OnNavigatingTo(INavigationParameters parameters)
        {
            
        }
    }
}
