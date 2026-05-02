using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace XLibur.Excel.InsertData;

internal sealed class SimpleTypeReader : IInsertDataReader
{
    private readonly IEnumerable<object> _data;
    private readonly Type _itemType;

    public SimpleTypeReader(IEnumerable data, Type itemType)
    {
        ArgumentNullException.ThrowIfNull(data);
        _itemType = itemType;
        _data = data.Cast<object>();
    }

    public IEnumerable<IEnumerable<XLCellValue>> GetRecords()
    {
        return _data.Select(item => new[] { item }.Select(XLCellValue.FromInsertedObject));
    }

    public int GetPropertiesCount()
    {
        return 1;
    }

    public string GetPropertyName(int propertyIndex)
    {
        if (propertyIndex != 0)
            throw new ArgumentException("SimpleTypeReader supports only a single property");

        return _itemType.Name;
    }
}
