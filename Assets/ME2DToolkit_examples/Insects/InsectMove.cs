using UnityEngine;
using System.Collections;

public class InsectMove : MonoBehaviour
{
	public float speed = 5f;
	public float turningSpeed = 15f;
	private Rect screenBounds;
	private Vector3 target;
	private Transform tr;
	private const float MIN_TARGET_DISTANCE = 3f;

	// Use this for initialization
	void Start ()
	{
		tr = transform;
		screenBounds = new Rect (-4f * (float)Screen.width / (float)Screen.height, -4f, 8f * (float)Screen.width / (float)Screen.height, 8f);
		target = new Vector3 (Random.Range (screenBounds.xMin, screenBounds.xMax), Random.Range (screenBounds.yMin, screenBounds.yMax), 0);
	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 nextMove = -Vector3.right * speed * Time.deltaTime;
		float distance = Vector3.Distance (tr.position, target);
		
		if (distance * distance < nextMove.sqrMagnitude) {
			target = GetNewTarget ();
		}
		tr.Translate (nextMove);
		tr.rotation = Quaternion.Slerp (
			tr.rotation,
			Quaternion.Euler (0f, 0f, Vector3.Angle (target - tr.position, -Vector3.right) * (target.y > tr.position.y ? -1f : 1f)),
			Time.deltaTime * turningSpeed);
		
		if (!screenBounds.Contains (tr.position)) {
			tr.position = ClosestPointToRect (screenBounds, tr.position);
		}
	}
	
	/// <summary>
	/// Gets the new target in space.
	/// </summary>
	/// <returns>
	/// The new target.
	/// </returns>
	Vector3 GetNewTarget ()
	{
		Vector3 ret;
		
		ret = new Vector3 (Random.Range (screenBounds.xMin, screenBounds.xMax), Random.Range (screenBounds.yMin, screenBounds.yMax), 0);
		if (Vector3.Distance (tr.position, ret) < MIN_TARGET_DISTANCE) {
			ret = GetNewTarget ();
		}
		
		return ret;
	}
	
	/// <summary>
	/// Returns closests point to the rectangle.
	/// </summary>
	/// <returns>
	/// The point near rect.
	/// </returns>
	/// <param name='rect'>
	/// Rectangle.
	/// </param>
	/// <param name='point'>
	/// Point in space.
	/// </param>
	private Vector3 ClosestPointToRect (Rect rect, Vector3 point)
	{
		Vector3 ret = Vector3.zero;
		
		if (point.x < rect.xMin) {
			ret.x = rect.xMin;
		} else if (point.x > rect.xMax) {
			ret.x = rect.xMax;
		} else {
			ret.x = point.x;
		}
		
		if (point.y < rect.yMin) {
			ret.y = rect.yMin;
		} else if (point.y > rect.yMax) {
			ret.y = rect.yMax;
		} else {
			ret.y = point.y;
		}
		
		return ret;
	}
}
