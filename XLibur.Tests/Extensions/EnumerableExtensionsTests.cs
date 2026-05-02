using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using XLibur.Extensions;
using XLibur.Tests.Excel.Tables;

namespace XLibur.Tests.Extensions;

public class EnumerableExtensionsTests
{
    private static readonly int[] SkipLastSingle = [1];
    private static readonly int[] SkipLastTwo = [1, 2];

    [Test]
    public void CanGetItemType()
    {
        var array = Array.Empty<int>();
        Assert.AreEqual(typeof(int), array.GetItemType());

        var list = new List<double>();
        Assert.AreEqual(typeof(double), list.GetItemType());
        Assert.AreEqual(typeof(double), list.AsEnumerable().GetItemType());

        IEnumerable<IEnumerable> enumerable = new List<string>();
        Assert.AreEqual(typeof(string), enumerable.GetItemType());

        enumerable = new List<List<string>>();
        Assert.AreEqual(typeof(List<string>), enumerable.GetItemType());

        enumerable = new List<int[]>();
        Assert.AreEqual(typeof(int[]), enumerable.GetItemType());

        var anonymousIterator = new List<TablesTests.TestObjectWithoutAttributes>()
            .Select(o => new { FirstName = o.Column1, LastName = o.Column2 });

        // expectedType is something like <>f__AnonymousType9`2[System.String,System.String], but
        // the `9` differs as new anonymous types are declared in the assembly — match only the ends.
        AssertAnonymousItemType(anonymousIterator);
        AssertAnonymousItemType((IEnumerable<object>)anonymousIterator);
    }

    private static void AssertAnonymousItemType(IEnumerable source)
    {
        const string expectedTypeStart = "<>f__AnonymousType";
        const string expectedTypeEnd = "`2[System.String,System.String]";
        var actualType = source.GetItemType()!.ToString();
        Assert.True(actualType.StartsWith(expectedTypeStart));
        Assert.True(actualType.EndsWith(expectedTypeEnd));
    }

    [Test]
    public void SkipLast_skips_last_element_of_enumerable()
    {
        var empty = Array.Empty<int>().SkipLast();
        Assert.That(empty, Is.Empty);

        var oneElement = SkipLastSingle.SkipLast();
        Assert.That(oneElement, Is.Empty);

        var twoElements = SkipLastTwo.SkipLast();
        Assert.That(twoElements, Is.EqualTo([1]));
    }

    [Test]
    public void WhereNotNull_removes_null_elements()
    {
        var source = new int?[] { 1, null, 2 };

        var result = source.WhereNotNull(x => x);

        Assert.That(result, Is.EqualTo([1, 2]));
    }
}
