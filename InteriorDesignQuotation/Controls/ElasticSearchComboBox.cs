using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BindingLibrary;
using InteriorDesignQuotation.Extensions;
using InteriorDesignQuotation.ViewModels;

namespace InteriorDesignQuotation.Controls
{
    /// <summary>
    /// 依照步驟 1a 或 1b 執行，然後執行步驟 2，以便在 XAML 檔中使用此自訂控制項。
    ///
    /// 步驟 1a) 於存在目前專案的 XAML 檔中使用此自訂控制項。
    /// 加入此 XmlNamespace 屬性至標記檔案的根項目為 
    ///要使用的: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:InteriorDesignQuotation.Controls"
    ///
    ///
    /// 步驟 1b) 於存在其他專案的 XAML 檔中使用此自訂控制項。
    /// 加入此 XmlNamespace 屬性至標記檔案的根項目為 
    ///要使用的: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:InteriorDesignQuotation.Controls;assembly=InteriorDesignQuotation.Controls"
    ///
    /// 您還必須將 XAML 檔所在專案的專案參考加入
    /// 此專案並重建，以免發生編譯錯誤: 
    ///
    ///     在 [方案總管] 中以滑鼠右鍵按一下目標專案，並按一下
    ///     [加入參考]->[專案]->[瀏覽並選取此專案]
    ///
    ///
    /// 步驟 2)
    /// 開始使用 XAML 檔案中的控制項。
    ///
    ///     <MyNamespace:ElasticSearchComboBox/>
    ///
    /// </summary>
    public class ElasticSearchComboBox : ComboBox
    {
        static ElasticSearchComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ElasticSearchComboBox),
                new FrameworkPropertyMetadata(typeof(ElasticSearchComboBox)));
            SearchContentProperty =
                DependencyProperty.Register(nameof(SearchContent), typeof(string), typeof(ElasticSearchComboBox));
            // AddNewItemCommandProperty = DependencyProperty.Register(nameof(AddNewItemCommand), typeof(ICommand),
            //     typeof(ElasticSearchComboBox));
        }

        private ObservableCollection<string> _viewCollection = new();
        private ObservableCollection<string> _sourceCollection = new();
        private ObservableCollection<string> _searchResultCollection = new();

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            DropDownClosed += (_, _) => SearchContent = string.Empty;
            var targetCollection = GetSourceCollection();
            if (targetCollection == null) return;
            _viewCollection = GetSourceCollection() ?? new ObservableCollection<string>();
            SetSearchTextBoxOnInput();
        }

        private void SetSearchTextBoxOnInput()
        {
            if(GetTemplateChild("SearchTextBox") is not TextBox searchTextBox) return;
            searchTextBox.TextInput += (_, e) =>
            {
                OnSearchTextBoxInput();
            };
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (GetTemplateChild("SearchTextBox") is not TextBox searchTextBox) return;
            if (e.Key == Key.Enter && searchTextBox.IsFocused && !string.IsNullOrWhiteSpace(SearchContent))
                OnSearchTextBoxPressEnter(e);
            else base.OnKeyDown(e);
        }

        private void OnSearchTextBoxInput()
        {
            if (string.IsNullOrWhiteSpace(SearchContent))
            {
                _viewCollection = _sourceCollection;
            }
            else
            {
                _sourceCollection = _viewCollection.ToObservableCollection();
                _viewCollection = _viewCollection.Where(x => x.Contains(SearchContent)).ToObservableCollection();
            }
        }

        private void OnSearchTextBoxPressEnter(KeyEventArgs e)
        {
            if (_viewCollection.All(x => x != SearchContent))
            {
                _viewCollection.Insert(0, SearchContent);
            }
            SelectedItem = SearchContent;
        }

        private ObservableCollection<string>? GetSourceCollection()
        {
            var binding = GetBindingExpression(ItemsSourceProperty);
            if (binding is null) return null;
            var mainViewModel = typeof(MainViewModel);
            var targetProperty = mainViewModel.GetProperty(binding.ResolvedSourcePropertyName);
            return targetProperty?.GetValue(binding.ResolvedSource) as ObservableCollection<string>;
        }

        public static readonly DependencyProperty SearchContentProperty;

        public string SearchContent
        {
            get => GetValue(SearchContentProperty) as string ?? string.Empty;
            set => SetValue(SearchContentProperty, value);
        }

        // public static readonly DependencyProperty AddNewItemCommandProperty;
        //
        // public ICommand? AddNewItemCommand
        // {
        //     get => GetValue(AddNewItemCommandProperty) as ICommand;
        //     set => SetValue(AddNewItemCommandProperty, value);
        // }

        private ICommand? _searchTextBoxDownKeyCommand;

        public ICommand SearchTextBoxDownKeyCommand =>
            _searchTextBoxDownKeyCommand ??= new RelayCommand((_) =>
            {
                if (GetTemplateChild("SearchTextBox") is not TextBox searchTextBox) return;
                searchTextBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
            });
    }
}