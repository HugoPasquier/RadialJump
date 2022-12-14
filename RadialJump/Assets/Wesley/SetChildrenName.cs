using System;
using UnityEngine;

/// <summary>
/// Rename all child game object by the father name
/// </summary>
public class SetChildrenName : MonoBehaviour
{
	[SerializeField] private string _childrenName = "";

	private void OnValidate() => SetName();

    [ContextMenu("Set Children Name", false, 0)]
	public void SetName()
	{
		Transform parent = transform;

		if (String.Compare(_childrenName, "", StringComparison.Ordinal) == 0)
			_childrenName = parent.name;

		for (int i = 0; i < transform.childCount; i++)
			parent.GetChild(i).name = $"{_childrenName}_{i}";
	}
}
