using System.Collections.Generic;

namespace Hikipuro.Collections {
	/// <summary>
	/// SpeedyList で使用する, インデックス管理用クラス.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	class SpeedyListIndex<T> {
		/// <summary>
		/// 処理対象のリスト.
		/// </summary>
		readonly List<T> target;

		/// <summary>
		/// インデックスの管理用.
		/// </summary>
		Dictionary<T, List<int>> indices;

		/// <summary>
		/// コンストラクタ.
		/// </summary>
		/// <param name="list">処理対象のリスト.</param>
		public SpeedyListIndex(List<T> list) {
			target = list;
			indices = new Dictionary<T, List<int>>();
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
		}

		/// <summary>
		/// 要素を追加する.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="index"></param>
		public void Add(T item, int index) {
			if (indices.ContainsKey(item) == false) {
				indices.Add(item, new List<int>());
			}
			List<int> list = indices[item];
			list.Add(index);
			list.Sort();
		}

		/// <summary>
		/// 要素が含まれているか確認する.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Contains(T item) {
			return indices.ContainsKey(item);
		}

		/// <summary>
		/// 要素のインデックス番号を取得する.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int IndexOf(T item) {
			if (indices.ContainsKey(item) == false) {
				return -1;
			}
			List<int> list = indices[item];
			if (list.Count <= 0) {
				return -1;
			}
			return list[0];
		}

		/// <summary>
		/// 要素を指定された場所に追加する.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="item"></param>
		public void Insert(int index, T item) {
			Add(item, index);
			for (int i = index; i < target.Count; i++) {
				List<int> list = indices[target[i]];
				for (int n = 0; n < list.Count; n++) {
					if (list[n] >= index) {
						list[n]++;
					}
				}
			}
		}

		/// <summary>
		/// 要素を取り除く.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int Remove(T item) {
			if (indices.ContainsKey(item) == false) {
				return -1;
			}
			List<int> list = indices[item];
			if (list.Count <= 0) {
				return -1;
			}
			int index = list[0];
			list.RemoveAt(0);
			return index;
		}
	}
}
