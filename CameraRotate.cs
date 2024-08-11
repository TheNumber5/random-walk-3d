using UnityEngine;

public class CameraRotate : MonoBehaviour {
	public bool isRotating;
	public float rotateSpeed;
    void Update() {
		if(isRotating) {
			this.transform.RotateAround(Vector3.zero, Vector3.up, rotateSpeed*Time.deltaTime);
		}
    }
}
