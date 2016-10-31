using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Hikipuro.Collections {
	/// <summary>
	/// Generic List の参照スピードを改善したクラス.
	/// - List<> の代わりとして使用する.
	/// - IndexOf, Contains の速度が上がる.
	/// - メモリのオーバーヘッドは増える.
	/// - 要素の追加, 削除に処理コストがかかる.
	/// </summary>
	/// <typeparam name="T"></typeparam>
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
		Object syncRoot;

		public int GetHistoryCount() {
			return listIndex.history.Count;
		}

		/// <summary>
		/// コンストラクタ.
		/// </summary>
		public SpeedyList() {
			list = new List<T>();
			listIndex = new SpeedyListIndex<T>(list);
		}

		/// <summary>
		/// コンストラクタ.
		/// </summary>
		/// <param name="capacity">初期容量.</param>
		public SpeedyList(int capacity) {
			list = new List<T>(capacity);
			listIndex = new SpeedyListIndex<T>(list);
		}

		/// <summary>
		/// コンストラクタ.
		/// </summary>
		/// <param name="collection">初期リスト.</param>
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
		/// インデクサ.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public T this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}

		/// <summary>
		/// インデクサ (IList インターフェイス).
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		object IList.this[int index] {
			get { return list[index]; }
			set { list[index] = (T)value; }
		}

		/// <summary>
		/// 要素の数.
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
					Interlocked.CompareExchange<Object>(
						ref syncRoot, new Object(), null
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
			list.Add((T)value);
			listIndex.Add((T)value, list.Count - 1);
			return list.Count - 1;
		}

		/// <summary>
		/// 要素を追加する.
		/// </summary>
		/// <param name="item"></param>
		public void Add(T item) {
			list.Add(item);
			listIndex.Add(item, Count - 1);
		}

		/// <summary>
		/// 全ての要素を取り除く.
		/// </summary>
		public void Clear() {
			list.Clear();
			listIndex.Clear();
		}

		/// <summary>
		/// 要素が含まれているか確認する.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool Contains(object value) {
			return listIndex.Contains((T)value);
		}

		/// <summary>
		/// 要素が含まれているか確認する.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Contains(T item) {
			return listIndex.Contains(item);
		}

		/// <summary>
		/// 配列にコピーする.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		public void CopyTo(Array array, int index) {
			CopyTo((T[])array, index);
		}

		/// <summary>
		/// 配列にコピーする.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="arrayIndex"></param>
		public void CopyTo(T[] array, int arrayIndex) {
			list.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// IEnumerator インターフェイスを取得する.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<T> GetEnumerator() {
			return list.GetEnumerator();
		}

		/// <summary>
		/// 引数で指定された要素のインデックス番号を取得する.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public int IndexOf(object value) {
			return IndexOf((T)value);
		}

		/// <summary>
		/// 引数で指定された要素のインデックス番号を取得する.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int IndexOf(T item) {
			return listIndex.IndexOf(item);
		}

		/// <summary>
		/// 引数で指定された場所に要素を追加する.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		public void Insert(int index, object value) {
			Insert(index, (T)value);
		}

		/// <summary>
		/// 引数で指定された場所に要素を追加する.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="item"></param>
		public void Insert(int index, T item) {
			list.Insert(index, item);
			listIndex.Insert(index, item);
			if (listIndex.history.Count > listIndex.HistoryThreshold) {
				listIndex.Refresh();
			}
		}

		/// <summary>
		/// 要素を取り除く.
		/// </summary>
		/// <param name="value"></param>
		public void Remove(object value) {
			Remove((T)value);
		}

		/// <summary>
		/// 要素を取り除く.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
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
			if (listIndex.history.Count > listIndex.HistoryThreshold) {
				listIndex.Refresh();
			}
			return true;
		}

		/// <summary>
		/// 引数で指定された場所から要素を取り除く.
		/// </summary>
		/// <param name="index"></param>
		public void RemoveAt(int index) {
			Console.WriteLine("!: RemoveAt");
			index = listIndex.Remove(list[index]);
			list.RemoveAt(index);
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
