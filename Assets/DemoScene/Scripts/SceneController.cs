using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.iOS;
using UnityEngine.EventSystems;

public class SceneController : MonoBehaviour {

	public GameObject asset;

	private bool isTapEnabled;

	// Use this for initialization
	void Start () {
		isTapEnabled = true;
	}

	// Update is called once per frame
	void Update () {


		if (Input.touchCount > 0) {
			if(IsPointerOverUI()) {
				return;
			}
		}
	}

	public void AddAsset() {
		if (asset != null && GetComponent<PlaneDetection>().SquareState == PlaneDetection.FocusState.Found) {
			GameObject obj = Instantiate (asset);
			var screenPosition = Camera.main.ScreenToViewportPoint(new Vector3(Screen.width/2,Screen.height/2,0));
			ARPoint point = new ARPoint {
				x = screenPosition.x,
				y = screenPosition.y
			};

				// prioritize reults types
			ARHitTestResultType[] resultTypes = {
				ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent, 
				// if you want to use infinite planes use this:
				//ARHitTestResultType.ARHitTestResultTypeExistingPlane,
				ARHitTestResultType.ARHitTestResultTypeHorizontalPlane, 
				ARHitTestResultType.ARHitTestResultTypeFeaturePoint
			}; 

			foreach (ARHitTestResultType resultType in resultTypes)
			{
				if (HitTestWithResultType (point, resultType, obj))
				{
					return;
				}
			}
		}
	}

	bool HitTestWithResultType (ARPoint point, ARHitTestResultType resultTypes, GameObject target)
	{
		List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface ().HitTest (point, resultTypes);
		if (hitResults.Count > 0) {
			foreach (var hitResult in hitResults) {
				Debug.Log ("Got hit!");
				target.transform.position = UnityARMatrixOps.GetPosition (hitResult.worldTransform);
				Vector3 up = Vector3.up;
				Vector3 right = Vector3.Cross(up, Camera.main.transform.forward).normalized;
				Vector3 forward = - Vector3.Cross(right, up).normalized;
				target.transform.rotation = Quaternion.LookRotation(forward,up);
				return true;
			}
		}
		return false;
	}

	private bool IsPointerOverUI() {
		PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
		eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		List<RaycastResult> results = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
		return results.Count > 0;
	}
}
