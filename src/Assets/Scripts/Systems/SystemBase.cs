using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SystemBase : MonoBehaviour {

	private static Dictionary<Type, ArrayList> components = new Dictionary<Type, ArrayList>();

	public static void Register<T>(T component) where T: ComponentBase {
		Type type = component.GetType();
		if (!components.ContainsKey(type))
			components.Add(type, new ArrayList());

		components[type].Add(component);

//		print("Registered component of type " + type);
	}

	public static void Unregister<T>(T component) where T: ComponentBase {
		Type type = component.GetType();
		if (components.ContainsKey(type)) {
			components[type].Remove(component);
		}

//		print("Unregistered component of type " + type);
	}

	public static View<T> GetEntities<T>() where T: ComponentBase {
		Type type = typeof(T);
		if (!components.ContainsKey(type))
			components.Add(type, new ArrayList());

//		print("Getting entities with component of type " + type);

		return new View<T>(components[type]);
	}

	public class View<T>: ICollection<T> {
		public int Count {
			get { return list.Count; }
		}

		public bool IsReadOnly {
			get { return true; }
		}
		
		public T this [int index] {
			get { return (T) list[index]; }
			set { list[index] = value; }
		}

		private ArrayList list;

		public View(ArrayList backingList) {
			list = backingList;
		}
		public void Add(T item) {
			list.Add(item);
		}

		public void Clear() {
			list.Clear();
		}

		public bool Contains(T item) {
			return list.Contains(item);
		}

		public void CopyTo(T[] array, int arrayIndex) {
			throw new NotImplementedException();
		}

		public bool Remove(T item) {
			throw new InvalidOperationException("View<T> is immutable.");
		}
		public IEnumerator<T> GetEnumerator() {
			throw new NotImplementedException();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			throw new NotImplementedException();
		}
	}


/*
	private static Dictionary<Type, object> components = new Dictionary<Type, object>();

	public static void Register<T>(T component) where T: ComponentBase {
		Type type = component.GetType();
		if (!components.ContainsKey(type))
			components.Add(type, new List<T>());

		List<T> list = (List<T>) components[type];
		list.Add(component);

		print("Registered component of type " + type);
	}

	public static void Unregister<T>(T component) where T: ComponentBase {
		Type type = component.GetType();
		if (components.ContainsKey(type)) {
			List<T> list = (List<T>) components[type];
			list.Remove(component);
		}

		print("Unregistered component of type " + type);
	}

	public static List<T> GetEntities<T>() where T: ComponentBase {
		Type type = typeof(T);
		if (!components.ContainsKey(type))
			components.Add(type, new List<T>());

		print("Getting entities with component of type " + type);

		return (List<T>) components[type];
	}
*/

/*
	private static Dictionary<Type, List<GameObject>> entitiesByComponent = new Dictionary<Type, List<GameObject>>();

	public static void Register<T>(T component) where T: ComponentBase {
		Type type = typeof(T);
		if (!entitiesByComponent.ContainsKey(type))
			entitiesByComponent.Add(type, new List<GameObject>());
		entitiesByComponent[type].Add(component.gameObject);
	}

	public static void Unregister<T>(T component) where T: ComponentBase {
		Type type = typeof(T);
		if (entitiesByComponent.ContainsKey(type))
			entitiesByComponent[type].Remove(component.gameObject);
	}

	public static List<GameObject> GetEntities<T>() where T: ComponentBase {
		Type type = typeof(T);
		if (!entitiesByComponent.ContainsKey(type))
			entitiesByComponent.Add(type, new List<GameObject>());
		return entitiesByComponent[type];
	}
*/

}
