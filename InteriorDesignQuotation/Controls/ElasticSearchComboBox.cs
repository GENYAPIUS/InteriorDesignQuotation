using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BindingLibrary;
using InteriorDesignQuotation.Extensions;

namespace InteriorDesignQuotation.Controls;

public class ElasticSearchComboBox : ComboBox
{
    internal static readonly DependencyProperty SearchContentProperty;

    private ICommand? _searchTextBoxDownKeyCommand;
    private ObservableCollection<string> _sourceCollection = new();
    private ObservableCollection<string> _viewCollection = new();

    static ElasticSearchComboBox()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ElasticSearchComboBox),
            new FrameworkPropertyMetadata(typeof(ElasticSearchComboBox)));
        SearchContentProperty =
            DependencyProperty.Register(nameof(SearchContent), typeof(string), typeof(ElasticSearchComboBox));
    }

    public ICommand SearchTextBoxDownKeyCommand =>
        _searchTextBoxDownKeyCommand ??= new RelayCommand(_ =>
        {
            if (GetTemplateChild("SearchTextBox") is not TextBox searchTextBox) return;
            searchTextBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
        });

    internal string SearchContent
    {
        get => GetValue(SearchContentProperty) as string ?? string.Empty;
        set => SetValue(SearchContentProperty, value);
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        DropDownClosed += (_, _) => SearchContent = string.Empty;
        SetBindingCollection();
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (GetTemplateChild("SearchTextBox") is not TextBox searchTextBox) return;
        if (e.Key == Key.Enter && searchTextBox.IsFocused && !string.IsNullOrWhiteSpace(SearchContent))
            OnSearchTextBoxPressEnter(e);
        else base.OnKeyDown(e);
    }

    private void SetBindingCollection()
    {
        var sourceCollection = GetSourceCollection();
        if (sourceCollection == null) return;
        _viewCollection = sourceCollection;
        _sourceCollection = sourceCollection.ToObservableCollection();
        SetSearchTextBoxOnInput();
    }

    private void SetSearchTextBoxOnInput()
    {
        if (GetTemplateChild("SearchTextBox") is not TextBox searchTextBox) return;
        searchTextBox.TextChanged += (_, e) => OnSearchTextBoxInput();
    }

    private void OnSearchTextBoxInput()
    {
        if (string.IsNullOrWhiteSpace(SearchContent))
        {
            var selectedIndex = SelectedIndex;
            _viewCollection.Clear();
            foreach (var item in _sourceCollection) _viewCollection.Add(item);

            SelectedIndex = selectedIndex;
        }
        else
        {
            _viewCollection.Clear();
            foreach (var item in _sourceCollection.Where(x => x.Contains(SearchContent))) _viewCollection.Add(item);
        }
    }

    private void OnSearchTextBoxPressEnter(KeyEventArgs e)
    {
        if (_sourceCollection.All(x => x != SearchContent))
        {
            _sourceCollection.Insert(0, SearchContent);
            _viewCollection.Clear();
            foreach (var item in _sourceCollection) _viewCollection.Add(item);
        }

        SelectedItem = SearchContent;
    }

    private ObservableCollection<string>? GetSourceCollection()
    {
        var binding = GetBindingExpression(ItemsSourceProperty);
        if (binding is null) return null;
        var sourceType = binding.ResolvedSource.GetType();
        var targetProperty = sourceType.GetProperty(binding.ResolvedSourcePropertyName);
        return targetProperty?.GetValue(binding.ResolvedSource) as ObservableCollection<string>;
    }
}