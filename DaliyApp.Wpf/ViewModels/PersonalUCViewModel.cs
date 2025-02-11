using MaterialDesignColors;
using MaterialDesignColors.ColorManipulation;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace DaliyApp.Wpf.ViewModels
{
    public class PersonalUCViewModel : BindableBase
    {
        #region 更改主题颜色

        private bool _isDarkTheme;

        public bool IsDarkTheme
        {
            get => _isDarkTheme;
            set
            {
                if (SetProperty(ref _isDarkTheme, value))
                {
                    ModifyTheme(theme => theme.SetBaseTheme(value ? BaseTheme.Dark : BaseTheme.Light));
                }
            }
        }

        private static void ModifyTheme(Action<Theme> modificationAction)
        {
            var paletteHelper = new PaletteHelper();
            Theme theme = paletteHelper.GetTheme();

            modificationAction?.Invoke(theme);

            paletteHelper.SetTheme(theme);
        }

        #endregion 更改主题颜色

        private readonly PaletteHelper _paletteHelper = new PaletteHelper();
        public IEnumerable<ISwatch> Swatches { get; } = SwatchHelper.Swatches;
        public ICommand ChangeHueCommand { get; }

        public PersonalUCViewModel(PaletteHelper paletteHelper)
        {
            ChangeHueCommand = new DelegateCommand<object>(ChangeHue);
        }

        private void ChangeHue(object? obj)
        {
            Theme theme = _paletteHelper.GetTheme();
            var color = (Color)obj;
            theme.PrimaryLight = new ColorPair(color.Lighten());
            theme.PrimaryMid = new ColorPair(color);
            theme.PrimaryDark = new ColorPair(color.Darken());

            _paletteHelper.SetTheme(theme);
        }
    }
}