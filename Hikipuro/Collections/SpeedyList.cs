using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Hikipuro.Collections {
	/// <summary>
	/// インデックスを使用してアクセスできる、厳密に型指定されたオブジェクトのリストを表します。リストの検索、並べ替え、および操作のためのメソッドを提供します。
	/// Generic List の参照スピードを改善したクラス.
	/// - List<> の代わりとして使用する.
	/// - IndexOf, Contains の速度が上がる.
	/// - メモリのオーバーヘッドは増える.
	/// - 要素の追加, 削除に処理コストがかかる.
	/// </summary>
	/// <typeparam name="T">リスト内の要素の型。</typeparam>
	[DebuggerDisplay("Count = {Count}")]
	public class SpeedyList<T> : IList<T>, ICollection<T>, IEnumerable<T>, IList, ICollection, IEnumerable {
		/// <summary>
		/// リスト.
		/// </summary>
		List<T> list;

		/// <summary>
		/// リストのインデックス.
		/// </summary>
		SpeedyListIndex<T> listIndex;

		/// <summary>
		/// ICollection インターフェイスで使用する.
		/// </summary>
		object syncRoot;

		/// <summary>
		/// 空で、既定の初期量を備えた、<see cref="T:Hikipuro.Collections.SpeedyList`1"/> クラスの新しいインスタンスを初期化します。
		/// </summary>
		public SpeedyList() {
			list = new List<T>();
			listIndex = new SpeedyListIndex<T>(list);
		}

		/// <summary>
		/// 空で、指定した初期量を備えた、<see cref="T:Hikipuro.Collections.SpeedyList`1"/> クラスの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="capacity">新しいリストに格納できる要素の数。</param>
		/// <exception cref="ArgumentOutOfRangeException">capacity が 0 未満です。</exception>
		public SpeedyList(int capacity) {
			list = new List<T>(capacity);
			listIndex = new SpeedyListIndex<T>(list);
		}

		/// <summary>
		/// 指定したコレクションからコピーした要素を格納し、コピーされる要素の数を格納できるだけの容量を備えた、
		/// <see cref="T:Hikipuro.Collections.SpeedyList`1"/> クラスの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="collection">新しいリストに要素がコピーされたコレクション。</param>
		/// <exception cref="ArgumentNullException">collection が null です。</exception>
		public SpeedyList(IEnumerable<T> collection) {
			list = new List<T>(collection);
			listIndex = new SpeedyListIndex<T>(list);
			listIndex.Refresh();
		}

		/// <summary>
		/// デストラクタ.
		/// </summary>
		~SpeedyList() {
			Clear();
		}

		/// <summary>
		/// 指定したインデックスにある要素を取得または設定します。
		/// </summary>
		/// <param name="index">取得または設定する要素の、0 から始まるインデックス番号。</param>
		/// <returns>指定したインデックスにある要素。</returns>
		/// <exception cref="ArgumentOutOfRangeException">index が 0 未満です。 または index が <see cref="T:Hikipuro.Collections.SpeedyList`1.Count"/> 以上です。</exception>
		public T this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}

		/// <summary>
		/// 指定したインデックスにある要素を取得または設定します。
		/// </summary>
		/// <param name="index">取得または設定する要素の、0 から始まるインデックス番号。</param>
		/// <returns>指定したインデックスにある要素。</returns>
		/// <exception cref="ArgumentOutOfRangeException">index が 0 未満です。 または index が <see cref="T:Hikipuro.Collections.SpeedyList`1.Count"/> 以上です。</exception>
		object IList.this[int index] {
			get { return list[index]; }
			set { list[index] = (T)value; }
		}

		/// <summary>
		/// <see cref="T:Hikipuro.Collections.SpeedyList`1"/> に実際に格納されている要素の数。
		/// </summary>
		public int Count {
			get { return list.Count; }
		}

		/// <summary>
		/// IList インターフェイスで使用する.
		/// </summary>
		public bool IsFixedSize {
			get { return false; }
		}

		/// <summary>
		/// IList インターフェイスで使用する.
		/// </summary>
		public bool IsReadOnly {
			get { return false; }
		}

		/// <summary>
		/// ICollection インターフェイスで使用する.
		/// </summary>
		public bool IsSynchronized {
			get { return false; }
		}

		/// <summary>
		/// ICollection インターフェイスで使用する.
		/// </summary>
		public object SyncRoot {
			get {
				if (syncRoot == null) {
					Interlocked.CompareExchange<object>(
						ref syncRoot, new object(), null
					);
				}
				return syncRoot;
			}
		}

		/// <summary>
		/// 要素を追加する (IList インターフェイス).
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public int Add(object value) {
			Add((T)value);
			return list.Count - 1;
		}

		/// <summary>
		/// <see cref="T:Hikipuro.Collections.SpeedyList`1"/> の末尾にオブジェクトを追加します。
		/// </summary>
		/// <param name="item"><see cref="T:Hikipuro.Collections.SpeedyList`1"/> の末尾に追加するオブジェクト。参照型の場合、null の値を使用できます。</param>
		public void Add(T item) {
			list.Add(item);
			listIndex.Add(item, list.Count - 1);
		}

		/// <summary>
		/// <see cref="T:Hikipuro.Collections.SpeedyList`1"/> からすべての要素を削除します。
		/// </summary>
		public void Clear() {
			list.Clear();
			listIndex.Clear();
		}

		/// <summary>
		/// ある要素が <see cref="T:Hikipuro.Collections.SpeedyList`1"/> 内に存在するかどうかを判断します。
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool Contains(object value) {
			return listIndex.Contains((T)value);
		}

		/// <summary>
		/// ある要素が <see cref="T:Hikipuro.Collections.SpeedyList`1"/> 内に存在するかどうかを判断します。
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Contains(T item) {
			return listIndex.Contains(item);
		}

		/// <summary>
		/// <see cref="T:Hikipuro.Collections.SpeedyList`1"/> 
		/// 全体を互換性のある 1 次元の配列にコピーします。コピー操作は、コピー先の配列の指定したインデックスから始まります。
		/// </summary>
		/// <param name="array"><see cref="T:Hikipuro.Collections.SpeedyList`1"/> から要素がコピーされる 1 次元の System.Array。System.Array には、0 から始まるインデックス番号が必要です。</param>
		/// <param name="index">コピーの開始位置となる、array の 0 から始まるインデックス番号。</param>
		/// <exception cref="ArgumentNullException">array が null です。</exception>
		/// <exception cref="ArgumentOutOfRangeException">arrayIndex が 0 未満です。</exception>
		/// <exception cref="ArgumentException">arrayIndex が array の長さ以上です。 または コピー元の <see cref="T:Hikipuro.Collections.SpeedyList`1"/> の要素数が、arrayIndex からコピー先の array の末尾までに格納できる数を超えています。</exception>
		public void CopyTo(Array array, int index) {
			CopyTo((T[])array, index);
		}

		/// <summary>
		/// <see cref="T:Hikipuro.Collections.SpeedyList`1"/> 
		/// 全体を互換性のある 1 次元の配列にコピーします。コピー操作は、コピー先の配列の指定したインデックスから始まります。
		/// </summary>
		/// <param name="array"><see cref="T:Hikipuro.Collections.SpeedyList`1"/> から要素がコピーされる 1 次元の System.Array。System.Array には、0 から始まるインデックス番号が必要です。</param>
		/// <param name="arrayIndex">コピーの開始位置となる、array の 0 から始まるインデックス番号。</param>
		/// <exception cref="ArgumentNullException">array が null です。</exception>
		/// <exception cref="ArgumentOutOfRangeException">arrayIndex が 0 未満です。</exception>
		/// <exception cref="ArgumentException">arrayIndex が array の長さ以上です。 または コピー元の <see cref="T:Hikipuro.Collections.SpeedyList`1"/> の要素数が、arrayIndex からコピー先の array の末尾までに格納できる数を超えています。</exception>
		public void CopyTo(T[] array, int arrayIndex) {
			list.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// <see cref="T:Hikipuro.Collections.SpeedyList`1"/> を反復処理する列挙子を返します。
		/// </summary>
		/// <returns><see cref="T:Hikipuro.Collections.SpeedyList`1"/> の <see cref="T:System.Collections.Generic.List`1.Enumerator"/> 。</returns>
		public IEnumerator<T> GetEnumerator() {
			return list.GetEnumerator();
		}

		/// <summary>
		/// 指定したオブジェクトを検索し、<see cref="T:Hikipuro.Collections.SpeedyList`1"/> 全体内で最初に見つかった位置の 0 から始まるインデックスを返します。
		/// </summary>
		/// <param name="value"><see cref="T:Hikipuro.Collections.SpeedyList`1"/> 内で検索するオブジェクト。参照型の場合、null の値を使用できます。</param>
		/// <returns><see cref="T:Hikipuro.Collections.SpeedyList`1"/> 全体内で item が見つかった場合は、最初に見つかった位置の 0 から始まるインデックス。それ以外の場合は -1 。</returns>
		public int IndexOf(object value) {
			return IndexOf((T)value);
		}

		/// <summary>
		/// 指定したオブジェクトを検索し、<see cref="T:Hikipuro.Collections.SpeedyList`1"/> 全体内で最初に見つかった位置の 0 から始まるインデックスを返します。
		/// </summary>
		/// <param name="item"><see cref="T:Hikipuro.Collections.SpeedyList`1"/> 内で検索するオブジェクト。参照型の場合、null の値を使用できます。</param>
		/// <returns><see cref="T:Hikipuro.Collections.SpeedyList`1"/> 全体内で item が見つかった場合は、最初に見つかった位置の 0 から始まるインデックス。それ以外の場合は -1 。</returns>
		public int IndexOf(T item) {
			int index = listIndex.IndexOf(item);
			/*
			int iDebug = list.IndexOf(item);
			if (index != iDebug) {
				Console.WriteLine("!: " + index + ", " + iDebug);
			}
			//*/
			return index;
		}

		/// <summary>
		/// <see cref="T:Hikipuro.Collections.SpeedyList`1"/> 内の指定したインデックスの位置に要素を挿入します。
		/// </summary>
		/// <param name="index">value を挿入する位置の、0 から始まるインデックス番号。</param>
		/// <param name="value">挿入するオブジェクト。参照型の場合、null の値を使用できます。</param>
		/// <exception cref="ArgumentOutOfRangeException">index が 0 未満です。 または index が <see cref="T:Hikipuro.Collections.SpeedyList`1.Count"/> より大きい。</exception>
		public void Insert(int index, object value) {
			Insert(index, (T)value);
		}

		/// <summary>
		/// <see cref="T:Hikipuro.Collections.SpeedyList`1"/> 内の指定したインデックスの位置に要素を挿入します。
		/// </summary>
		/// <param name="index">item を挿入する位置の、0 から始まるインデックス番号。</param>
		/// <param name="item">挿入するオブジェクト。参照型の場合、null の値を使用できます。</param>
		/// <exception cref="ArgumentOutOfRangeException">index が 0 未満です。 または index が <see cref="T:Hikipuro.Collections.SpeedyList`1.Count"/> より大きい。</exception>
		public void Insert(int index, T item) {
			list.Insert(index, item);
			if (listIndex.HistoryCount > listIndex.HistoryThreshold) {
				listIndex.Refresh();
			} else {
				listIndex.Insert(index, item);
			}
		}

		/// <summary>
		/// <see cref="T:Hikipuro.Collections.SpeedyList`1"/> 内で最初に見つかった特定のオブジェクトを削除します。
		/// </summary>
		/// <param name="value"><see cref="T:Hikipuro.Collections.SpeedyList`1"/> から削除するオブジェクト。参照型の場合、null の値を使用できます。</param>
		public void Remove(object value) {
			Remove((T)value);
		}

		/// <summary>
		/// <see cref="T:Hikipuro.Collections.SpeedyList`1"/> 内で最初に見つかった特定のオブジェクトを削除します。
		/// </summary>
		/// <param name="item"><see cref="T:Hikipuro.Collections.SpeedyList`1"/> から削除するオブジェクト。参照型の場合、null の値を使用できます。</param>
		/// <returns>item が正常に削除された場合は true。それ以外の場合は false。このメソッドは、item が <see cref="T:Hikipuro.Collections.SpeedyList`1"/> に見つからなかった場合にも false を返します。</returns>
		public bool Remove(T item) {
			if (listIndex.Contains(item) == false) {
				return false;
			}
			int index = listIndex.Remove(item);

			/*
			int iDebug = list.IndexOf(item);
			if (index != iDebug) {
				Console.WriteLine("!: " + index + ", " + iDebug);
			}
			//*/

			//if (index < 0) {
			//	return false;
			//}

			list.RemoveAt(index);
			if (listIndex.HistoryCount > listIndex.HistoryThreshold) {
				listIndex.Refresh();
			}
			return true;
		}

		/// <summary>
		/// <see cref="T:Hikipuro.Collections.SpeedyList`1"/> の指定したインデックスにある要素を削除します。
		/// </summary>
		/// <param name="index">削除する要素の、0 から始まるインデックス番号。</param>
		/// <exception cref="ArgumentOutOfRangeException">index が 0 未満です。 または index が <see cref="T:Hikipuro.Collections.SpeedyList`1.Count"/> 以上です。</exception>
		public void RemoveAt(int index) {
			T item = list[index];
			list.RemoveAt(index);
			if (listIndex.HistoryCount > listIndex.HistoryThreshold) {
				listIndex.Refresh();
			} else {
				listIndex.RemoveAt(item, index);
			}
		}

		/// <summary>
		/// IEnumerator インターフェイスを取得する.
		/// </summary>
		/// <returns></returns>
		IEnumerator IEnumerable.GetEnumerator() {
			return list.GetEnumerator();
		}
	}
}
