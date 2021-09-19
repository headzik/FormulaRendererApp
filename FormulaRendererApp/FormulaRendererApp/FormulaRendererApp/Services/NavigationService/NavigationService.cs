using FormulaRendererApp.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace FormulaRendererApp.Services
{
    public class NavigationService : INavigationService
    {
        // Dictionary with registered pages in the app:
        private readonly Dictionary<AppPages, Type> pagesByKey = new Dictionary<AppPages, Type>();
        
        // Navigation page where MainPage is hosted:
        private NavigationPage navigation;
        
        // Get currently displayed page:
        public string CurrentPageKey
        {
            get
            {
                lock (pagesByKey)
                {
                    if (navigation.CurrentPage == null)
                    {
                        return null;
                    }

                    var pageType = navigation.CurrentPage.GetType();

                    return pagesByKey.ContainsValue(pageType)
                                      ? pagesByKey.First(p => p.Value == pageType).Key.ToString() : null;
                }
            }
        }
        // GoBack implementation (just pop page from the navigation stack):
     
        public void GoBack()
        {
            navigation.PopAsync();
        }

        // NavigateTo method to navigate between pages without passing parameter:
        public void NavigateTo(AppPages pageKey)
        {
            NavigateTo(pageKey, null);
        }

        // NavigateTo method to navigate between pages with passing parameter:
        public void NavigateTo(AppPages pageKey, object parameter)
        {
            lock (pagesByKey)
            {

                if (pagesByKey.ContainsKey(pageKey))
                {
                    var type = pagesByKey[pageKey];
                    ConstructorInfo constructor;
                    object[] parameters;

                    if (parameter == null)
                    {
                        constructor = type.GetTypeInfo()
                            .DeclaredConstructors
                            .FirstOrDefault(c => !c.GetParameters().Any());

                        parameters = new object[]
                        {
                        };
                    }
                    else
                    {
                        constructor = type.GetTypeInfo()
                            .DeclaredConstructors
                            .FirstOrDefault(
                                c =>
                                {
                                    var p = c.GetParameters();
                                    return p.Count() == 1
                                           && p[0].ParameterType == parameter.GetType();
                                });

                        parameters = new[]
                        {
                            parameter
                        };
                    }

                    if (constructor == null)
                    {
                        throw new InvalidOperationException(
                            "No suitable constructor found for page " + pageKey);
                    }

                    var page = constructor.Invoke(parameters) as Page;
                    navigation.PushAsync(page);
                }
                else
                {
                    throw new ArgumentException(
                        string.Format(
                            "No such page: {0}. Did you forget to call NavigationService.Configure?",
                            pageKey), nameof(pageKey));
                }
            }
        }

        // Register pages and add them to the dictionary:
        public void Configure(AppPages pageKey, Type pageType)
        {
            lock (pagesByKey)
            {
                if (pagesByKey.ContainsKey(pageKey))
                {
                    pagesByKey[pageKey] = pageType;
                }
                else
                {
                    pagesByKey.Add(pageKey, pageType);
                }
            }
        }

        // Initialize first app page (navigation page):
        public void Initialize(NavigationPage navigation)
        {
            this.navigation = navigation;
        }

    }
}
