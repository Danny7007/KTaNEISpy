using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Rnd = UnityEngine.Random;

public class ClickableIcon : MonoBehaviour {

	public KMSelectable selectable;
	public ISpyScript parentScript { private get; set; }

	public KMBombModule hostMod { private get; set; }
	public int priority { get; set; }

	public SpriteRenderer sprite;
	[SerializeField]
	private new MeshCollider collider;
	[SerializeField]
	private List<GameObject> instantiatedObjects = new List<GameObject>();

	// Use this for initialization
	IEnumerator Start () {
		yield return null;
		selectable.OnInteract += delegate () 
		{
			parentScript.CollectIcon(priority);
			return false; 
		};
	}
	public void SetUpHost()
    {
		KMSelectable parentSel = hostMod.GetComponent<KMSelectable>();
		Array.Resize(ref parentSel.Children, parentSel.Children.Length + 1);
		parentSel.Children[parentSel.Children.Length - 1] = this.selectable;
		parentSel.UpdateChildren();

		selectable.Parent = parentSel;

		hostMod.OnPass += delegate { transform.parent = hostMod.transform.parent; return false; };
    }
	public void PlaceSelf()
    {
		this.transform.SetParent(hostMod.transform);
		this.transform.localPosition = new Vector3(Rnd.Range(-0.06f, 0.06f), 0.0075f, Rnd.Range(0.065f, -0.075f));
		this.transform.localEulerAngles = Vector3.zero;

		List<Collider> allHostColliders = hostMod.GetComponentsInChildren<MeshFilter>()
			.Where(meshFilter => meshFilter != null && meshFilter.mesh != null)
			.Select(x => MakeMesh(x.mesh, x.gameObject))
			.Where(col => !col.Equals(this.collider))
			.ToList();
		Vector3 _1;
		float _2;
		Debug.Log("Placeself");
		while (allHostColliders.Any(hostCol =>
				Physics.ComputePenetration(hostCol, hostCol.transform.position, hostCol.transform.rotation,
				collider, collider.transform.position, collider.transform.rotation,
				out _1, out _2)))
			this.transform.localPosition += 0.001f * Vector3.up;
		//foreach (GameObject obj in instantiatedObjects)
		//	Destroy(obj);
    }

	Collider MakeMesh(Mesh mesh, GameObject obj)
    {
		GameObject newObj = Instantiate(obj, obj.transform.position, obj.transform.rotation, obj.transform);
		instantiatedObjects.Add(newObj);
		MeshCollider mcol = newObj.AddComponent<MeshCollider>();
		mcol.sharedMesh = mesh;
		return mcol;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
