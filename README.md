# CSharp-Collection-Sampling
C#對集合類型以權重進行抽樣。 以IEnumerable的泛型擴充方法撰寫。

C# Collection Sampling with weight. Base on IEnumerable generic extension method.

## Language and Framework
C# .NET 4.5 above

## How to Use
選擇其中一種：Choose one:

1.下載CollectionSampling.cs加入專案中。Download CollectionSampling.cs file and add it into project.

2.下載CollectionSampling.dll並使專案參考它。Download CollectionSampling.dll and add referance to project.

3.使用以下NuGet指令：Run the following command in the Package Manager Console:

```
PM> Install-Package CollectionSampling
```

4.從NuGet管理員中搜尋CollectionSampling。Search CollectionSampling in the NuGet Manager.

使用命名空間(namespace):
```C#
using System.Collections.Generic.CollectionSampling;
```

### Collection
其中有兩個多載方法 There are two overloading method：
```C#
int SamplingIndex<TSource>()
TSource Sampling<TSource>()
```
第一個回傳抽樣結果的索引值，第二個則回傳抽樣結果該物件。

The first one return index of the sampling result, second one return sampling result object.

1.一般數值類型(Normal Numeric Type)

直接使用方法取得抽樣索引值或抽樣結果之物件。

Use method directly to get index of sampling result or sampling result object.
```C#
List<int> list = new List<int>() { 2, 4, 6, 8, 10 };
Console.WriteLine(list.SamplingIndex<int>());
```
輸出結果為0~4其中之一，其中抽中10的機率為10/(2+4+6+8+10)=1/3，以此類推。

The output will between 0 and 4, the sampling probability of 10 is 10/(2+4+6+8+10)=1/3, and so on.
```C#
List<int> list = new List<int>() { 2, 4, 6, 8, 10 };
Console.WriteLine(list.Sampling<int>());
```
輸出結果則為2,4,6,8,10其中之一，權重計算同上。

The output will be one of {2,4,6,8,10}, and the weight calculation is the same as above.

泛型類別中數值型別都可以使用，包含整數、實數、字元或是內容為數值的字串，如實數：

All numerical type of generic can be used, include integer, real number, character, or string that is numerical.For example of the real number:
```C#
List<double> list = new List<double>() { 0.2, 0.5, 0.1, 1.3 };
Console.WriteLine(list.SamplingIndex<double>());
```
字串String：
```C#
List<string> list = new List<string>() { "20", "30", "70" };
Console.WriteLine(list.Sampling<string>());
```
權重計算同上。

The weight calculation is the same as above.

字元Character：
```C#
List<char> list = new List<char>() { 'A', 'B', 'C' };
Console.WriteLine(list.Sampling<char>());
```
該範例會輸出A,B,C其中之一，權重則是以該字元的Unicode作為數值。

This example will output one of A,B,C, and the weight is base on that character's unicode.

集合類型中除了Dictionary之外都可使用，且用法相同，如：

All collection type can be used except Dictionary, and the same way to use. For example:
```C#
int[] arr = new int[4] { 4, 3, 7, 2 };
Console.WriteLine(arr.Sampling<int>());
```

也可使用自訂隨機種子：

It can also use custom Random Seed:
```C#
List<int> list = new List<int>() { 2, 4, 6, 8, 10 };
Random rnd = new Random();
Console.WriteLine(list.Sampling<int>(rnd));
```
則該方法會使用 rnd 作為隨機的物件。

Therefore, the method will use rnd as Random object.

2.物件(Object)

若泛型型別為物件，則必須使用多載參數weightPropertyName指定用於計算權重之屬性名稱。如測試類別：

If the generic type is object, there must be parameter weightPropertyName to assign the property name of the weight.As the test class:
```C#
public class MyClass
{
    public string str { get; set; }
    public int i { get; set; }
    public double d { get; set; }
}
```
在使用時以字串指定屬性：

Use string to assign property when using:
```C#
List<MyClass> list = new List<MyClass>(){
    new MyClass(){i=5},
    new MyClass(){i=3},
    new MyClass(){i=6}
};
Console.WriteLine(list.SamplingIndex<MyClass>("i"));
```
同1，也可使用其他數值型別：

As 1, there can be other numerical type:
```C#
List<MyClass> list = new List<MyClass>(){
    new MyClass(){d=5.2},
    new MyClass(){d=4.7},
    new MyClass(){d=6.5}
};
Console.WriteLine(list.SamplingIndex<MyClass>("d"));
```
注意若以物件使用Sampling<T>方法，該方法會回傳該抽樣物件，如：

Notice that if using method Sampling<T> with object, that method will return that sampling result object, for example:
```C#
List<MyClass> list = new List<MyClass>(){
    new MyClass(){i=5,str="AA"},
    new MyClass(){i=3,str="BB"},
    new MyClass(){i=6,str="CC"}
};
Console.WriteLine(list.Sampling<MyClass>("i").str);
```
則輸出為AA,BB,CC其中之一，而權重則為5,3,6。

So the output will be one of AA, BB, or CC, and the weight is 5,3,6.

另外物件一樣可以使用Dictionary之外其他集合類型與自訂隨機物件。

Also, the object can use other collection type except Dictionary, and custom Random object, too.

### Dictionary
Dictionary包含另兩個多載方法There are other two method in Dictionary：
```C#
TValue Sampling<TKey, TValue>()
TKey SamplingKey<TKey, TValue>()
```
第一個回傳抽樣結果的值，第二個則回傳抽樣結果的索引。

The first one return value of the sampling result, second one return key of the sampling result.

使用方式大部分與一般集合相同，但最後須指定額外布林參數，決定是否以索引作為抽樣的權重。

Most part are same as normal collection, but you have to assign another boolean property, to determine whether use the key of the dictionary as weight or not.

E.g.
```C#
Dictionary<int, string> dic = new Dictionary<int, string>(){
    {4, "Eric"},
    {5, "Allen"},
    {2, "Apple"}
};
Console.WriteLine(dic.Sampling<int, string>(true));
```
結果會是Eric, Allen, Apple其中之一。

The result will be one of Eric, Allen, Apple.

## Exception
當傳入非數值型別，或傳入類別的權重屬性未指定或非數值，則會產生例外：

When giving the not numerical type, or not assign or not a number of property of giving class, there will be exception:

`System.NotFiniteNumberException: Collection Element is not a Number.`

***

## Information
### Latest Update
2015/09/10

### Version
v1.1.0

### System Requirements
.NET Framework 4.5+

### Version History
#### v1.1.0
-加入Dictionary的部分。

-修正部分說明文字。

#### v1.0.1
-將.NET Framework版本修改為4.5以對應較舊版本的專案。
