using System;
using System.Collections.Generic;

namespace Hikipuro.Collections {
	/// <summary>
	/// SpeedyList で使用する, インデックス管理用クラス.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class SpeedyListIndex<T> {
		/// <summary>
		/// インデックス 1 つ分.
		/// </summary>
		class IndexItem : IComparable<IndexItem> {
			/// <summary>
			/// 要素のインデックス番号.
			/// </summary>
			public int Index = 0;

			/// <summary>
			/// 追加されたタイミング.
			/// History のインデックス番号.
			/// </summary>
			public int InsertedAt = 0;

			/// <summary>
			/// コンストラクタ.
			/// </summary>
			/// <param name="index"></param>
			/// <param name="insertedAt"></param>
			public IndexItem(int index, int insertedAt = 0) {
				Index = index;
				InsertedAt = insertedAt;
			}

			/// <summary>
			/// List.Sort() の比較用.
			/// </summary>
			/// <param name="other"></param>
			/// <returns></returns>
			int IComparable<IndexItem>.CompareTo(IndexItem other) {
				return Index - other.Index;
			}
		}

		/// <summary>
		/// リストの操作履歴 1 つ分.
		/// </summary>
		public class HistoryItem {
			/// <summary>
			/// 対象のインデックス番号.
			/// </summary>
			public int Index = 0;

			/// <summary>
			/// どのくらい要素が動くか.
			/// Insert: 1, Remove: -1.
			/// </summary>
			public int Move = 0;

			/// <summary>
			/// コンストラクタ.
			/// </summary>
			/// <param name="index"></param>
			/// <param name="move"></param>
			public HistoryItem(int index, int move) {
				Index = index;
				Move = move;
			}
		}

		/// <summary>
		/// 操作履歴の件数のしきい値.
		/// 操作履歴が一定数以上溜まったら, インデックス全体を再構築する.
		/// 扱う要素の数によって, この値は調整した方が良さそう.
		/// </summary>
		public int HistoryThreshold = 1000;
		//public int HistoryThreshold {
		//	get { return target.Count / 10; }
		//}

		/// <summary>
		/// 処理対象のリスト.
		/// </summary>
		readonly List<T> target;

		/// <summary>
		/// インデックスの管理用.
		/// </summary>
		Dictionary<T, List<IndexItem>> indices;

		/// <summary>
		/// 値が null の時専用のインデックス.
		/// </summary>
		List<IndexItem> nullIndex;

		/// <summary>
		/// 操作履歴.
		/// </summary>
		List<HistoryItem> history;

		/// <summary>
		/// 操作履歴の次の位置.
		/// </summary>
		int position;

		/// <summary>
		/// 操作履歴の個数.
		/// </summary>
		public int HistoryCount {
			get { return position; }
		}

		/// <summary>
		/// コンストラクタ.
		/// </summary>
		/// <param name="list">処理対象のリスト.</param>
		public SpeedyListIndex(List<T> list) {
			if (list == null) {
				throw new ArgumentNullException("list");
			}
			target = list;
			indices = new Dictionary<T, List<IndexItem>>();
			nullIndex = new List<IndexItem>();
			history = new List<HistoryItem>();
			position = 0;
		}

		/// <summary>
		/// デストラクタ.
		/// </summary>
		~SpeedyListIndex() {
			Clear();
		}

		/// <summary>
		/// インデックスを再構築する.
		/// </summary>
		public void Refresh() {
			Clear();
			for (int i = 0; i < target.Count; i++) {
				T item = target[i];
				Add(item, i);
			}
		}

		/// <summary>
		/// インデックスをクリアする.
		/// </summary>
		public void Clear() {
			foreach (T item in indices.Keys) {
				indices[item].Clear();
			}
			indices.Clear();
			nullIndex.Clear();
			history.Clear();
			position = 0;
		}

		/// <summary>
		/// 要素を追加する.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="index"></param>
		public void Add(T item, int index) {
			// null の場合
			if (item == null) {
				nullIndex.Add(new IndexItem(index, position));
				NormalizeList(nullIndex);
				return;
			}

			// null 以外の場合
			if (indices.ContainsKey(item) == false) {
				indices.Add(item, new List<IndexItem>());
			}
			List<IndexItem> list = indices[item];
			list.Add(new IndexItem(index, position));
			NormalizeList(list);
		}

		/// <summary>
		/// 要素が含まれているか確認する.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Contains(T item) {
			// null の場合
			if (item == null) {
				return nullIndex.Count > 0;
			}

			// null 以外の場合
			return indices.ContainsKey(item);
		}

		/// <summary>
		/// 要素のインデックス番号を取得する.
		/// 見つからなかった場合は -1 を返す.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int IndexOf(T item) {
			// null の場合
			if (item == null) {
				if (nullIndex.Count <= 0) {
					return -1;
				}
				NormalizeList(nullIndex);
				return nullIndex[0].Index;
			}

			// null 以外の場合
			if (indices.ContainsKey(item) == false) {
				return -1;
			}
			List<IndexItem> list = indices[item];
			NormalizeList(list);
			return list[0].Index;
		}

		/// <summary>
		/// 要素のインデックス番号を取得する.
		/// 見つからなかった場合は -1 を返す.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		public int IndexOf(T item, int index) {
			// null の場合
			if (item == null) {
				if (nullIndex.Count <= 0) {
					return -1;
				}
				NormalizeList(nullIndex);
				return FindIndex(nullIndex, index);
			}

			// null 以外の場合
			if (indices.ContainsKey(item) == false) {
				return -1;
			}
			List<IndexItem> list = indices[item];
			NormalizeList(list);
			return FindIndex(list, index);
		}

		/// <summary>
		/// 要素のインデックス番号を取得する.
		/// 見つからなかった場合は -1 を返す.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="index"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public int IndexOf(T item, int index, int count) {
			// null の場合
			if (item == null) {
				if (nullIndex.Count <= 0) {
					return -1;
				}
				NormalizeList(nullIndex);
				return FindIndex(nullIndex, index, count);
			}

			// null 以外の場合
			if (indices.ContainsKey(item) == false) {
				return -1;
			}
			List<IndexItem> list = indices[item];
			NormalizeList(list);
			return FindIndex(list, index, count);
		}

		/// <summary>
		/// 要素のインデックス番号を取得する.
		/// 見つからなかった場合は -1 を返す.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int LastIndexOf(T item) {
			// null の場合
			if (item == null) {
				if (nullIndex.Count <= 0) {
					return -1;
				}
				NormalizeList(nullIndex);
				return nullIndex[nullIndex.Count - 1].Index;
			}

			// null 以外の場合
			if (indices.ContainsKey(item) == false) {
				return -1;
			}
			List<IndexItem> list = indices[item];
			NormalizeList(list);
			return list[list.Count - 1].Index;
		}

		/// <summary>
		/// 要素のインデックス番号を取得する.
		/// 見つからなかった場合は -1 を返す.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		public int LastIndexOf(T item, int index) {
			// null の場合
			if (item == null) {
				if (nullIndex.Count <= 0) {
					return -1;
				}
				NormalizeList(nullIndex);
				return FindLastIndex(nullIndex, index);
			}

			// null 以外の場合
			if (indices.ContainsKey(item) == false) {
				return -1;
			}
			List<IndexItem> list = indices[item];
			NormalizeList(list);
			return FindLastIndex(list, index);
		}

		/// <summary>
		/// 要素のインデックス番号を取得する.
		/// 見つからなかった場合は -1 を返す.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="index"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public int LastIndexOf(T item, int index, int count) {
			// null の場合
			if (item == null) {
				if (nullIndex.Count <= 0) {
					return -1;
				}
				NormalizeList(nullIndex);
				return FindLastIndex(nullIndex, index, count);
			}

			// null 以外の場合
			if (indices.ContainsKey(item) == false) {
				return -1;
			}
			List<IndexItem> list = indices[item];
			NormalizeList(list);
			return FindLastIndex(list, index, count);
		}

		/// <summary>
		/// 要素を指定された場所に追加する.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="item"></param>
		public void Insert(int index, T item) {
			// 範囲外の場所に追加しようとした場合は, 例外を発生させる
			if (index < 0 || index > target.Count) {
				throw new IndexOutOfRangeException();
			}

			// index より後の要素を全てインクリメントする
			//IncrementIndex(index, 1);

			// 履歴に追加する
			AddHistory(index, 1);

			// 要素を追加する
			Add(item, index);
		}

		/// <summary>
		/// 要素を取り除く.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int Remove(T item) {
			// 要素が null の場合
			if (item == null) {
				return RemoveNullItem();
			}

			// null 以外の場合
			if (indices.ContainsKey(item) == false) {
				return -1;
			}
			List<IndexItem> list = indices[item];
			//if (list.Count <= 0) {
			//	return -1;
			//}
			NormalizeList(list);
			IndexItem indexItem = list[0];
			list.RemoveAt(0);
			if (list.Count <= 0) {
				indices.Remove(item);
			}
			//Debug.WriteLine("count: " + list.Count);
			//IncrementIndex(index.Index, -1);
			//int index = GetIndex(indexItem);
			int index = indexItem.Index;
			AddHistory(index + 1, -1);
			return index;
		}

		public void RemoveAll(Predicate<T> match) {
			T[] keys = new T[indices.Count];
			indices.Keys.CopyTo(keys, 0);
			foreach (T key in keys) {
				if (match.Invoke(key)) {
					Remove(key);
				}
			}
		}

		/// <summary>
		/// 要素を取り除く.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		public void RemoveAt(T item, int index) {
			// 要素が null の場合
			if (item == null) {
				RemoveAtNullItem(index);
				return;
			}

			// null 以外の場合
			if (indices.ContainsKey(item) == false) {
				return;
			}
			List<IndexItem> list = indices[item];
			NormalizeList(list);

			int listIndex = -1;
			int length = list.Count;
			for (int i = 0; i < length; i++) {
				IndexItem indexItem = list[i];
				if (indexItem.Index == index) {
					listIndex = i;
					break;
				}
			}
			if (listIndex < 0) {
				return;
			}
			list.RemoveAt(listIndex);
			if (list.Count <= 0) {
				indices.Remove(item);
			}

			AddHistory(index + 1, -1);
		}

		/// <summary>
		/// 要素が null の場合の Remove().
		/// </summary>
		/// <returns></returns>
		private int RemoveNullItem() {
			if (nullIndex.Count <= 0) {
				return -1;
			}
			NormalizeList(nullIndex);
			IndexItem indexItem = nullIndex[0];
			nullIndex.RemoveAt(0);
			int index = GetIndex(indexItem);
			AddHistory(index + 1, -1);
			return index;
		}

		/// <summary>
		/// 要素が null の場合の RemoveAt().
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		private void RemoveAtNullItem(int index) {
			if (nullIndex.Count <= 0) {
				return;
			}
			NormalizeList(nullIndex);
			int listIndex = -1;
			for (int i = 0; i < nullIndex.Count; i++) {
				IndexItem indexItem = nullIndex[i];
				if (indexItem.Index == index) {
					listIndex = i;
					break;
				}
			}
			if (listIndex < 0) {
				return;
			}
			nullIndex.RemoveAt(listIndex);
			AddHistory(index + 1, -1);
		}


		/// <summary>
		/// 操作履歴を追加する.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="move"></param>
		private void AddHistory(int index, int move) {
			/*
			if (position > PositionThreshold) {
				Refresh();
				return;
			}
			*/
			history.Add(new HistoryItem(index, move));
			position++;
		}

		/// <summary>
		/// リストのインデックスの位置と並び順を, 
		/// 操作履歴反映済みの状態にする.
		/// </summary>
		/// <param name="list"></param>
		private void NormalizeList(List<IndexItem> list) {
			int count = 0;
			foreach (IndexItem item in list) {
				if (item.InsertedAt == position) {
					continue;
				}
				count++;
				int index = GetIndex(item);
				item.Index = index;
				item.InsertedAt = position;
			}
			if (count > 0) {
				list.Sort();
			}
		}

		/// <summary>
		/// 操作履歴を反映したインデックス番号を返す.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		private int GetIndex(IndexItem item) {
			int index = item.Index;
			int length = history.Count;
			for (int i = item.InsertedAt; i < length; i++) {
				HistoryItem historyItem = history[i];
				if (index >= historyItem.Index) {
					index += historyItem.Move;
				}
			}
			return index;
		}

		/// <summary>
		/// インデックスのリストから, 指定されたインデックス番号以上のインデックス番号を探す.
		/// 見つからなかった場合 -1 を返す.
		/// </summary>
		/// <param name="list"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		private int FindIndex(List<IndexItem> list, int index) {
			foreach (IndexItem item in list) {
				if (item.Index >= index) {
					return item.Index;
				}
			}
			return -1;
		}

		/// <summary>
		/// インデックスのリストから, 指定されたインデックス番号以上のインデックス番号を探す.
		/// 見つからなかった場合 -1 を返す.
		/// </summary>
		/// <param name="list"></param>
		/// <param name="index"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		private int FindIndex(List<IndexItem> list, int index, int count) {
			foreach (IndexItem item in list) {
				int i = item.Index;
				if (i >= index && i < index + count) {
					return item.Index;
				}
			}
			return -1;
		}

		/// <summary>
		/// インデックスのリストから, 指定されたインデックス番号以上のインデックス番号を探す.
		/// 見つからなかった場合 -1 を返す.
		/// </summary>
		/// <param name="list"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		private int FindLastIndex(List<IndexItem> list, int index) {
			for (int i = list.Count - 1; i >= 0; i--) {
				IndexItem item = list[i];
				if (item.Index <= index) {
					return item.Index;
				}
			}
			return -1;
		}

		/// <summary>
		/// インデックスのリストから, 指定されたインデックス番号以上のインデックス番号を探す.
		/// 見つからなかった場合 -1 を返す.
		/// </summary>
		/// <param name="list"></param>
		/// <param name="index"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		private int FindLastIndex(List<IndexItem> list, int index, int count) {
			for (int i = list.Count - 1; i >= 0; i--) {
				IndexItem item = list[i];
				int itemIndex = item.Index;
				if (itemIndex <= index && itemIndex > index - count) {
					return item.Index;
				}
			}
			return -1;
		}

		/*
		private IndexItem GetFirstItem(List<IndexItem> list) {
			IndexItem result = list[0];
			int index = GetIndex(result);
			foreach (IndexItem item in list) {
				int i = GetIndex(item);
				if (index > i) {
					index = i;
					result = item;
				}
			}
			return result;
		}
		*/

		/*
		private void IncrementIndex(int index, int value) {
			foreach (T key in indices.Keys) {
				List<IndexItem> list = indices[key];
				for (int i = 0; i < list.Count; i++) {
					if (list[i].Index >= index) {
						list[i].Index += value;
					}
				}
			}
			for (int i = 0; i < nullIndex.Count; i++) {
				if (nullIndex[i].Index >= index) {
					nullIndex[i].Index += value;
				}
			}
		}
		*/
	}
}
