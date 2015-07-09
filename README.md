# CSharp-Collection-Sampling
C#對集合類型(Dictionary除外)以權重進行抽樣。 以IEnumerable的泛型擴充方法撰寫。

C# Collection(except Dictionary) Sampling with weight. Base on IEnumerable generic extension method.

#Language and Framework
C# .NET 4.5.1

#How to use
下載CollectionSampling.cs加入專案中，或是下載CollectionSampling.dll並使專案參考它。

Download CollectionSampling.cs file and add it into project, or download CollectionSampling.dll and add referance to project.

使用命名空間(namespace):System.Collections.Generic.CollectionSampling
```C#
using System.Collections.Generic.CollectionSampling;
```

其中有兩個多載方法：
```C#
int SamplingIndex<TSource>()
TSource Sampling<TSource>()
```
第一個回傳抽樣結果的索引值，第二個則回傳抽樣結果該物件。

1.一般數值類型(Normal Numeric Type)

直接使用方法取得抽樣索引值或抽樣結果之物件。
```C#
List<int> list = new List<int>() { 2, 4, 6, 8, 10 };
Console.WriteLine(list.SamplingIndex<int>());
```
輸出結果為0~4其中之一，其中抽中10的機率為10/(2+4+6+8+10)=1/3，以此類推。
```C#
List<int> list = new List<int>() { 2, 4, 6, 8, 10 };
Console.WriteLine(list.Sampling<int>());
```
輸出結果則為2,4,6,8,10其中之一，權重計算同上。

泛型類別中數值型別都可以使用，包含整數、實數、字串或是內容為數值的字串，如實數：
```C#
List<double> list = new List<double>() { 0.2, 0.5, 0.1, 1.3 };
Console.WriteLine(list.SamplingIndex<double>());
```
字串：
```C#
List<string> list = new List<string>() { "20", "30", "70" };
Console.WriteLine(list.Sampling<string>());
```
權重計算同上。

字元：
```C#
List<char> list = new List<char>() { 'A', 'B', 'C' };
Console.WriteLine(list.Sampling<char>());
```
該範例會輸出A,B,C其中之一，權重則是以該字元的Unicode作為數值。

集合類型中除了Dictionary之外都可使用，且用法相同，如：
```C#
int[] arr = new int[4] { 4, 3, 7, 2 };
Console.WriteLine(arr.Sampling<int>());
```

也可使用自訂隨機種子：
```C#
List<int> list = new List<int>() { 2, 4, 6, 8, 10 };
Random rnd = new Random();
Console.WriteLine(list.Sampling<int>(rnd));
```
則該方法會使用 rnd 作為隨機的物件。

2.物件(Object)

若泛型型別為物件，則必須使用多載參數weightPropertyName指定用於計算權重之屬性名稱。如測試類別：
```C#
public class MyClass
{
    public string str { get; set; }
    public int i { get; set; }
    public double d { get; set; }
}
```
在使用時以字串指定屬性：
```C#
List<MyClass> list = new List<MyClass>(){
    new MyClass(){i=5},
    new MyClass(){i=3},
    new MyClass(){i=6}
};
Console.WriteLine(list.SamplingIndex<MyClass>("i"));
```
同1，也可使用其他數值型別：
```C#
List<MyClass> list = new List<MyClass>(){
    new MyClass(){d=5.2},
    new MyClass(){d=4.7},
    new MyClass(){d=6.5}
};
Console.WriteLine(list.SamplingIndex<MyClass>("d"));
```
注意若以物件使用Sampling<T>方法，該方法會回傳該物件，如：
```C#
List<MyClass> list = new List<MyClass>(){
    new MyClass(){i=5,str="AA"},
    new MyClass(){i=3,str="BB"},
    new MyClass(){i=6,str="CC"}
};
Console.WriteLine(list.Sampling<MyClass>("i").str);
```
則輸出為AA,BB,CC其中之一，而權重則為5,3,6。

另外物件一樣可以使用Dictionary之外其他集合類型與自訂隨機物件。

#Exception
當傳入非數值型別，或傳入類別的權重屬性未指定或非數值，則會產生例外：

System.NotFiniteNumberException: Collection Element is not a Number.
