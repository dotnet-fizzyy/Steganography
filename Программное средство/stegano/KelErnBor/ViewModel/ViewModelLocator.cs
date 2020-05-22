/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Stegano"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Stegano.ViewModel.Settings;

namespace Stegano.ViewModel
{
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<ShowWindowViewModel>();
            SimpleIoc.Default.Register<HideColorViewModel>();
            SimpleIoc.Default.Register<ShowColorViewModel>();
            SimpleIoc.Default.Register<ShowAttributeViewModel>();
            SimpleIoc.Default.Register<AproshViewModel>();
            SimpleIoc.Default.Register<HideAproshViewModel>();
            SimpleIoc.Default.Register<ShowAproshViewModel>();
            SimpleIoc.Default.Register<HideFontViewModel>();
            SimpleIoc.Default.Register<ShowFontViewModel>();
            SimpleIoc.Default.Register<AppearanceViewModel>();
            SimpleIoc.Default.Register<AttributeHidingViewModel>();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
        public ShowWindowViewModel ShowWindow
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ShowWindowViewModel>();
            }
        }

        public HideColorViewModel Hide
        {
            get
            {
                return ServiceLocator.Current.GetInstance<HideColorViewModel>();
            }
        }

        public ShowColorViewModel Show
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ShowColorViewModel>();
            }
        }

        public ShowAttributeViewModel ShowAttribute
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ShowAttributeViewModel>();
            }
        }

        public AproshViewModel Aprosh
        public HideFontViewModel HideFont
        {
            get
            {
                return ServiceLocator.Current.GetInstance<HideFontViewModel>();
            }
        }

        public ShowFontViewModel ShowFont
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ShowFontViewModel>();
            }
        }

        public HideAproshViewModel Aprosh
        {
            get
            {
                return ServiceLocator.Current.GetInstance<HideAproshViewModel>();
            }
        }

        public ShowAproshViewModel ShowAprosh
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ShowAproshViewModel>();
            }
        }

        public AppearanceViewModel Appearance
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AppearanceViewModel>();
            }
        }

        public AttributeHidingViewModel AttributeHiding
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AttributeHidingViewModel>();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}