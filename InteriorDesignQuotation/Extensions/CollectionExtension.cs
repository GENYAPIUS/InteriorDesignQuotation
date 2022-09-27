using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace InteriorDesignQuotation.Extensions;

public static class CollectionExtension
{
    public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> item) => new(item);
}