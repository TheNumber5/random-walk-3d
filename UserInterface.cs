using UnityEngine;
using UnityEngine.UIElements;
public class UserInterface : MonoBehaviour {
	private Toggle colorChangeToggle, cameraRot;
	private Slider timeSlider, maxDist;
	private Label timeSeconds, bottomLabel, maxDistLabel, FPS;
	private Button start, pause, stop;
	private VisualElement rootObject, mainObj;
	private TextField totalAtoms;
	public Simulator sim;
	public CameraRotate cam;
	void Awake() {
		mainObj = GetComponent<UIDocument>().rootVisualElement;
		colorChangeToggle = mainObj.Q("ColorToggle") as Toggle;
		colorChangeToggle.RegisterCallback<ClickEvent>(ColorChange);
		timeSlider = mainObj.Q("SpawnTimeSlider") as Slider;
		timeSlider.RegisterValueChangedCallback(TimeChange);
		timeSeconds = mainObj.Q("TimeSeconds") as Label;
		start = mainObj.Q("StartButton") as Button;
		start.RegisterCallback<ClickEvent>(StartNow);
		stop = mainObj.Q("Stop") as Button;
		stop.RegisterCallback<ClickEvent>(StopNow);
		pause = mainObj.Q("Pause") as Button;
		pause.RegisterCallback<ClickEvent>(PauseNow);
		bottomLabel = mainObj.Q("BottomLabel") as Label;
		rootObject = mainObj.Q("TopContainer");
		cameraRot = mainObj.Q("CameraRotate") as Toggle;
		cameraRot.RegisterCallback<ClickEvent>(CameraRotateChange);
		totalAtoms = mainObj.Q("TotalAtoms") as TextField;
		totalAtoms.RegisterValueChangedCallback(TotalAtomsChanged);
		maxDist = mainObj.Q("MaxDist") as Slider;
		maxDist.RegisterValueChangedCallback(ChangeMaxDist);
		maxDistLabel = mainObj.Q("MaxDistLabel") as Label;
		FPS = mainObj.Q("FPS") as Label;
	}
	//This is horrible but it works, somehow, approximatively
	void ColorChange(ClickEvent mainEvent) {
		sim.withColor = !sim.withColor;
	}
	void StartNow(ClickEvent mainEvent) {
		sim.BeginSimulation();
	}
	void PauseNow(ClickEvent mainEvent) {
		sim.PauseSimulation();
	}
	void StopNow(ClickEvent mainEvent) {
		sim.StopSimulation();
	}
	void TimeChange(ChangeEvent<float> mainEvent) {
		sim.waitTime = timeSlider.value;
		timeSeconds.text = sim.waitTime.ToString("F2") + " s";
	}
	void CameraRotateChange(ClickEvent mainEvent) {
		cam.isRotating = !cam.isRotating; //Don't even ask what is this
	}
	void TotalAtomsChanged(ChangeEvent<string> mainEvent) {
		sim.totalAtoms = int.Parse(mainEvent.newValue);
	}
	void ChangeMaxDist(ChangeEvent<float> mainEvent) {
		sim.maxMov = maxDist.value;
		maxDistLabel.text = sim.maxMov.ToString("F2") + " m";
	}
	void Start() {
		Screen.SetResolution(1920, 1080, true);
		Application.targetFrameRate = 200;
	}
	void Update() {
		if(Input.GetKeyDown(KeyCode.Space)) {
			bottomLabel.style.opacity = 0;
			rootObject.style.opacity = 1f-rootObject.resolvedStyle.opacity;
		}
		if(Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
			Debug.Log("Exited program");
		}
		if(Time.timeScale!=0)
		FPS.text = (1f/Time.smoothDeltaTime).ToString("F0") + " FPS";
	}
}
