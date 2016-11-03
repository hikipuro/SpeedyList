# SpeedyList
C# の Generic List の速度改善版

- .NET Framework 2.0 以降の環境で実行できます。
- 今のところ開発中のため、十分なテストができていません。

## ライセンス
- [MIT](LICENSE)

## 開発環境

- Visual Studio 2015

## 説明

開発の動機

- List 型は IndexOf と Contains の呼び出しが遅い (リストの項目 n 個とすると、平均 O(n/2) の処理時間になる)
- メモリのオーバーヘッドを増やしても改善したい
- 内部的に List を検索する操作をオーバーライドすることで改善可能
- Dictionary の ContainsKey を活用することで、おそらく IndexOf と Contains は O(1) に近い処理時間になる
- .NET Framework 2.0 でも動作させたい

動作内容

- 普通の List とほぼ同じ操作ができる
- 並び順はランダムで OK。値の重複も OK。
- メモリのオーバーヘッド: 32 bit 環境で 1 項目につき、おおよそ 70 バイト程度、 64 bit 環境では 1 項目につき 120 バイト程度 (List&lt;int&gt; の場合、メモリ消費量は数十倍大きくなります。)
- Add と Insert の処理時間は、普通の List の数倍かかる
- IndexOf と Contains の処理時間は、場合によっては数百分の 1 になる
- ユーザ定義クラスをアイテムとして使用する場合は、 GetHashCode() のオーバーライドが必要