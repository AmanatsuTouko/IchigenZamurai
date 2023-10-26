using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyConDemo2 : MonoBehaviour
{

	private List<Joycon> joycons;

	// Values made available via Unity
	public float[] stick;
	public Vector3 gyro;
	public Vector3 accel;
	public int jc_ind = 0;
	public Quaternion orientation;


	void Start()
	{
		gyro = new Vector3(0, 0, 0);
		accel = new Vector3(0, 0, 0);
		// get the public Joycon array attached to the JoyconManager in scene
		joycons = JoyconManager.Instance.j;
		if (joycons.Count < jc_ind + 1)
		{
			Destroy(gameObject);
		}
	}
	
	// Update is called once per frame
	void Update()
	{
		// make sure the Joycon only gets checked if attached
		if (joycons.Count > 0)
		{
			Joycon j = joycons[jc_ind];
			// GetButtonDown checks if a button has been pressed (not held)
			if (j.GetButtonDown(Joycon.Button.SHOULDER_2))
			{
				Debug.Log("Shoulder button 2 pressed");
				// GetStick returns a 2-element vector with x/y joystick components
				Debug.Log(string.Format("Stick x: {0:N} Stick y: {1:N}", j.GetStick()[0], j.GetStick()[1]));

				// Joycon has no magnetometer, so it cannot accurately determine its yaw value. Joycon.Recenter allows the user to reset the yaw value.
				j.Recenter();
			}
			// GetButtonDown checks if a button has been released
			if (j.GetButtonUp(Joycon.Button.SHOULDER_2))
			{
				Debug.Log("Shoulder button 2 released");
			}
			// GetButtonDown checks if a button is currently down (pressed or held)
			if (j.GetButton(Joycon.Button.SHOULDER_2))
			{
				Debug.Log("Shoulder button 2 held");
			}

			if (j.GetButtonDown(Joycon.Button.DPAD_DOWN))
			{
				Debug.Log("Rumble");

				// Rumble for 200 milliseconds, with low frequency rumble at 160 Hz and high frequency rumble at 320 Hz. For more information check:
				// https://github.com/dekuNukem/Nintendo_Switch_Reverse_Engineering/blob/master/rumble_data_table.md

				j.SetRumble(160, 320, 0.6f, 200);

				// The last argument (time) in SetRumble is optional. Call it with three arguments to turn it on without telling it when to turn off.
				// (Useful for dynamically changing rumble values.)
				// Then call SetRumble(0,0,0) when you want to turn it off.
			}

			stick = j.GetStick();

			// Gyro values: x, y, z axis values (in radians per second)
			gyro = j.GetGyro();

			// Accel values:  x, y, z axis values (in Gs)
			accel = j.GetAccel();

			orientation = j.GetVector();

			Quaternion vec = orientation;

			//vec.x = 0.0f;
			//vec.y = 0.0f;
			//vec.z = 0.0f;

			/*
			vec.x = 0.0f;
			//vec.y = 0.0f;
			vec.z = 0.0f;
			 */
			//vec = new Quaternion(0.0f, vec.y, 0.0f, vec.w);

			//Vector3 transVec = vec.eulerAngles;
			//Debug.Log(transVec);

			//float posX = (transVec.z - 180) / 10;

			/*
			float r = 9.0f;
			float theta = 270 - transVec.z;
			float posX = r * Mathf.Cos(theta / 180 * Mathf.PI);
			
			float theta2 = 180 - transVec.y;
			float posY = r * Mathf.Cos(theta2 / 180 * Mathf.PI);
			*/

			//this.gameObject.transform.position = new Vector3(posX, posY, 0);
			//this.gameObject.transform.position = new Vector3(posX, 0, 0);
			//this.gameObject.transform.position = new Vector3(0, posY, 0);
			//this.gameObject.transform.position = transVec;

			//gameObject.transform.rotation = orientation;
			gameObject.transform.rotation = vec;

			//Debug.Log(transform.up);

			if (j.GetButton(Joycon.Button.DPAD_UP))
			{
				gameObject.GetComponent<Renderer>().material.color = Color.red;

				//ジャイロの上方向のリセット
				//現在の値を取得して保存する
				//ResetBaseVectorZ = vec.z;

				//0との差を取得する
				//ResetBaseVectorZ =  Mathf.Abs(vec.z);

				//リセット
				//useVecZ = 0.0f;
				j.Recenter();
			}
			else
			{
				gameObject.GetComponent<Renderer>().material.color = Color.blue;
			}
		}
	}
}